using UnityEngine;
using System.Collections;

public class SpeechArea : MonoBehaviour 
{
    public Skulls skullName;
    public string text;

    private MessageSystem messenger;
    private bool used;

    private void Start()
    {
        messenger = MessageSystem.GetInstance();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dog" && other.GetComponent<DogAttributes>().heldSkull == skullName && !used)
        {
            messenger.StartDialogue(text);
            used = true;
        }
    }
}
