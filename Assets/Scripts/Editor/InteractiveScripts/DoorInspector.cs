using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System;

using Object = UnityEngine.Object;

[CustomEditor(typeof(Door))]
public class DoorInspector : Editor
{
    private Door door;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Separator();
        DrawVariables();
        EditorGUILayout.Separator();
        DrawButtons();
    }

    public void DrawButtons()
    {
        door = target as Door;

        if (GUILayout.Button("Add Closed Rotation"))
        {
            Debug.Log("closed added");
            Undo.RecordObject(door, "Add Closed Rotation");
            door.closedRot = door.gameObject.transform.rotation;
            EditorUtility.SetDirty(door);
        }

        if (GUILayout.Button("Add Open Rotation"))
        {
            Undo.RecordObject(door, "Add Open Rotation");
            door.openRot = door.gameObject.transform.rotation;
            EditorUtility.SetDirty(door);
        }

        if (GUILayout.Button("Open"))
        {
            Undo.RecordObject(door, "Open Door");
            door.gameObject.transform.rotation = door.openRot;
            door.open = true;
            EditorUtility.SetDirty(door);
        }

        if (GUILayout.Button("Close"))
        {
            Debug.Log("closed");
            Undo.RecordObject(door, "Close Door");
            door.gameObject.transform.rotation = door.closedRot;
            door.open = false;
            EditorUtility.SetDirty(door);
        }
    }

    public void DrawVariables()
    {
        door = target as Door;

        EditorGUI.BeginChangeCheck();
        float tmpSpeed = EditorGUILayout.FloatField("Door speed", door.speed);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(door, "Change speed");
            door.speed = tmpSpeed;
            EditorUtility.SetDirty(door);
        }
    }
}
