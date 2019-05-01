using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        public Controls controls;
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.

                // BEGIN MODIF
                // Original
                //m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
                // Option 1: Key Code
                //m_Jump = Input.GetKeyDown(controls.jump);
                // Option 2: Input Manager
                m_Jump = CrossPlatformInputManager.GetButtonDown(controls.jumpButton);
                // END MODIF            
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            // <<< BEGIN MODIF
            // Original
            //float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Option 1: Key Codes
            // float h = 0;

            // if (Input.GetKey(controls.left)) {
            //     h += -1;
            // }
            // if (Input.GetKey(controls.right)) {
            //     h += 1;
            // }

            // Option 2: Input Manager
            float h = CrossPlatformInputManager.GetAxis(controls.horizontalAxis);
            // END MODIF >>>

            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;
        }
    }
}

// Aditional informations about inputs: https://docs.unity3d.com/Manual/ConventionalGameInput.html