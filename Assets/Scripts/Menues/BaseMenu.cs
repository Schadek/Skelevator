using UnityEngine;
using System.Collections;

public class BaseMenu : MonoBehaviour 
{
    [Tooltip("The maximum alpha the black fade texture can reach")]
    public float maximumAlpha = 0.5f;

    [SerializeField]
    protected MenuButton selectedButton;

    protected virtual void Start()
    {
        InputManager.ButtonPress += HandleInput;
        selectedButton.OnSelect();
    }

    //For each individual menu there is another override HandleInput function
    protected virtual void HandleInput(UserInput button, InputState state)
    {
        //Although this is obsolete, we check for the title screen input state
        if (state == InputState.Title)
        {
            switch (button)
            {
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

    //We go up one in the menu button hierarchy
    protected void Up()
    {
        selectedButton.OnDeselect();
        selectedButton = selectedButton.prev;
        selectedButton.OnSelect();
    }

    //We go down one in the menu button hierarchy
    protected void Down()
    {
        selectedButton.OnDeselect();
        selectedButton = selectedButton.next;
        selectedButton.OnSelect();
    }
}
