using UnityEngine;
using System.Collections;

public class QuitGameButton : MenuButton
{
    public MeshRenderer visualMesh;

    public override void Invoke()
    {
        Application.Quit();
    }

    public override void OnSelect()
    {
        Material tmpMat = visualMesh.material;
        tmpMat.color = Color.red;
        visualMesh.material = tmpMat;
    }

    public override void OnDeselect()
    {
        Material tmpMat = visualMesh.material;
        tmpMat.color = new Color(0.7f, 0.7f, 0.7f);
        visualMesh.material = tmpMat;
    }
}
