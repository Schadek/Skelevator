using UnityEngine;
using System.Collections;

//This class only exists to determine if the player touched the platform
public class DogLiftPlatform : MonoBehaviour 
{
    public bool humanOnTop;


    private void OnCollisionEnter(Collision collision)
    {
        //The player is currently touching the lifting pad
        if (collision.transform.CompareTag("Player"))
        {
            humanOnTop = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //The player has left the lifting pad
        if (collision.transform.CompareTag("Player"))
        {
            humanOnTop = false;
        }
    }
}
