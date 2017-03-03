using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utility;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private Animator anim;
    private GameObject player;
    private Rigidbody2D mRigidBody;
    public float minDistance = 2f;
    public bool chasePlayer = false;
    private bool playerInRange = false;
    public float movementSpeed = 0.5f;
    private Transform mTransform;
    protected bool FacingRight = false;         // For determining which way the player is currently facing.
    private SpriteRenderer mSpriteRenderer;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        mRigidBody = GetComponent<Rigidbody2D>();
        mTransform = transform;
        mSpriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (playerInRange)
        {
            MoveToPlayer();
        }
	}

    private void MoveToPlayer()
    {
        
        if (Vector2.Distance(player.transform.position, mTransform.position) > minDistance)
        {
            //Flip enemy to face the player
            Flip();
        
            //move enemy towards player
        }
    }

    private void Flip()
    {
        Vector2 h = (player.transform.position - mTransform.position).normalized;
        float dot = Vector2.Dot(h, mTransform.right);
        ARKLogger.LogMessage(eLogCategory.General, eLogLevel.Info, dot.ToString());
        //if dot is negative the player is behind the enemy so flip the sprite
        if (dot < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            ARKLogger.LogMessage(eLogCategory.General, eLogLevel.Info, "a");
        }
        else if (dot > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            ARKLogger.LogMessage(eLogCategory.General, eLogLevel.Info, "b");
        }
    }
    public void PlayerEnteredRange(Collider2D collision)
    {
        print("Player detected!");
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Subtract player live

        }
    }
}
