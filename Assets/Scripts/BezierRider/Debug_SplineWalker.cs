﻿using UnityEngine;
using System.Collections;

public class Debug_SplineWalker : MonoBehaviour 
{
    public BezierSpline spline;
    public float duration;
    public bool lookForward;

    private float progress;

    private void Update()
    {
        progress += Time.deltaTime / duration;
        if (progress > 1f)
        {
            progress = 1f;
        }
        Vector3 position = spline.GetPoint(progress);
        transform.localPosition = position;
        if (lookForward)
        {
            transform.LookAt(position + spline.GetDirection(progress));
        }
    }
}
