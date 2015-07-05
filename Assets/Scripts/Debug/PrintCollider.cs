using UnityEngine;
using System.Collections;

public class PrintCollider : MonoBehaviour 
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name, other.gameObject);
    }
}
