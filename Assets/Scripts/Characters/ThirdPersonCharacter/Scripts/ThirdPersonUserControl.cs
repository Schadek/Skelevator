using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


[RequireComponent(typeof(ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_CamObject;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

    public Vector3 MoveVector { get; set; }

    private void Start()
    {
        if (Camera.main != null)
        {
            m_CamObject = Camera.main.transform;
        }

        m_Character = GetComponent<ThirdPersonCharacter>();
        InputManager.ButtonPress += HandleInput;
    }


    private void HandleInput(UserInput button, InputState state)
    {
        if (button == UserInput.A && state == InputState.Ingame)
        {
            m_Jump = true;
        }
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (InputManager.state != InputState.Ingame)
            return;

        // read inputs
        float h = Input.GetAxis("LeftStickX");
        float v = Input.GetAxis("LeftStickY");

        if (m_CamObject != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_CamObject.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_CamObject.right;
        }

        // pass all parameters to the character control script
        m_Character.Move(m_Move, m_Jump);
        m_Jump = false;
    }
}
