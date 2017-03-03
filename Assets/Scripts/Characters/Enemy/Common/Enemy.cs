using System.Collections;
using System.Collections.Generic;
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

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        mRigidBody = GetComponent<Rigidbody2D>();
        mTransform = transform;
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
        mTransform.LookAt(player.transform.position);
        mTransform.Rotate(new Vector2(0, -90), Space.Self); //correcting the original rotation
        print(Vector2.Distance(player.transform.position, mTransform.position));
        if (Vector2.Distance(player.transform.position, mTransform.position) > minDistance)
        {
            //Move towards player
            mTransform.Translate(new Vector2(movementSpeed * Time.deltaTime, 0));
        }
    }

    public void PlayerEnteredRange(Collider2D collision)
    {
        print("Player detected!");
        if (chasePlayer)
        {
            playerInRange = true;
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
