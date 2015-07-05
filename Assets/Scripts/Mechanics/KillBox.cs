using UnityEngine;
using System.Collections;

public class KillBox : MonoBehaviour 
{
    public Transform respawn;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = respawn.position;
    }
}
