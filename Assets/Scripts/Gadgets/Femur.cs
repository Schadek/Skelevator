using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//Obsolete gadget for we did not implement any 'rollercoaster rides'
public class Femur : Gadget {

    public float radiusOfCheck = 1f;
    public SplineRiderPlayer pRider;

    //What happens if the player pressed the appropriate button and the IsUsable() function returned a true value
    public override bool Execute()
    {
        return false;
        /* ObjectDistance[] targets = GetTargets();

       if (targets.Length > 0)
        {
            Debug.Log(targets[0].node.Spline);
            //Assigning the spline to the splineRider
            pRider.AssignSpline(targets[0].node.Spline);
            pRider.TravellingInPositiveDirection = true;
            pRider.ManipulateProgress = 0f;
            //pRider.speed = distances[0].node.beginSpeed;
            //pRider.speed = pRider.gameObject.transform.position.sqrMagnitude * 0.5f;

            if (targets[0].node.EndNode)
            {
                pRider.TravellingInPositiveDirection = false;
                pRider.ManipulateProgress = 1f;
            }
        }*/
    }
}
