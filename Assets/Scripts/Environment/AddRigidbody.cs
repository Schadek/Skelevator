using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AddRigidbody : MonoBehaviour, IInteractable
{
    public GadgetIdentifier keyGadget = GadgetIdentifier.Hammer;
    public GameObject target = null;

    public bool Execute(GadgetIdentifier usedGagdet)
    {
        if (usedGagdet == keyGadget)
        {
            target.AddComponent<Rigidbody>();
            return true;
        }
        return false;
    }
}
