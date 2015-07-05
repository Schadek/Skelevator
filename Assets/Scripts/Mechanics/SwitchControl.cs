using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum Entity
{
    None,
    Human,
    Dog
}

//A template for all the components a character needs. Can be extended at will
[System.Serializable]
public class CharacterComponents
{
    public Entity identification;
    public GameObject mainObject;
    public Transform cameraFocus;
    public Transform idealCameraPosition;
    public ThirdPersonCharacter characterMotor;
    public ThirdPersonUserControl characterController;
}

/* A few notes about understanding this script: Every character object has a child object marked as an 'ideal position' where the camera 'wants' to be.
 * When the camera gets assigned another focus object, it switches world space position to the ideal position of the intended character
 * When switching to another character the old character looses its rigidbody to prevent unforeseen interaction with the environment
 * Also, the new character gets a brand new rigidbody (To this date rigidbodies cannot be disabled/enabled at will [Unity 5.1])
 * Every character has certain self-explanatory values associated with it which you can find in the CharacterComponents class */

public class SwitchControl : MonoBehaviour
{

    public CharacterComponents human;
    public CharacterComponents dog;
    private static CharacterComponents controlledCharacterComponents;

    //The texture that hides the screen when changing controls
    public Image fadeTexture;
    public Entity startingCharacter;

    private Transform mainCam;
    private CameraBoom boom;
    private float fadeTime = 1f;
    private bool currentlySwitching;
    public static bool HumanHasSkull { get; set; }
    public static CharacterComponents ControlledCharacter
    {
        get { return controlledCharacterComponents; }
    }

    //Event for switching the controlled entity
    public static event SwitchedEntities Switched;
    public delegate void SwitchedEntities(CharacterComponents entity);
    /*********************************************************************************************************************************************************************************/


    private void Awake()
    {
        InputManager.ButtonPress += HandleInput;
    }

    private void Start()
    {
        fadeTexture.color = new Color(fadeTexture.color.r, fadeTexture.color.g, fadeTexture.color.b, 0);
        mainCam = Camera.main.gameObject.transform;
        boom = mainCam.GetComponent<CameraBoom>();

        //Initialize starting character
        switch (startingCharacter)
        {
            case Entity.Human:
                ChangeTo(human);
                break;
            case Entity.Dog:
                ChangeTo(dog);
                break;
        }
    }

    private bool HasHumanSkull()
    {
        if (MainCharacterAttributes.Instance.heldSkull != Skulls.None)
        {
            return true;
        }

        if (GameInstance.GetInstance().debugMode)
        {
            return true;
        }
        return false;
    }

    private void HandleInput(UserInput button, InputState state)
    {
        //This only gets triggered when the pressed button is R1, the controls are currently not changing and the human wears a skull.
        //If you forcefully want to change the controlled entity, use ForceChangeTo()
        //Ugly if statement incoming:
        if (state == InputState.Ingame && button == UserInput.R1 && !currentlySwitching && HasHumanSkull() && ControlledCharacter.characterMotor.isGrounded)
        {
            switch (controlledCharacterComponents.identification)
            {
                case Entity.Human:
                    //If the currently controlled character is the human, switch to the dog
                    StartCoroutine(SwitchControlRoutine(dog));
                    break;
                case Entity.Dog:
                    //If the currently controlled character is the dog, switch to the human
                    StartCoroutine(SwitchControlRoutine(human));
                    break;
            }
        }
    }

    private void ChangeTo(CharacterComponents comps)
    {
        //If there is no current character controller installed => first time initializing one
        if (controlledCharacterComponents != null)
        {
            Rigidbody rBody = controlledCharacterComponents.characterMotor.GetComponent<Rigidbody>();
            Animator anim = controlledCharacterComponents.characterMotor.GetComponent<Animator>();

            controlledCharacterComponents.characterMotor.enabled = false;
            controlledCharacterComponents.characterController.enabled = false;

            //To assure that nothing can influence the inactive character, we destroy its rigidbody component
            Destroy(rBody);

            //To assure that no leftover animation blending value remains on the inactive character, we manually nullify them
            anim.SetFloat("Forward", 0f);
            anim.SetFloat("Turn", 0f);
        }

        //Change the camera behaviour to the specified target and its focus
        boom.IdealPosition = comps.idealCameraPosition;
        boom.Target = comps.cameraFocus;

        //Enable the controller components
        comps.characterMotor.enabled = true;
        comps.characterController.enabled = true;

        //Remove or add a rigidbody component
        comps.characterMotor.gameObject.AddComponent<Rigidbody>();
        comps.characterMotor.ReassignRigidbody();

        //Lastly we assign our new component container to our currently controlled one and invoke our event
        controlledCharacterComponents = comps;
        Switched(comps);
    }

    public void ForceChangeTo(Entity entity)
    {
        //First we have to stop all kind of fading or switching etc.
        StopAllCoroutines();

        switch (entity)
        {
            case Entity.Human:
                StartCoroutine(SwitchControlRoutine(human));
                break;
            case Entity.Dog:
                StartCoroutine(SwitchControlRoutine(dog));
                break;
        }
    }

    private IEnumerator SwitchControlRoutine(CharacterComponents comps)
    {
        Color newColor = fadeTexture.color;

        //Mark the gameState as 'switching' to prevent changing controls while already changing controls
        currentlySwitching = true;
        //The farther the two bodies are away from each other, the longer it takes for the texture to fade to black. This is limited to one second.
        fadeTime = Mathf.Min(Vector3.Distance(controlledCharacterComponents.characterMotor.transform.position, comps.characterMotor.transform.position) * 0.1f, 1f);
        boom.IdealPosition = null;

        float counter = 0f;
        //While the overlay texture (most likely completely black) is not complete blocking the view, increase its opaqueness
        while (counter < 1f)
        {
            counter += Time.deltaTime * (1 / fadeTime);
            newColor.a = Mathf.SmoothStep(0f, 1f, counter);
            fadeTexture.color = newColor;
            yield return null;
        }

        //Now we disable the current body and enable to body that we will possess after the change
        switch (comps.identification)
        {
            case Entity.Human:
                ChangeTo(human);
                break;
            case Entity.Dog:
                ChangeTo(dog);
                break;
        }

        //Finally we wait for one second before the overlay texture starts to fade out again
        yield return new WaitForSeconds(1f);
        StartCoroutine(RegainControl());
    }

    //Fades the screen back to its normal stance + gives player control of new character
    private IEnumerator RegainControl()
    {
        Color newColor = fadeTexture.color;
        while (newColor.a > 0)
        {
            newColor.a -= Time.deltaTime * (1 / fadeTime);
            fadeTexture.color = newColor;
            yield return null;
        }

        //Last but not least we give control back to the character. Also from now on he/she is able to switch again
        currentlySwitching = false;
    }

    public static void NullifySwitched()
    {
        Switched = null;
    }
}
