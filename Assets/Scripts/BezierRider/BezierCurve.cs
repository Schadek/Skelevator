using UnityEngine;
using System.Collections;

public class BezierCurve : MonoBehaviour 
{
    public Vector3[] points;

    //Reset message. Returns all point close to the parent transform
    public void Reset()
    {
        points = new Vector3[] {
            new Vector3(3f, 0, 0),
            new Vector3(6f, 0, 0),
            new Vector3(9f, 0, 0),
            new Vector3(12f, 0, 0)
        };
    }

    //Get a point on the curve. t => lerp t
    public Vector3 GetPoint(float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
    }

    //The velocity of a specific point on the curve. (First derivative)
    public Vector3 GetVelocity(float t)
    {
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) - transform.position;
    }

    //This is an extension of each intersection. It shows the direction of a step
    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized * 5;
    }
}
