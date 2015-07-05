using UnityEngine;
using System.Collections;

public class StationarySpeechObject : MonoBehaviour
{
    [Multiline(10)]
    public string[] speeches;
    private int timesSpokenTo;
    private MessageSystem messenger;

    void Start()
    {
        messenger = MessageSystem.GetInstance();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (timesSpokenTo < speeches.Length)
                messenger.StartDialogue(speeches[timesSpokenTo]);
            timesSpokenTo++;
        }
    }
}
