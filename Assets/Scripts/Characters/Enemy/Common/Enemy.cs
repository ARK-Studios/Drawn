using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utility;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private Animator anim;
    public GameObject player;
    private Rigidbody2D mRigidBody;
    public float minDistance = 2f;
    public bool chasePlayer = false;
    public bool playerInRange = false;
    public float movementSpeed = 0.5f;
    protected bool FacingRight = false;         // For determining which way the player is currently facing.
    public SpriteRenderer mSpriteRenderer;

    // Use this for initialization
    public virtual void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        mRigidBody = GetComponent<Rigidbody2D>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public Transform PlayerTransform
    {
        get { return player.transform; }
    }

    private void MoveToPlayer()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > minDistance)
        {
            //Flip enemy to face the player
            FacePlayer();
        
            //move enemy towards player
        }
    }

    public void FacePlayer()
    {
        Vector2 h = (transform.position - player.transform.position ).normalized;
        float dot = Vector2.Dot(h, transform.right);
        //if dot is negative the player is behind the enemy so flip the sprite
        if (dot < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            print(dot);
        }
        else if (dot > 0)
        {
            print(dot);
        }
    }

    public void PlayerEnteredRange(Collider2D collision)
    {
        playerInRange = true;
        if (chasePlayer)
        {
            
        }
    }

    public void PlayerExitedRange(Collider2D collision)
    {
        print("Player Exited  range!!");
        playerInRange = false;
    }
}
