using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

[CustomEditor(typeof(AddRigidbody))]
public class AddRigidbodyInspector :Editor
{
    AddRigidbody addR;

    public override void OnInspectorGUI()
    {
        addR = target as AddRigidbody;

        EditorGUILayout.Separator();

        EditorGUI.BeginChangeCheck();
        GadgetIdentifier tmpIdent = (GadgetIdentifier)EditorGUILayout.EnumPopup("Key Gadget", addR.keyGadget);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(addR, "Change Key Gadget");
            addR.keyGadget = tmpIdent;
            EditorUtility.SetDirty(addR);
        }

        EditorGUILayout.Separator();

        EditorGUI.BeginChangeCheck();
        GameObject addTarget = (GameObject)EditorGUILayout.ObjectField("Target", addR.target, typeof(GameObject), true);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(addR, "Change Target");
            addR.target = addTarget;
            EditorUtility.SetDirty(addR);
        }
    }
}
