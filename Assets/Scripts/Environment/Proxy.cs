using UnityEngine;
using System.Collections;

public class Proxy : MonoBehaviour, IInteractable
{
    public MonoBehaviour redirectTo;
    private IInteractable castedObject;

    private void Start()
    {
        castedObject = (IInteractable)redirectTo;
    }

    public bool Execute(GadgetIdentifier usedGadget)
    {
        if (castedObject.Execute(usedGadget))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}

