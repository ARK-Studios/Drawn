using System;
using UnityEngine;
using Assets.Scripts.Utility;

namespace ARK.Player
{
    public class PlayerProfile : ARK.Base.BaseProfile
    {
        [SerializeField] public int lives = 5;
        public Collider2D currSavePt;

        public Respawn RespawnChar { get; set; }      // Character respawn


        private void Awake()
        {
            // Setting up references.
            GroundCheck = transform.Find("GroundCheck");
            CeilingCheck = transform.Find("CeilingCheck");
            Anim = GetComponent<Animator>();
            Charbody2D = GetComponent<Rigidbody2D>();
            Charbody2D.gravityScale = 2;
            RespawnChar = GetComponent<Respawn>();
            RespawnChar.charSpawnPos = Charbody2D.position;
            IsDead = false;
            Fell = false;
            AirControl = true;
            lives = 5;

        }

        public Vector2 CharacterPosition()
        {
            return Charbody2D.position;
        }
    }
}