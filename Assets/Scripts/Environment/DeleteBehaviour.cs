using UnityEngine;
using System.Collections;

public class DeleteBehaviour : MonoBehaviour, IInteractable
{
    public GadgetIdentifier keyGadget;
    public MonoBehaviour[] toDelete;

    public bool Execute(GadgetIdentifier usedGadget)
    {
        if (usedGadget == keyGadget)
        {
            for (int i = 0; i < toDelete.Length; i++)
            {
                Destroy(toDelete[i]);
            }
            return true;
        }
        return false;
    }
}
