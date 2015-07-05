using UnityEngine;
using System.Collections;

public class SettingsButton : MenuButton
{
    public override void Invoke()
    {
        throw new System.NotImplementedException();
    }

    public override void OnSelect()
    {
        visualRepresentation.color = Color.blue;
    }

    public override void OnDeselect()
    {
        visualRepresentation.color = Color.white;
    }
}
