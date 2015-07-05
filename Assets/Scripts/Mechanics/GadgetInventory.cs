using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public struct ObjectDistance
{
    public GameObject obj;
    public float distance;
}

public class ObjectDistanceComparer : IComparer<ObjectDistance>
{
    public int Compare(ObjectDistance x, ObjectDistance y)
    {
        if (x.distance == y.distance)
            return 0;
        if (x.distance < y.distance)
            return -1;
        return 1;
    }
}

public class GadgetInventory : MonoBehaviour
{
    [System.Serializable]
    public struct UI_Elements
    {
        [Tooltip("The mask limiting the icon display")]
        public RectTransform iconMask;
        [Tooltip("The position a new icon spawns at on the left side")]
        public RectTransform spawnLeft;
        [Tooltip("The position a new icon spawns at on the right side")]
        public RectTransform spawnRight;
        [Tooltip("The Image component of the item left to the current one")]
        public Image previousGadget_IMG;
        [Tooltip("The Image component of the current item")]
        public Image currentGadget_IMG;
        [Tooltip("The Image component of the item right to the current one")]
        public Image nextGadget_IMG;
        [Tooltip("The prefab for a gadget icon")]
        public GameObject imagePrefab;
    }

    public UI_Elements uiElements;

    [Tooltip("Every gadget (except the jaw) the human can carry")]
    public Gadget[] allGadgets;
    [Tooltip("The jaw, the only gadget usable by the dog")]
    public Gadget jaw;
    [Tooltip("The transparency ")]
    [Range(0f, 1f)]
    public float inactiveAlpha = 0.7f;
    [Tooltip("The layer of interactable objects")]
    public LayerMask interactables;
    [Tooltip("The layer for dog interactable objects")]
    public LayerMask dogInteractables;
    [Tooltip("The speed at which the icons change place")]
    public float iconChangeSpeed = 4f;
    [Tooltip("Outline color for marked interactable objects")]
    public Color outlineColor;

    private List<Gadget> storedGadgets = new List<Gadget>();

    private Vector2 prevStaticPosition;
    private Vector2 curStaticPosition;
    private Vector2 nextStaticPosition;

    private Vector2 prevStaticSize;
    private Vector2 curStaticSize;
    private Vector2 nextStaticSize;

    private RectTransform prevTransform;
    private RectTransform curTransform;
    private RectTransform nextTransform;

    private int currentlyEquippedGadgetIndex;
    private bool currentlyChanging;

    private Color inactiveColor;
    private Color transparentColor = new Color(0, 0, 0, 0);

    private Transform humanInvokingPosition;
    private Transform dogInvokingPosition;
    private GameObject nearestObject;
    private bool isHuman;

    //The update function is replaced by a coroutine
    private IEnumerator currentInventoryRoutine;

    public ObjectDistance[] MarkedObjects { get; set; }
    public static GadgetInventory Instance { get; set; }

    private void Awake()
    {
        //Initialize Singleton
        Instance = this;

        //Subscribe functions to static events
        SwitchControl.Switched += ToggleInventoryVisibility;
        SwitchControl.Switched += ChangeHuman;
        SwitchControl.Switched += DemarkTarget;
        InputManager.ButtonPress += HandleInput;
    }

    private void Start()
    {
        //Setting up the singleton
        Instance = this;

        //Get the transform of the human character
        humanInvokingPosition = GetComponent<GameInstance>().Player_Components.invokingPosition;

        //Setup color
        inactiveColor = new Color(1, 1, 1, inactiveAlpha);

        //Setup the transform positions
        prevStaticPosition = uiElements.previousGadget_IMG.gameObject.GetComponent<RectTransform>().anchoredPosition;
        curStaticPosition = uiElements.currentGadget_IMG.gameObject.GetComponent<RectTransform>().anchoredPosition;
        nextStaticPosition = uiElements.nextGadget_IMG.gameObject.GetComponent<RectTransform>().anchoredPosition;

        prevStaticSize = uiElements.previousGadget_IMG.gameObject.GetComponent<RectTransform>().sizeDelta;
        curStaticSize = uiElements.currentGadget_IMG.gameObject.GetComponent<RectTransform>().sizeDelta;
        nextStaticSize = uiElements.nextGadget_IMG.gameObject.GetComponent<RectTransform>().sizeDelta;

        prevTransform = uiElements.previousGadget_IMG.gameObject.GetComponent<RectTransform>();
        curTransform = uiElements.currentGadget_IMG.gameObject.GetComponent<RectTransform>();
        nextTransform = uiElements.nextGadget_IMG.gameObject.GetComponent<RectTransform>();

        //Add appropriate items to the inventory. !DebugMode = Hands, otherwise everything
        AddItemsToInventory();

        //Set the images to null. In the main menu there are some placeholder graphics for lazyness' sake
        uiElements.previousGadget_IMG.sprite = null;
        uiElements.currentGadget_IMG.sprite = null;
        uiElements.nextGadget_IMG.sprite = null;
        uiElements.previousGadget_IMG.color = transparentColor;
        uiElements.currentGadget_IMG.color = transparentColor;
        uiElements.nextGadget_IMG.color = transparentColor;

        if (allGadgets.Length > 0 && storedGadgets.Count > 0)
        {
            uiElements.currentGadget_IMG.sprite = storedGadgets[0].icon;
            uiElements.previousGadget_IMG.sprite = GetPrevGadget(0).icon;
            uiElements.nextGadget_IMG.sprite = GetNextGadget(0).icon;

            if (uiElements.currentGadget_IMG.sprite)
            {
                uiElements.currentGadget_IMG.color = Color.white;
            }
            if (uiElements.previousGadget_IMG.sprite)
            {
                uiElements.previousGadget_IMG.color = Color.white;
            }
            if (uiElements.nextGadget_IMG.sprite)
            {
                uiElements.nextGadget_IMG.color = Color.white;
            }
        }
    }

    private void ChangeHuman(CharacterComponents ent)
    {
        if (ent.identification == Entity.Human)
        {
            isHuman = true;
        }
        else
        {
            isHuman = false;
        }
    }

    private void Update()
    {
        MarkNearestInteractable();
    }

    private ObjectDistance[] GetTargets()
    {
        Collider[] potentialObjects;

        if (isHuman)
        {
            potentialObjects = Physics.OverlapSphere(humanInvokingPosition.position, storedGadgets[currentlyEquippedGadgetIndex].radius, interactables);
        }
        else
        {
            potentialObjects = Physics.OverlapSphere(jaw.transform.position, jaw.radius, dogInteractables);
        }

        ObjectDistance[] distances = new ObjectDistance[potentialObjects.Length];

        //We feed our data to the struct array and sort it in the end with a custom IComparer<ObjectDistance> class
        for (int i = 0; i < distances.Length; i++)
        {
            if (isHuman)
            {
                distances[i].distance = Vector3.Distance(potentialObjects[i].transform.position, storedGadgets[currentlyEquippedGadgetIndex].transform.position);
            }
            else
            {
                distances[i].distance = Vector3.Distance(potentialObjects[i].transform.position, jaw.transform.position);
            }
            distances[i].obj = potentialObjects[i].gameObject;
        }

        System.Array.Sort(distances, new ObjectDistanceComparer());

        MarkedObjects = distances;
        return distances;
    }

    private void MarkNearestInteractable()
    {
        //First we get all the possible interactable objects around our gadget point
        ObjectDistance[] posTargets = GetTargets();

        //If there were no targets found we can be sure that a maybe existing previously marked object is not in range anymore => Demark it
        if (posTargets.Length == 0)
        {
            DemarkTarget(null);
        }
        //If there were targets found and the first (= nearest) target is different from our currently saved nearest object, we switch them
        else if (posTargets.Length > 0 && posTargets[0].obj != nearestObject)
        {
            DemarkTarget(null);
            nearestObject = posTargets[0].obj;

            MeshRenderer mRend = nearestObject.GetComponent<MeshRenderer>();
            Material mainMaterial = mRend.material;
            mainMaterial.shader = Shader.Find("Standard (Outlined)");
            mainMaterial.SetColor("_OutlineColor", outlineColor);
            mainMaterial.SetFloat("_Outline", 0.012f);
            mRend.material = mainMaterial;
        }
    }

    private void DemarkTarget(CharacterComponents disregard)
    {
        //Note: We do not need Entity => disregard
        if (nearestObject)
        {
            nearestObject.GetComponent<MeshRenderer>().material.shader = Shader.Find("Standard");
            nearestObject = null;
        }
    }

    //Gets called in the beginning of the game
    private void AddItemsToInventory()
    {
        if (GameInstance.GetInstance().debugMode)
        {
            foreach (Gadget i in allGadgets)
            {
                storedGadgets.Add(i);
            }
        }
        else
        {
            storedGadgets.Add(GetGadget("Hands"));
        }
    }

    public bool AddGadgetToInventory(Gadget g)
    {
        if (!storedGadgets.Contains(g))
        {
            storedGadgets.Add(g);
            return true;
        }
        return false;
    }

    public void ToggleInventoryVisibility(CharacterComponents entity)
    {
        switch (entity.identification)
        {
            case Entity.Human:
                GameInstance.GetInstance().UI_Components.inventory.alpha = 1;
                break;
            case Entity.Dog:
                GameInstance.GetInstance().UI_Components.inventory.alpha = 0;
                break;
        }
    }

    private void HandleInput(UserInput button, InputState state)
    {
        if (state == InputState.Ingame)
        {
            //Check for human control before executing any action
            if (SwitchControl.ControlledCharacter.identification == Entity.Human)
            {
                switch (button)
                {
                    case UserInput.R2:
                        //Switch the currently equipped item one to the right
                        if (!currentlyChanging)
                            StartCoroutine(ChangeGadget(Direction.Right));
                        break;
                    case UserInput.L2:
                        //Switch the currently equipped item one to the left
                        if (!currentlyChanging)
                            StartCoroutine(ChangeGadget(Direction.Left));
                        break;
                    case UserInput.B:
                        //Execute the current item's action
                        storedGadgets[currentlyEquippedGadgetIndex].Execute();
                        break;
                }
            }
            else if (SwitchControl.ControlledCharacter.identification == Entity.Dog)
            {
                //If the controlled character is the dog, the only action the player can take is using the dog's jaw
                switch (button)
                {
                    case UserInput.B:
                        //We play the animation of the dog picking up something only if there was a successful invocation
                        if (jaw.Execute())
                        {
                            Animator dogAnim = GameInstance.GetInstance().Player_Components.dog.GetComponent<Animator>();
                            if (dogAnim.GetCurrentAnimatorStateInfo(0).IsTag("GroundedState"))
                            {
                                dogAnim.SetTrigger("PickUp");
                            }
                        }
                        break;
                }
            }
        }
    }

    Gadget GetGadget(string name)
    {
        for (int i = 0; i < allGadgets.Length; i++)
        {
            if (allGadgets[i].gameObject.name == name)
            {
                return allGadgets[i];
            }
        }
        throw new System.ArgumentException(string.Format("The given item {0} was not found in the database.", name));
    }

    Gadget GetNextGadget(int index)
    {
        if (index + 1 <= storedGadgets.Count - 1)
        {
            return storedGadgets[index + 1];
        }

        //If the index has come to an end, return to the first gadget
        return storedGadgets[0];
    }

    Gadget GetPrevGadget(int index)
    {
        if (index - 1 >= 0)
        {
            return storedGadgets[index - 1];
        }

        //If the index has come to an end, return to the last gadget
        return storedGadgets[storedGadgets.Count - 1];
    }

    int DecreaseIndex()
    {
        if (currentlyEquippedGadgetIndex == 0)
        {
            return storedGadgets.Count - 1;
        }
        return currentlyEquippedGadgetIndex - 1;
    }

    int IncreaseIndex()
    {
        if (currentlyEquippedGadgetIndex == storedGadgets.Count - 1)
        {
            return 0;
        }
        return currentlyEquippedGadgetIndex + 1;
    }

    IEnumerator ChangeGadget(Direction dir)
    {
        float counter = 0;
        currentlyChanging = true;

        if (dir == Direction.Right)
        {
            RectTransform newTrans = Instantiate(uiElements.imagePrefab).GetComponent<RectTransform>();
            Image newTransImage = newTrans.GetComponent<Image>();
            newTrans.SetParent(uiElements.iconMask);
            newTrans.anchoredPosition = uiElements.spawnLeft.anchoredPosition;
            newTrans.sizeDelta = uiElements.spawnLeft.sizeDelta;
            currentlyEquippedGadgetIndex = DecreaseIndex();
            newTransImage.sprite = GetPrevGadget(currentlyEquippedGadgetIndex).icon;
            newTransImage.color = inactiveColor;

            while (counter < 1)
            {
                counter += Time.deltaTime * iconChangeSpeed;
                if (counter > 1)
                    counter = 1;

                newTrans.anchoredPosition = Vector2.Lerp(newTrans.anchoredPosition, prevStaticPosition, counter);
                //NEW CODE
                newTrans.sizeDelta = Vector2.Lerp(newTrans.sizeDelta, prevStaticSize, counter);

                prevTransform.anchoredPosition = Vector2.Lerp(prevTransform.anchoredPosition, curStaticPosition, counter);
                prevTransform.sizeDelta = Vector2.Lerp(prevTransform.sizeDelta, curStaticSize, counter);
                uiElements.previousGadget_IMG.color = Color.Lerp(uiElements.previousGadget_IMG.color, Color.white, counter);

                curTransform.anchoredPosition = Vector2.Lerp(curTransform.anchoredPosition, nextStaticPosition, counter);
                curTransform.sizeDelta = Vector2.Lerp(curTransform.sizeDelta, nextStaticSize, counter);
                uiElements.currentGadget_IMG.color = Color.Lerp(uiElements.currentGadget_IMG.color, inactiveColor, counter);

                nextTransform.anchoredPosition = Vector2.Lerp(nextTransform.anchoredPosition, uiElements.spawnRight.anchoredPosition, counter);

                yield return null;
            }
            Destroy(nextTransform.gameObject);
            //Rearrange pointer
            nextTransform = curTransform;
            curTransform = prevTransform;
            prevTransform = newTrans;

            uiElements.nextGadget_IMG = uiElements.currentGadget_IMG;
            uiElements.currentGadget_IMG = uiElements.previousGadget_IMG;
            uiElements.previousGadget_IMG = newTransImage;
            currentlyChanging = false;
        }
        else if (dir == Direction.Left)
        {
            RectTransform newTrans = Instantiate(uiElements.imagePrefab).GetComponent<RectTransform>();
            Image newTransImage = newTrans.GetComponent<Image>();
            newTrans.SetParent(uiElements.iconMask);
            newTrans.anchoredPosition = uiElements.spawnRight.anchoredPosition;
            newTrans.sizeDelta = uiElements.spawnRight.sizeDelta;
            currentlyEquippedGadgetIndex = IncreaseIndex();
            newTransImage.sprite = GetNextGadget(currentlyEquippedGadgetIndex).icon;
            newTransImage.color = inactiveColor;

            while (counter < 1)
            {
                counter += Time.deltaTime * iconChangeSpeed;
                if (counter > 1)
                    counter = 1;

                newTrans.anchoredPosition = Vector2.Lerp(newTrans.anchoredPosition, nextStaticPosition, counter);
                //NEW CODE
                newTrans.sizeDelta = Vector2.Lerp(newTrans.sizeDelta, nextStaticSize, counter);

                nextTransform.anchoredPosition = Vector2.Lerp(nextTransform.anchoredPosition, curStaticPosition, counter);
                nextTransform.sizeDelta = Vector2.Lerp(nextTransform.sizeDelta, curStaticSize, counter);
                uiElements.nextGadget_IMG.color = Color.Lerp(uiElements.nextGadget_IMG.color, Color.white, counter);

                curTransform.anchoredPosition = Vector2.Lerp(curTransform.anchoredPosition, prevStaticPosition, counter);
                curTransform.sizeDelta = Vector2.Lerp(curTransform.sizeDelta, prevStaticSize, counter);
                uiElements.currentGadget_IMG.color = Color.Lerp(uiElements.currentGadget_IMG.color, inactiveColor, counter);

                prevTransform.anchoredPosition = Vector2.Lerp(prevTransform.anchoredPosition, uiElements.spawnLeft.anchoredPosition, counter);
                yield return null;
            }
            Destroy(prevTransform.gameObject);
            //Rearrange pointer
            prevTransform = curTransform;
            curTransform = nextTransform;
            nextTransform = newTrans;

            uiElements.previousGadget_IMG = uiElements.currentGadget_IMG;
            uiElements.currentGadget_IMG = uiElements.nextGadget_IMG;
            uiElements.nextGadget_IMG = newTransImage;
            currentlyChanging = false;
        }
    }

    enum Direction
    {
        Right,
        Left
    }
}
