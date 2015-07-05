using UnityEngine;
using System.Collections;

public class PrintMessage : MonoBehaviour, IInteractable
{
    public GadgetIdentifier keyGadget;
    [Space(15)]
    [Multiline]
    public string message;

    public bool Execute(GadgetIdentifier ident)
    {
        if (ident == keyGadget)
        {
            MessageSystem.GetInstance().StartDialogue(message);
            return true;
        }
        return false;
    }
}
