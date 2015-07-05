using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{
    [Tooltip("The other pressure plate for another character, if there is one")]
    public PressurePlate sisterPlate;
    [Tooltip("Plate only activatable by a specific character")]
    public Entity characterOnly = Entity.None;
    [Tooltip("The door to be triggered")]
    public Transform door;

    public Entity StandingOnTop { get; set; }
    public bool Triggered { get; set; }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (StandingOnTop == Entity.None)
            {
                Debug.Log("Human standing");
                StandingOnTop = Entity.Human;
                IsTriggered();
            }
        }
        else if (other.gameObject.CompareTag("Dog"))
        {
            if (StandingOnTop == Entity.None)
            {
                Debug.Log("Dog standing");
                StandingOnTop = Entity.Dog;
                IsTriggered();
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Dog") && StandingOnTop == Entity.Dog)
        {
            StandingOnTop = Entity.None;
        }
        else if (other.gameObject.CompareTag("Player") && StandingOnTop == Entity.Human)
        {
            StandingOnTop = Entity.None;
        }
    }

    private void IsTriggered()
    {
        if (!Triggered)
        {
            if (StandingOnTop == Entity.Dog && sisterPlate.StandingOnTop == Entity.Human)
            {
                StartCoroutine(liftDoor());
                Triggered = true;
                sisterPlate.Triggered = true;
            }
            else if (StandingOnTop == Entity.Human && sisterPlate.StandingOnTop == Entity.Dog)
            {
                StartCoroutine(liftDoor());
                Triggered = true;
                sisterPlate.Triggered = true;
            }
        }
    }

    private IEnumerator liftDoor()
    {
        float counter = 0;
        Vector3 newPosition = door.position + new Vector3(0, 6f, 0);

        while (counter < 1)
        {
            counter += Time.deltaTime * 0.1f;
            door.position = Vector3.Lerp(door.position, newPosition, counter);
            yield return null;
        }
    }
}
