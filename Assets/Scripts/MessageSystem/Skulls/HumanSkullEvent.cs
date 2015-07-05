using UnityEngine;
using System.Collections;

public class HumanSkullEvent : MonoBehaviour {

    public string text = "Warte, warte. Das ist nicht mein Körper! O Nein, diese Schmach!";

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dog")
        {
            DogAttributes attr = other.GetComponent<DogAttributes>();

            if (attr.heldSkull == Skulls.Jacques)
            {
                MessageSystem.GetInstance().StartDialogue(text);

                SwitchControl.HumanHasSkull = true;
                attr.heldSkull = Skulls.None;
            }
        }
    }
}
