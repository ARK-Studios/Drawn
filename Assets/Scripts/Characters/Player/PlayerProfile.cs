using System;
using UnityEngine;
using Assets.Scripts.Utility;

namespace ARK.Player
{
    public class PlayerProfile : MonoBehaviour
    {
        [SerializeField] public bool AirControl { get; set; }                          // Whether or not a player can steer while jumping;
        [SerializeField] public int lives = 5;
        public Collider2D currSavePt;

        public const float k_GroundedRadius = .2f;           // Radius of the overlap circle to determine if grounded
        public const float k_LowBound = -20;                 // Lower bound to check if the player is dead
        public const float k_CeilingRadius = .01f;           // Radius of the overlap circle to determine if the player can stand up

        public Transform GroundCheck { get; set; }    // A position marking where to check if the player is grounded.
        public bool Grounded { get; set; }            // Whether or not the player is grounded.
        public bool Fell { get; set; }       // For determining if player has fallen
        public Transform CeilingCheck { get; set; }   // A position marking where to check for ceilings
        public Animator Anim { get; set; }            // Reference to the player's animator component.
        public Rigidbody2D Charbody2D { get; set; }   // Character body
        public bool IsDead { get; set; }              // If character is dead or not
        public Respawn RespawnChar { get; set; }      // Character respawn


        private void Awake()
        {
            // Setting up references.
            GroundCheck = transform.Find("GroundCheck");
            CeilingCheck = transform.Find("CeilingCheck");
            Anim = GetComponent<Animator>();
            Charbody2D = GetComponent<Rigidbody2D>();
            RespawnChar = GetComponent<Respawn>();
            RespawnChar.charSpawnPos = Charbody2D.position;
            IsDead = false;
            Fell = false;
            AirControl = false;
            lives = 5;

        }

        public Vector2 CharacterPosition()
        {
            return Charbody2D.position;
        }
    }
}
