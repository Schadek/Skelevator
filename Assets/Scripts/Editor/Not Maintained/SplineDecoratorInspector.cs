using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SplineDecorator))]
public class SplineDecoratorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        SplineDecorator deco = target as SplineDecorator;
        deco.spline = deco.GetComponent<BezierSpline>();

        if (GUILayout.Button("Fill Spline"))
        {
            Undo.RecordObject(deco, "Revert Fill");
            deco.Decorate();
            EditorUtility.SetDirty(deco);
        }
    }
}
