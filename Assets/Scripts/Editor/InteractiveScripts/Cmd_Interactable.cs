using UnityEngine;
using UnityEditor;
using System.Collections;

public class Cmd_Interactable : MonoBehaviour
{
    [MenuItem("Interactive/Create Cube Interactable", false)]
    private static void CreateBoxInteractable()
    {
        Transform parent = null;
        if (Selection.activeGameObject && PrefabUtility.GetPrefabParent(Selection.activeGameObject) == null)
        {
            parent = Selection.activeGameObject.transform;
        }

        Transform interactable = new GameObject("Interactive Object", typeof(InteractableEntries), typeof(BoxCollider), typeof(MeshFilter), typeof(MeshRenderer)).transform;
        interactable.parent = parent;
        interactable.gameObject.layer = LayerMask.NameToLayer("Interactables");

        Selection.activeGameObject = interactable.gameObject;
    }

    [MenuItem("Interactive/Create Sphere Interactable", false)]
    private static void CreateSphereInteractable()
    {
        Transform parent = null;
        if (Selection.activeGameObject && PrefabUtility.GetPrefabParent(Selection.activeGameObject) == null)
        {
            parent = Selection.activeGameObject.transform;
        }

        Transform interactable = new GameObject("Interactive Object", typeof(InteractableEntries), typeof(SphereCollider), typeof(MeshFilter), typeof(MeshRenderer)).transform;
        interactable.parent = parent;
        interactable.gameObject.layer = LayerMask.NameToLayer("Interactables");

        Selection.activeGameObject = interactable.gameObject;
    }

    [MenuItem("Interactive/Create CompCollider Interactable", false)]
    private static void CreateCompInteractable()
    {
        Transform parent = null;
        if (Selection.activeGameObject && PrefabUtility.GetPrefabParent(Selection.activeGameObject) == null)
        {
            parent = Selection.activeGameObject.transform;
        }

        Transform interactable = new GameObject("Interactive Object", typeof(InteractableEntries), typeof(MeshFilter), typeof(MeshRenderer)).transform;
        interactable.parent = parent;
        interactable.gameObject.layer = LayerMask.NameToLayer("Interactables");

        Selection.activeGameObject = interactable.gameObject;
    }

    [MenuItem("Interactive/Make Interactive", true)]
    private static bool MakeInteractable_validate()
    {
        if ((Selection.activeGameObject == null || PrefabUtility.GetPrefabObject(Selection.activeObject) != null) && PrefabUtility.GetPrefabType(Selection.activeGameObject) != PrefabType.PrefabInstance)
        {
            return false;
        }
        return true;
    }

    [MenuItem("Interactive/Make Interactive", false, 10)]
    private static void MakeInteractable()
    {
        GameObject selectedObject = Selection.activeGameObject;

        if (!selectedObject.GetComponent<InteractableEntries>())
        {
            selectedObject.AddComponent<InteractableEntries>();
        }
        selectedObject.layer = LayerMask.NameToLayer("Interactables");
    }
}
