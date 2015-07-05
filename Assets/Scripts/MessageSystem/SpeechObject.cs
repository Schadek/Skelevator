using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeechObject : MonoBehaviour
{
    [Multiline]
    public string[] speeches;
    protected MessageSystem system;

    void Start()
    {
        system = MessageSystem.GetInstance();
    }
}
