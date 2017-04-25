using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utility;
using UnityEngine;

public class Enemy : MonoBehaviour {
    //Private members
    private Animator anim;
    private GameObject player;
    private Rigidbody2D mRigidBody;
    protected bool FacingRight = false;         // For determining which way the player is currently facing.
    private SpriteRenderer mSpriteRenderer;
    public bool isHostile = false;

    //Public members
    public bool chasePlayer = false;
    public float minDistance = 2f;
    public bool playerInRange = false;
    public float movementSpeed = 0.5f;

    // Use this for initialization
    public virtual void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        mRigidBody = GetComponent<Rigidbody2D>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    //Class Properities
    public SpriteRenderer spriteRenderer
    {
        get { return mSpriteRenderer; }
    } 
    public Rigidbody2D rigidBody2d
    {
        get { return mRigidBody; }
    }
    public Transform PlayerTransform
    {
        get { return player.transform; }
    }

    //Class Methods
    public int detectPlayer()
    {
        Vector2 h = (transform.position - player.transform.position).normalized;
        float dot = Vector2.Dot(h, transform.right);
        //if dot is negative the player is behind the enemy so flip the sprite
        if (dot < 0)
        {
            mSpriteRenderer.flipX = false;
            return 1;
        }
        else
        {
            mSpriteRenderer.flipX = true;
            return -1;
        }
    }

    public void MoveToPlayer()
    {
        if (Vector2.Distance(PlayerTransform.position, transform.position) > minDistance)
        {
            //Flip enemy to face the player
            detectPlayer();
            //move enemy towards player
            Vector2 velocity = new Vector2((transform.position.x - player.transform.position.x) * movementSpeed, (transform.position.y - player.transform.position.y) * movementSpeed);
            mRigidBody.velocity = -velocity;
        }
    }

    //Enemy Collision Handlers
    public void PlayerEnteredRange(Collider2D collision)
    {
        ARKLogger.LogMessage(eLogCategory.Combat, eLogLevel.Info, "Player is in detection range");
        playerInRange = true;
    }

    public void PlayerInRange(Collider2D collision)
    {
        //ARKLogger.LogMessage(eLogCategory.Combat, eLogLevel.Info, "Player is still in detection range");
        playerInRange = true;
    }
    public void PlayerExitedRange(Collider2D collision)
    {
        ARKLogger.LogMessage(eLogCategory.Combat, eLogLevel.Info, "Player exited detection range");
        playerInRange = false;
    }
}
