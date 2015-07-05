using UnityEngine;
using System.Collections;

public class BoulderBridge : MonoBehaviour, IInteractable
{
    public GadgetIdentifier keyGadget;
    [Space(15)]
    public Rigidbody[] boulderBridge;
    public GameObject playerCollider;

    private void Start()
    {
        playerCollider.SetActive(false);
    }


    public bool Execute(GadgetIdentifier ident)
    {
        Debug.Log("triggered");
        if (ident == keyGadget)
        {
            Debug.Log("is jaw");
            foreach (Rigidbody i in boulderBridge)
            {
                i.isKinematic = false;
                i.gameObject.layer = 8;
            }
            playerCollider.SetActive(true);
            return true;
        }
        return false;
    }
}
