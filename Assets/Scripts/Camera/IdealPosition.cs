using UnityEngine;
using System.Collections;

//To let the player rotate around the player character and alter the camera's height, he/she has direct control over the ideal position of the player character
public class IdealPosition : MonoBehaviour
{
    [Tooltip("The object this ideal position should follow")]
    public Transform parent;
    public Transform rotationPivot;

    private Vector3 offset;
    private Vector3 lastFrameParentPosition;

    private static float maximumAngle = 50f;
    private static float minimumAngle = 4f;

    public bool UsedIdealPosition { get; set; }
    /*********************************************************************************************************************************************************************************/

    private void Awake()
    {
        InputManager.ButtonPress += ResetIdealPosition;
    }

    private void Start()
    {
        //We initialize the offsets of this ideal position to be ideal in the beginning. The object will try to be in the original offset at all times
        offset = parent.position - transform.position;
        lastFrameParentPosition = parent.position;
    }

    private void ResetIdealPosition(UserInput button, InputState state)
    {
        if (button == UserInput.L1 && state == InputState.Ingame)
        {
            Vector3 flatVector = transform.position;
            flatVector.y = rotationPivot.position.y;

            float magnitude = Vector3.Distance(flatVector, rotationPivot.position);

            flatVector = -rotationPivot.forward.normalized;
            flatVector *= magnitude;
            flatVector.y = transform.position.y - rotationPivot.position.y;
            transform.position = rotationPivot.position + flatVector;
        }
    }

    private Vector3 GetResetPosition()
    {
        Vector3 flatVector = transform.position;
        flatVector.y = rotationPivot.position.y;

        float magnitude = Vector3.Distance(flatVector, rotationPivot.position);

        flatVector = -rotationPivot.forward.normalized;
        flatVector *= magnitude;
        flatVector.y = transform.position.y - rotationPivot.position.y;
        return rotationPivot.position + flatVector;
    }

    public void UpdateIdealPosition()
    {
        offset = parent.position - lastFrameParentPosition;
        transform.position += offset;

        float xAxis = -Input.GetAxis("RightStickX");
        float yAxis = -Input.GetAxis("RightStickY");

        offset = Vector3.zero;

        //Because we can rotate our camera 360° we do not need to check for any angles
        offset.y = xAxis;

        transform.RotateAround(parent.position, offset.normalized, Mathf.Abs(offset.y) * 3f);
        transform.LookAt(parent);


        //Source of a very annoying but where the camera can get below the minimum angle and because of Quaternion rotation
        //we cannot check for this violation of rotating easily 
        float xRotation = transform.rotation.eulerAngles.x;
        float yInput = yAxis;
        if ((xRotation < maximumAngle && xRotation > minimumAngle) || xRotation > maximumAngle && yInput < 0 || xRotation < minimumAngle && yInput > 0)
        {
            float difference = yInput * 0.1f;
            transform.Translate(new Vector3(0, difference, 0));
            if (transform.rotation.eulerAngles.x < minimumAngle)
            {
                transform.rotation = Quaternion.Euler(minimumAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
        }

        //Lastly we save the current parent position for the next frame
        lastFrameParentPosition = parent.position;
    }
}
