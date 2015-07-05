using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Paws : Gadget
{
    public bool debugMode = false;
    Rigidbody playerBody;
    Vector3 jumpVector;

    private void Start()
    {
        playerBody = this.gameObject.GetComponentInParent<Rigidbody>();
        SwitchControl.Switched += ReassignRigidbody;
    }

    private void ReassignRigidbody(CharacterComponents entity)
    {
        if (entity.identification == Entity.Human)
            playerBody = this.gameObject.GetComponentInParent<Rigidbody>();
    }

    //What happens if the player pressed the appropriate button and the IsUsable() function returned a true value
    public override bool Execute()
    {
        if (playerBody.velocity.y < 0.001f && playerBody.velocity.y > -0.001f)
        {
            //playerBody.velocity = new Vector3(playerBody.velocity.x, 0, playerBody.velocity.z);
            playerBody.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
        }
        else if (debugMode)
        {
            playerBody.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
        }

        return true;
    }
}
