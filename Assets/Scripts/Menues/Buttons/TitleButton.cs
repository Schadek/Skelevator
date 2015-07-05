using UnityEngine;
using System.Collections;

public class TitleButton : MenuButton 
{
    public override void Invoke()
    {
        //Besides loading another scene we need to nullify all scene local subscriptions
        //These will be loaded again as soon as we enter the scene
        //If you come across this section, Martin, this would probably be a good use case for UnityEvents
        InputManager.NullifyButtonPressed();
        SwitchControl.NullifySwitched();

        InputManager.state = InputState.Title;
        Time.timeScale = 1f;
        Application.LoadLevel(0);
    }

    public override void OnSelect()
    {
        visualRepresentation.color = Color.yellow;
    }

    public override void OnDeselect()
    {
        visualRepresentation.color = Color.white;
    }
}
