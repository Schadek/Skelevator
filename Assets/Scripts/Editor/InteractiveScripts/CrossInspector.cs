using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Cross))]
public class CrossInspector : Editor 
{
    public override void OnInspectorGUI()
    {
        Cross cross = target as Cross;

        cross.keyGadget = (GadgetIdentifier)EditorGUILayout.EnumPopup(cross.keyGadget);

        if (GUILayout.Button("Add Upright Rotation"))
        {
            cross.upright = cross.transform.rotation;
        }
        if (GUILayout.Button("Add Turned Rotation"))
        {
            cross.turned = cross.transform.rotation;
        }
        if (GUILayout.Button("Flip"))
        {
            cross.isTurned = !cross.isTurned;
            if (cross.isTurned)
                cross.transform.rotation = cross.turned;
            else
                cross.transform.rotation = cross.upright;
        }

        EditorGUI.BeginChangeCheck();
        cross.uprightColor = EditorGUILayout.ColorField(cross.uprightColor);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(cross, "Change Upright Color");
            EditorUtility.SetDirty(cross);
        }

        EditorGUI.BeginChangeCheck();
        cross.turnedColor = EditorGUILayout.ColorField(cross.turnedColor);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(cross, "Change Turned Color");
            EditorUtility.SetDirty(cross);
        }

        //Getting lazy and busy... By the way I know of my wrong usage of the Undo class D:
        EditorGUILayout.LabelField("Upright Color");
        cross.uprightParticle = EditorGUILayout.ColorField(cross.uprightParticle);
        EditorGUILayout.LabelField("Turned Color");
        cross.turnedParticle = EditorGUILayout.ColorField(cross.turnedParticle);

        EditorGUI.BeginChangeCheck();
        Light tempLight = EditorGUILayout.ObjectField("Light", cross.crossLight, typeof(Light), true) as Light;
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(cross, "Set Light");
            cross.crossLight = tempLight;
            EditorUtility.SetDirty(cross);
        }

        EditorGUI.BeginChangeCheck();
        ParticleSystem tempSystem = EditorGUILayout.ObjectField("Particle System", cross.pSystem, typeof(ParticleSystem), true) as ParticleSystem;
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(cross, "Set Particle System");
            cross.pSystem = tempSystem;
            EditorUtility.SetDirty(cross);
        }
    }
}
