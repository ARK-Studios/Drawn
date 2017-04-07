using System;
using UnityEngine;
using Assets.Scripts.Utility;
using ARK.Player;

namespace ARK.Player.Controller
{
    [RequireComponent(typeof (PlayerProfile))]
    public class PlayerController : ARK.Base.Movement.BaseMovement
    {
        private PlayerProfile m_Character;       // Character profile class which has character information

        private void Awake()
        {
            m_Character = GetComponent<PlayerProfile>();
            m_whatIsGround = LayerMask.NameToLayer("Everything");
        }


        private void Update()
        {
            if (!this.m_Jump)
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

            m_Character = (PlayerProfile)Move(h, crouch, m_Jump, m_Character);
            FallOff_Check();

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

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "SavePt")
            {
                m_Character.currSavePt = collider;
            }
            if (collider.tag == "LifeUp")
            {
                Destroy(collider.gameObject);
                m_Character.lives++;
            }
            if(collider.tag == "LifeDown")
            {
                Destroy(collider.gameObject);
                m_Character.lives--;
                CharacterRespawn();
            }

        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            print("test");
            if (collision.gameObject.tag == "Enemy")
            {
                ARKLogger.LogMessage(eLogCategory.Control,
                   eLogLevel.Info,
                   "Player collided with enemy");
                if (GetComponent<Collider>().GetComponent<StateChanger>().isHostile)
                {
                    m_Character.lives--;
                    CharacterRespawn();
                }
            }
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            print("test");
            if (collision.gameObject.tag == "Enemy")
            {
                ARKLogger.LogMessage(eLogCategory.Control,
                   eLogLevel.Info,
                   "player is colliding with enemy");
                if (collision.gameObject.GetComponent<StateChanger>().isHostile)
                {
                    m_Character.lives--;
                    CharacterRespawn();
                }
            }
        }
    }
}
