using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : BaseMenu
{
    [Tooltip("The group holding the menu UI objects")]
    public CanvasGroup menuGroup;

    protected override void Start()
    {
        base.Start();
        menuGroup.Deactivate();
    }

    protected override void HandleInput(UserInput button, InputState state)
    {
        //First we need to define if we are inside the ingame screen
        if (state == InputState.Ingame)
        {
            //If the pressed button equals Start we open the menu screen and disable ingame input
            if (button == UserInput.Start)
            {
                StopAllCoroutines();
                StartCoroutine(OpenPauseMenu());
            }
        }
        //If we are not then maybe we are inside the menus screen?
        else if (state == InputState.PauseMenu)
        {
            switch (button)
            {
                case UserInput.Start:
                    StopAllCoroutines();
                    StartCoroutine(ClosePauseMenu());
                    break;
                case UserInput.Up:
                    Up();
                    break;
                case UserInput.Down:
                    Down();
                    break;
                case UserInput.A:
                    selectedButton.Invoke();
                    break;
            }
        }
    }

    private IEnumerator OpenPauseMenu()
    {
        Image fadeTexture = GameInstance.GetInstance().UI_Components.fadeTexture;
        Color newColor = new Color(fadeTexture.color.r, fadeTexture.color.g, fadeTexture.color.b, maximumAlpha);
        float counter = 0f;

        Time.timeScale = 0;

        //We need to set all character controller inputs to zero
        SwitchControl.ControlledCharacter.characterMotor.NullifyMovement();

        //We need to set the new state at the beginning of the coroutine
        InputManager.state = InputState.PauseMenu;

        //Standard black fade in
        while (counter < 1f)
        {
            counter += Time.fixedDeltaTime;
            fadeTexture.color = Color.Lerp(fadeTexture.color, newColor, counter);
            menuGroup.alpha = Mathf.Lerp(menuGroup.alpha, 1, counter);
            yield return null;
        }

        menuGroup.Activate();
    }

    private IEnumerator ClosePauseMenu()
    {
        Image fadeTexture = GameInstance.GetInstance().UI_Components.fadeTexture;
        Color newColor = new Color(fadeTexture.color.r, fadeTexture.color.g, fadeTexture.color.b, 0f);
        float counter = 0f;

        InputManager.state = InputState.Ingame;
        Time.timeScale = 1;

        while (counter < 1f)
        {
            counter += Time.deltaTime;
            fadeTexture.color = Color.Lerp(fadeTexture.color, newColor, counter);
            menuGroup.alpha -= Time.deltaTime * 2;
            yield return null;
        }

        menuGroup.Deactivate();
    }
}
