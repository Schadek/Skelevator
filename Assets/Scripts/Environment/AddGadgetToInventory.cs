using UnityEngine;
using System.Collections;

public class AddGadgetToInventory : MonoBehaviour, IInteractable
{
    private GadgetInventory inventory;
    public GadgetIdentifier keyGadget;
    public Gadget gadgetToAdd;

    private void Start()
    {
        inventory = GadgetInventory.Instance;
    }

    public bool Execute(GadgetIdentifier usedGadget)
    {
        if (usedGadget == keyGadget)
        {
            if (inventory.AddGadgetToInventory(gadgetToAdd))
            {
                return true;
            }
            else
            {
                Debug.Log("The requested gadget is already added to the inventory");
                return false;
            }
        }
        return false;
    }
}
