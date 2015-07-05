using UnityEngine;
using System.Collections;

public class DogLift : MonoBehaviour, IInteractable
{
    public GadgetIdentifier keyGadget;
    [Space(15)]

    public Transform platform;
    public float height;
    public LayerMask platformLayer;
    public DogLiftPlatform platformScript;

    private bool lifted;
    private Vector3 groundedPosition;

    private void Start()
    {
        groundedPosition = platform.position;
    }

    public bool Execute(GadgetIdentifier ident)
    {
        if (ident == keyGadget && !platformScript.humanOnTop)
        {
            StopAllCoroutines();

            if (lifted)
            {
                StartCoroutine(LiftPlatform(false));
            }
            else
            {
                StartCoroutine(LiftPlatform(true));
            }
            return true;
        }
        return false;
    }

    private Transform CheckForDog()
    {
        Transform dogTransform = GameInstance.GetInstance().Player_Components.dog;
        RaycastHit hit;

        if (Physics.Raycast(dogTransform.position + new Vector3(0, 0.1f, 0), Vector3.down, out hit, 5, platformLayer.value))
        {
            //If we hit anything the hit platform is assigned as the dog's parent object
            dogTransform.SetParent(platform);
            return hit.transform;
        }

        //Otherwise we return the current parent object
        return dogTransform.parent;
    }

    private IEnumerator LiftPlatform(bool liftPlatform)
    {
        float counter = 0;
        Vector3 initialPosition = platform.position;

        Transform initialTransform = GameInstance.GetInstance().Player_Components.dog;
        Transform newParent = CheckForDog();

        lifted = !lifted;
        while (counter < 1)
        {
            counter += Time.deltaTime * 0.5f;
            if (liftPlatform)
            {
                platform.position = ExtensionMethods.SmoothStep(initialPosition, groundedPosition + new Vector3(0, height, 0), counter);
            }
            else
            {
                platform.position = ExtensionMethods.SmoothStep(initialPosition, groundedPosition, counter);
            }
            yield return null;
        }
        initialTransform.SetParent(newParent);
    }
}
