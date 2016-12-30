using System;
using UnityEngine;
using Spine.Unity;
using Assets.Scripts.Utility;

namespace ARK.Player
{
    public class PlayerProfile : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
        [SerializeField] public int lives;
        public Collider2D currSavePt;

        #region Inspector
        // [SpineAnimation] attribute allows an Inspector dropdown of Spine animation names coming form SkeletonAnimation.
        [SpineAnimation]
        public string runAnimationName = "Move";

        [SpineAnimation]
        public string idleAnimationName = "Idle";

        [SpineAnimation]
        public string jumpAnimationName = "Jump";

        [SpineAnimation]
        public string fallAnimationName = "Fall";
        #endregion

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        public float k_GroundedRadius = .4f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private bool m_Falling;
        const float lowBound = -20;        // Lower bound to check if the player is dead
        private bool m_fell = false;        // For determining if player has fallen
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Rigidbody2D m_Rigidbody2D;
        private bool isDead;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.

        private Respawn respawnChar;
        // Spine.AnimationState and Spine.Skeleton are not Unity-serialized objects. You will not see them as fields in the inspector.
        public Spine.AnimationState spineAnimationState;
        public Spine.Skeleton skeleton;

        SkeletonAnimation skeletonAnimation;

        string currentXAnimation = "Idle";
        string currentYAnimation = "None";

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_Falling = false;

            respawnChar = GetComponent<Respawn>();
            respawnChar.charSpawnPos = m_Rigidbody2D.position;
            isDead = false;
            lives = 5;
        }

        private void Start()
        {
            // Make sure you get these AnimationState and Skeleton references in Start or Later. Getting and using them in Awake is not guaranteed by default execution order.
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.state;
            skeleton = skeletonAnimation.skeleton;
        }

        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
        }

        public void FallOff_Check()
        {
            if (m_GroundCheck.position.y < lowBound && !isDead)
            {
                isDead = true;
                lives--;
                if (lives > 0)
                {
                    m_fell = true;
                    ARKLogger.LogMessage(eLogCategory.Control,
                    eLogLevel.Info,
                    "Character Fell off the map!");
                    CharacterRespawn();
                }
               
            } else {
                m_fell = false;
                respawnChar.respawn = false;
            }
        }

        public void CharacterRespawn ()
        {
            if (lives > 0) { 
                respawnChar.respawn = true;
                respawnChar.RespawnChar(m_Rigidbody2D);
                isDead = false;
            }
            else{
                lives = 0;
            }
        }

        public void Move(float move, bool crouch, bool jump)
        {

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }

            // If the player should jump...
            if (m_Grounded && jump)
            {
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
            
            
            if (m_Rigidbody2D.velocity.x == 0 && currentXAnimation == runAnimationName)
            {
                skeletonAnimation.state.SetAnimation(0, idleAnimationName, true);
                currentXAnimation = idleAnimationName;
            }
            else if (m_Rigidbody2D.velocity.x != 0 && currentXAnimation == idleAnimationName)
            {
                skeletonAnimation.state.SetAnimation(0, runAnimationName, true);
                currentXAnimation = runAnimationName;
            }

            if (m_Grounded && m_Rigidbody2D.velocity.y > 0)
            {
                skeletonAnimation.state.SetAnimation(1, jumpAnimationName, false);
                currentYAnimation = jumpAnimationName;
            }
            else if (!m_Grounded && !m_Falling && m_Rigidbody2D.velocity.y < 0)
            {
                skeletonAnimation.state.SetAnimation(1, fallAnimationName, false);
                currentYAnimation = fallAnimationName;
                m_Falling = true;
            }
            else if (m_Grounded && m_Rigidbody2D.velocity.y == 0)
            {
                ARKLogger.LogMessage(eLogCategory.Control,
                    eLogLevel.Info,
                    "Clearing Vertical Animation");
                skeletonAnimation.state.SetEmptyAnimation(1, 1.0f);
            }

            if (m_Grounded)
            {
                m_Falling = false;
            }

            if (m_Grounded && jump)
            {
                m_Grounded = false;
            }

            FallOff_Check();
        }

        public Vector2 CharacterPosition()
        {
            return m_Rigidbody2D.position;
        }

        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            skeletonAnimation.skeleton.FlipX = !m_FacingRight;
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "SavePt")
            {
                currSavePt = collider;
            }
        }
    }
}
