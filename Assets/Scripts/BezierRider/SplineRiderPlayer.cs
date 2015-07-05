using UnityEngine;
using System.Collections;

public class SplineRiderPlayer : MonoBehaviour
{
    [HideInInspector]
    public BezierSpline spline;
    public float speed = 0.001f;
    public float maximumSpeed = 0.2f;
    public bool lookForward;

    private float progress;
    private const float minSpeed = 0.1f;
    private int curvesInSpline;
    private const float gravity = 0.0007f;
    public bool TravellingInPositiveDirection { get; set; }
    public float ManipulateProgress { set { progress = Mathf.Clamp01(value); } }

    private void Update()
    {
        if (spline && TravellingInPositiveDirection)
        {
            VelocitySystem();
            progress += (speed / (float)curvesInSpline) * Time.deltaTime;
            if (progress > 1f)
            {
                progress = 1f;
            }
            Vector3 position = spline.GetPoint(progress);
            transform.position = position;
            if (lookForward)
            {
                transform.LookAt(position + spline.GetDirection(progress));
            }

            if (progress >= 1f)
            {
                Rigidbody rBody = gameObject.GetComponent<Rigidbody>();
                rBody.velocity = Vector3.zero;

                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                rBody.AddForce(spline.GetVelocity(1f) * speed * 14f);
                Debug.Log(rBody.velocity.sqrMagnitude);

                spline = null;
                speed = minSpeed;
                progress = 0f;
            }
        }
        else if (spline)
        {
            VelocitySystem();
            progress -= (speed / (float)curvesInSpline) * Time.deltaTime;
            if (progress < 0f)
            {
                progress = 0f;
            }
            Vector3 position = spline.GetPoint(progress);
            transform.position = position;
            if (lookForward)
            {
                transform.LookAt(position + spline.GetDirection(progress) * -1);
            }

            if (progress <= 0f)
            {
                Rigidbody rBody = gameObject.GetComponent<Rigidbody>();
                rBody.velocity = Vector3.zero;
                //StartCoroutine(StandUpRight());

                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                rBody.AddForce(spline.GetVelocity(0f) * speed * -14f);

                spline = null;
                speed = minSpeed;
                progress = 0f;
            }
        }
    }

    //This is a highly experimental way to simulate dynamic velocity in spline riding
    private void VelocitySystem()
    {
        bool GoingUp = false;
        if (spline.GetVelocity(progress).y > 0)
        {
            GoingUp = true;
            speed += spline.GetVelocity(progress).y * gravity;
        }
        else if (spline.GetVelocity(progress).y < 0)
        {
            GoingUp = false;
            speed -= spline.GetVelocity(progress).y * gravity;
        }

        Debug.Log("Speed: " + speed + ", " + "MaxSpeed: " + maximumSpeed + " Going Up: " + GoingUp);
        if (speed > maximumSpeed)
        {
            speed = maximumSpeed;
        }
        else if (speed < minSpeed)
        {
            speed = minSpeed;
        }
    }

    public void AssignSpline(BezierSpline targetSpline)
    {
        spline = targetSpline;
        curvesInSpline = targetSpline.ControlPointCount / 3;
    }

    /*private IEnumerator StandUpRight()
    {
        float counter = 0;
        Quaternion newRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        while (counter < 1)
        {
            counter += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, counter);
            yield return null;
        }
        Rigidbody rBody = gameObject.GetComponent<Rigidbody>();
        rBody.AddForce(transform.forward * 20);
        Debug.Log("FOrce applied");
    }*/
}
