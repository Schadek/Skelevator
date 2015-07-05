using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResumeButton : MenuButton 
{
    public override void Invoke()
    {
        StartCoroutine(ResumeGame());
    }

    public override void OnSelect()
    {
        visualRepresentation.color = Color.red;
    }

    public override void OnDeselect()
    {
        visualRepresentation.color = Color.white;
    }

    private IEnumerator ResumeGame()
    {
        Image fadeTexture = GameInstance.GetInstance().UI_Components.fadeTexture;
        Color newColor = new Color(fadeTexture.color.r, fadeTexture.color.g, fadeTexture.color.b, 0f);
        float counter = 0f;
        Time.timeScale = 1;

        while (counter < 1f)
        {
            if (counter > 0.1f)
            {
                //Band-aid solution. If we change the state immediately the character jumps
                InputManager.state = InputState.Ingame;
            }
            counter += Time.fixedDeltaTime;
            fadeTexture.color = Color.Lerp(fadeTexture.color, newColor, counter);
            menuGroup.alpha -= Time.deltaTime * 2;
            yield return null;
        }

        menuGroup.Deactivate();
    }
}
