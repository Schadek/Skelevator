using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//An ugly way to signal each possible interaction object in range which item was invoked by the player
public enum GadgetIdentifier
{
    Hands,
    Hammer,
    Hook,
    Knife,
    Jaw
}

/* This is the parent class to all the gadgets in the game. It provides common methods for target searching and distance sorting. 
 * For further information on how the gadget system operates, refer to GadgetInventory.cs */
public abstract class Gadget : MonoBehaviour
{
    [Tooltip("The icon representation of the item")]
    public Sprite icon;
    [Tooltip("The range of the item")]
    public float radius;
    [Tooltip("The interactable layer")]
    public LayerMask lMask;

    //Function invoking the gadget's functionality
    public abstract bool Execute();

    //Fetch the potential objects stored in the inventory
    protected ObjectDistance[] GetTargets()
    {
        if (GadgetInventory.Instance.MarkedObjects != null)
        {
            return GadgetInventory.Instance.MarkedObjects;
        }
        return new ObjectDistance[0];
    }

    protected bool TryInvoking(IInteractable[] list, GadgetIdentifier identifier)
    {
        //If any gadget was invoked successfully, we return true in the end
        bool anyGadgetFired = false;

        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].Execute(identifier))
            {
                anyGadgetFired = true;
            }
        }

        if (anyGadgetFired)
        {
            return true;
        }

        //Usually this should return false because no action was fired at this point. 
        //To prevent the system from skipping to the next nearest object we simply always
        //tell it that the invocation was a success
        return true;
    }
}
