using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SplineDecorator : MonoBehaviour
{
    public BezierSpline spline;
    private int frequency = 1;
    private bool lookForward = true;

    private Transform item;
    private List<Transform> objects = new List<Transform>();
    private Transform parentObject;
    private Transform prototype;


    public void Decorate()
    {
        prototype = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        prototype.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        parentObject = new GameObject("Visualization").transform;
        parentObject.SetParent(this.gameObject.transform);

        item = prototype;
        prototype.SetParent(parentObject);
        DestroyImmediate(item.gameObject.GetComponent<Collider>());

        if (frequency <= 0)
        {
            return;
        }
        float stepSize = 1f / (frequency * 100);
        for (int p = 0, f = 0; f < frequency; f++)
        {
            for (int i = 0; i < 100; i++, p++)
            {
                Transform curItem = (Transform)Instantiate(item.gameObject).transform;
                curItem.SetParent(parentObject);
                objects.Add(curItem);
                Vector3 position = spline.GetPoint(p * stepSize);
                item.transform.localPosition = position;
                if (lookForward)
                {
                    item.transform.LookAt(position + spline.GetDirection(p * stepSize));
                }
                item.transform.parent = transform;
            }
        }
    }
}
