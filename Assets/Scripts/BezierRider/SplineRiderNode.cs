using UnityEngine;
using System.Collections;

public class SplineRiderNode : MonoBehaviour
{
    public bool EndNode;

    public BezierSpline Spline { get; set; }

    private void Start()
    {
        Spline = GetComponentInParent<BezierSpline>();
    }


    //Obsolete code used previously when gadget checking was done Object -> Gadget not Gadget -> Object
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!other.gameObject.GetComponent<SplineRiderPlayer>().spline)
            {
                SplineRiderPlayer splineRider = other.gameObject.GetComponent<SplineRiderPlayer>();
                splineRider.spline = spline;
                splineRider.TravellingInPositiveDirection = true;
                splineRider.ManipulateProgress = 0f;

                if (EndNode)
                {
                    splineRider.TravellingInPositiveDirection = false;
                    splineRider.ManipulateProgress = 1f;
                }
            }
        }
    }*/
}
