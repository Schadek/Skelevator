using UnityEngine;
using System.Collections;

public class SkeletonBody : MonoBehaviour 
{
    public Skulls skull;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            MainCharacterAttributes attr = other.gameObject.GetComponent<MainCharacterAttributes>();
            if (attr.heldSkull == skull)
                MessageSystem.GetInstance().StartDialogue("Danke :)");
        }
    }
}
