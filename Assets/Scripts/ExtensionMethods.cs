using UnityEngine;
using System;
using System.Collections;

public static class ExtensionMethods
{
    public static Color ParseToColor(this string str)
    {
        Color newColor = new Color();

        string redStr = "";
        string greenStr = "";
        string blueStr = "";
        string alphaStr = "";

        int index = str.IndexOf(',');
        redStr = str.Remove(index);
        str = str.Remove(0, index);

        index = str.IndexOf(',');
        greenStr = str.Remove(index);
        str = str.Remove(0, index);

        index = str.IndexOf(',');
        blueStr = str.Remove(index);
        str = str.Remove(0, index);

        alphaStr = str;

        newColor.r = Convert.ToSingle(redStr);
        newColor.g = Convert.ToSingle(greenStr);
        newColor.b = Convert.ToSingle(blueStr);
        newColor.a = 1;

        if (alphaStr.Length > 0)
        {
            newColor.a = Convert.ToSingle(alphaStr);
        }

        return newColor;
    }

    public static void Deactivate(this CanvasGroup group)
    {
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    public static void Activate(this CanvasGroup group)
    {
        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    public static Vector3 SmoothStep(this Vector3 from, Vector3 to, float counter)
    {
        Vector3 smoothedVector = Vector3.zero;

        smoothedVector.x = Mathf.SmoothStep(from.x, to.x, counter);
        smoothedVector.y = Mathf.SmoothStep(from.y, to.y, counter);
        smoothedVector.z = Mathf.SmoothStep(from.z, to.z, counter);

        return smoothedVector;
    }
}
