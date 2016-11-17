using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using ARK.Player;

namespace ARK.Player.Controller
{
    [RequireComponent(typeof (PlayerProfile))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerProfile m_Character;
        private bool m_Jump;


        private void Awake()
        {
            m_Character = GetComponent<PlayerProfile>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
