using System;
using UnityEngine;
using Assets.Scripts.Utility;

namespace ARK.Base
{
    public class BaseProfile : MonoBehaviour
    {
        [SerializeField] public bool AirControl { get; set; }                          // Whether or not a player can steer while jumping;

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
    }
}
