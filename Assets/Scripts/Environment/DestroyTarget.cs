using UnityEngine;
using System.Collections;

public class DestroyTarget : MonoBehaviour, IInteractable
{
    public GameObject objectToDestroy;
    public GadgetIdentifier keyGadget;

    public bool Execute(GadgetIdentifier ident)
    {
        if (ident == keyGadget)
        {
            Destroy(objectToDestroy);
            return true;
        }

        return false;
    }
}
