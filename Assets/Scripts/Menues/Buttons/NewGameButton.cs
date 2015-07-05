using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewGameButton : MenuButton
{
    [Tooltip("The fading texture used to darken the screen")]
    public Image fadeTexture;
    public MeshRenderer visualMesh;

    private void Start()
    {
        //In the beginning the set the fade texture to invisible
        Color newColor = fadeTexture.color;
        newColor.a = 0f;
        fadeTexture.color = newColor;
    }

    public override void Invoke()
    {
        StartCoroutine(StartNewGame());
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

    private IEnumerator StartNewGame()
    {
        float counter = 0f;
        Color newColor = fadeTexture.color;
        Color oldColor = newColor;
        newColor.a = 1f;

        //As soon as the player presses the button all input is disabled
        InputManager.state = InputState.Disabled;

        while (counter < 1f)
        {
            counter += Time.deltaTime * 0.5f;
            fadeTexture.color = Color.Lerp(oldColor, newColor, counter);
            yield return null;
        }

        //We load the new level
        Application.LoadLevel(1);

        //Lastly we load the new level and enable controls
        InputManager.NullifyButtonPressed();
        InputManager.state = InputState.Ingame;
        StopAllCoroutines();
    }
}
