using UnityEngine;
using System.Collections;

public class QuitGameIngame : MenuButton 
{

    public override void Invoke()
    {
        Application.Quit();
    }

    public override void OnSelect()
    {
        visualRepresentation.color = Color.red;
    }

    public override void OnDeselect()
    {
        visualRepresentation.color = Color.white;
    }
}
