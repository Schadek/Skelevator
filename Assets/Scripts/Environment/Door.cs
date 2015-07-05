using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour, IInteractable
{
    public GadgetIdentifier keyGadget;
    public float speed = 0.5f;
    public bool open;
    public Quaternion openRot = Quaternion.identity;
    public Quaternion closedRot = Quaternion.identity;

    public bool Execute(GadgetIdentifier ident)
    {
        if (ident == keyGadget)
        {
            if (open)
            {
                StopAllCoroutines();
                StartCoroutine(closeDoor());
                return true;
            }
            else 
            {
                StopAllCoroutines();
                StartCoroutine(openDoor());
                return true;
            }
        }
        return false;
    }

    private IEnumerator openDoor()
    {
        float counter = 0;
        open = true;

        while (counter < 1)
        {
            counter += Time.deltaTime * speed;
            transform.rotation = Quaternion.Slerp(transform.rotation, openRot, counter);
            yield return null;
        }
    }

    private IEnumerator closeDoor()
    {
        float counter = 0;
        open = false;

        while (counter < 1)
        {
            counter += Time.deltaTime * speed;
            transform.rotation = Quaternion.Slerp(transform.rotation, closedRot, counter);
            yield return null;
        }
    }
}
