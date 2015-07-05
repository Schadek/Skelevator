using UnityEngine;
using System.Collections;

public class ManipulateDogAttribute : MonoBehaviour, IInteractable
{
    public Skulls skullToWear;
    public GadgetIdentifier keyGadget;

    public bool Execute(GadgetIdentifier usedGadget)
    {
        if (usedGadget == keyGadget)
        {
            DogAttributes.Instance.heldSkull = skullToWear;
            return true;
        }
        return false;
    }
}
