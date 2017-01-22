using System;
using UnityEngine;
using Assets.Scripts.Utility;
using ARK.Player;

namespace ARK.Player.Controller
{
    [RequireComponent(typeof (PlayerProfile))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private LayerMask m_whatIsGround;                   // A mask determining what is ground to the character
        [SerializeField] private float MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float JumpForce = 400f;                  // Amount of force added when the player jumps.
        [SerializeField] private float CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        private PlayerProfile m_Character;
        private bool m_Jump;
        private bool FacingRight = true;         // For determining which way the player is currently facing.

        private void Awake()
        {
            m_Character = GetComponent<PlayerProfile>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = Input.GetButtonDown("Jump");
            }
            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_Character.GroundCheck.position, PlayerProfile.k_GroundedRadius, m_whatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Character.Grounded = true;
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = Input.GetAxis("Horizontal");

            Move(h, crouch, m_Jump);
            
            m_Jump = false;
            m_Character.Grounded = false;
        }

        public void FallOff_Check()
        {
            if (m_Character.GroundCheck.position.y < PlayerProfile.k_LowBound && !m_Character.IsDead)
            {
                m_Character.IsDead = true;
                m_Character.lives--;
                if (m_Character.lives > 0)
                {
                    m_Character.Fell = true;
                    ARKLogger.LogMessage(eLogCategory.Control,
                    eLogLevel.Info,
                    "Character Fell off the map!");
                    CharacterRespawn();
                }

            }
            else
            {
                m_Character.Fell = false;
                m_Character.RespawnChar.respawn = false;
            }
        }

        public void CharacterRespawn()
        {
            if (m_Character.lives > 0)
            {
                m_Character.RespawnChar.respawn = true;
                m_Character.RespawnChar.RespawnChar(m_Character.Charbody2D);
                m_Character.IsDead = false;
            }
            else
            {
                m_Character.lives = 0;
            }
        }
        public void Move(float move, bool crouch, bool jump)
        {
            //only control the player if grounded or airControl is turned on
            if (m_Character.Grounded || m_Character.AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move * CrouchSpeed : move);

                // Move the character
                m_Character.Charbody2D.velocity = new Vector2(move * MaxSpeed, m_Character.Charbody2D.velocity.y);

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
            FallOff_Check();
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

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "SavePt")
            {
                m_Character.currSavePt = collider;
            }
        }
    }
}
