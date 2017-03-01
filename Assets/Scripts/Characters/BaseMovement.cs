using System;
using UnityEngine;
using Assets.Scripts.Utility;
using ARK.Base;

namespace ARK.Base.Movement
{
    [RequireComponent(typeof(BaseProfile))]
    public class BaseMovement : MonoBehaviour
    {
        [SerializeField] protected LayerMask m_whatIsGround;                // A mask determining what is ground to the character
        [SerializeField] protected float MaxSpeed = 8f;                    // The fastest the player can travel in the x axis.
        [SerializeField] protected float JumpForce = 400f;                  // Amount of force added when the player jumps.
        [SerializeField] protected float CrouchSpeed = .36f;                // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] protected float AirAccel = 2f;
        protected bool m_Jump;                     // Detect if jump button is pressed
        protected bool FacingRight = true;         // For determining which way the player is currently facing.

        public BaseProfile Move(float move, bool crouch, bool jump, BaseProfile m_Character)
        {
            float resistance = 0;
            //only control the player if grounded or airControl is turned on
            if (m_Character.Grounded || m_Character.AirControl)
            {
                if (m_Character.AirControl && !m_Character.Grounded)
                {
                    move = move * AirAccel;
                } else
                {
                    // Reduce the speed if crouching by the crouchSpeed multiplier
                    move = (crouch ? move * CrouchSpeed : move);
                    // Move the character
                }
                resistance = m_Character.Charbody2D.velocity.x * (float)0.07;
                move = m_Character.Charbody2D.velocity.x + move - resistance;
                if (Math.Abs(m_Character.Charbody2D.velocity.x) > MaxSpeed)
                {
                    m_Character.Charbody2D.velocity = new Vector2(Math.Sign(move)*MaxSpeed, m_Character.Charbody2D.velocity.y);
                } else {
                    m_Character.Charbody2D.velocity = new Vector2(move, m_Character.Charbody2D.velocity.y);
                }

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Character.Grounded && jump)
            {
                // Add a vertical force to the player.
                m_Character.Grounded = false;
                m_Character.Charbody2D.AddForce(new Vector2(0f, JumpForce));
            }
            return m_Character;
        }

        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            FacingRight = !FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
