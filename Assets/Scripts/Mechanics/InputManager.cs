using UnityEngine;
using System.Collections;

public enum UserInput
{
    A,
    B,
    X,
    Y,
    Back,
    Start,
    L1,
    R1,
    L2,
    R2,
    Up,
    Right,
    Down,
    Left
}

public enum InputState
{
    Disabled,
    Title,
    Ingame,
    PauseMenu,
    Settings,
    Command
}

public class InputManager : MonoBehaviour
{
    [Tooltip("Should the object we preserved on scene load?")]
    public bool Invincible;
    [Tooltip("Which state should the manager begin with? (Debug only)")]
    public InputState beginState;

    public static event PressedButton ButtonPress;
    public delegate void PressedButton(UserInput button, InputState state);

    public static InputState state = InputState.Title;
    private bool xReset;
    private bool yReset;
    private static InputManager instance;

    private void Start()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        if (Invincible)
        {
            //We do not need specific input managers for each scene therefore we can stay with one object at a time
            DontDestroyOnLoad(this.gameObject);
        }
        if (GameInstance.GetInstance() && GameInstance.GetInstance().debugMode)
        {
            state = beginState;
        }
    }

    private void Update()
    {
        float leftX = Input.GetAxis("LeftStickX");
        float leftY = Input.GetAxis("LeftStickY");

        if (leftX == 0)
            xReset = true;
        if (leftY == 0)
            yReset = true;

        if (ButtonPress != null)
        {
            if (Input.GetButtonDown("A"))
                ButtonPress(UserInput.A, state);
            if (Input.GetButtonDown("B"))
                ButtonPress(UserInput.B, state);
            if (Input.GetButtonDown("X"))
                ButtonPress(UserInput.X, state);
            if (Input.GetButtonDown("Y"))
                ButtonPress(UserInput.Y, state);
            if (Input.GetButtonDown("Back"))
                ButtonPress(UserInput.Back, state);
            if (Input.GetButtonDown("Start"))
                ButtonPress(UserInput.Start, state);
            if (Input.GetButtonDown("L1"))
                ButtonPress(UserInput.L1, state);
            if (Input.GetButtonDown("R1"))
                ButtonPress(UserInput.R1, state);
            if (Input.GetAxis("L2") > 0)
                ButtonPress(UserInput.L2, state);
            if (Input.GetAxis("R2") > 0)
                ButtonPress(UserInput.R2, state);
            if (leftX > 0 && xReset)
            {
                ButtonPress(UserInput.Right, state);
                xReset = false;
            }
            else if (leftX < 0 && xReset)
            {
                ButtonPress(UserInput.Left, state);
                xReset = false;
            }
            if (leftY > 0 && yReset)
            {
                ButtonPress(UserInput.Up, state);
                yReset = false;
            }
            else if (leftY < 0 && yReset)
            {
                ButtonPress(UserInput.Down, state);
                yReset = false;
            }
        }
    }

    public static void NullifyButtonPressed()
    {
        ButtonPress = null;
    }
}
