using UnityEngine;
using System.Collections;

/* The camera boom keeps the camera from clipping into as environment assets marked game objects and lets it follow a specific object */
public class CameraBoom : MonoBehaviour
{
    public LayerMask environmentMask;

    private Transform idealPosition;
    private IdealPosition idealPositionScript;

    [HideInInspector]
    public Transform Target;

    public Transform IdealPosition
    {
        get
        {
            return idealPosition;
        }
        set
        {
            idealPosition = value;
            if (value)
                idealPositionScript = value.GetComponent<IdealPosition>();
        }
    }

    private void FixedUpdate()
    {
        if (!idealPosition)
            return;

        RaycastHit hit;
        Ray ray = new Ray(Target.position, IdealPosition.position - Target.position);

        //Command the ideal position object to update its position (The user controls the ideal position by using the right gamepad stick)
        if (InputManager.state == InputState.Ingame)
        {
            idealPositionScript.UpdateIdealPosition();
        }
        //Cast a sphere in the direction of the ideal position with the length of the distance between the ideal position and the target
        if (Physics.Raycast(ray, out hit, Vector3.Distance(IdealPosition.position, Target.position), environmentMask.value))
        {
            //After we checked that there is no obstacle in the middle of the screen we need to check for clipping
            if (Physics.CheckSphere(hit.point, 0.3f))
            {
                //If there would be an object in range of the check sphere, we move the new desired lerp point a bit more to the camera target
                transform.position = Vector3.Lerp(transform.position, hit.point + (Target.position - transform.position).normalized * 0.4f, 0.1f);
            }
            else
            {
                //If no collider was found within 0.3f units we only lerp to the hit point (Theoretically this should never happen)
                Debug.Log("If you read this, something went wrong", gameObject);
                transform.position = Vector3.Lerp(transform.position, hit.point, 0.1f);
            }
        }
        else
        {
            //Set the base position of the camera object back to the ideal position
            transform.position = Vector3.Lerp(transform.position, IdealPosition.position, 0.1f);
        }

        //Lastly we look at the focus object again
        transform.LookAt(Target);
    }
}
