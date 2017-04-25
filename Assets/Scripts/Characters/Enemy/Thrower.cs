using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : Enemy {

    public GameObject projectile;
    public float fireRate = 0.5f;
    private float nextFire = 0.0F;
    public float force = 5f;

    // Use this for initialization
    public override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        if (playerInRange && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            ThrowProjectile();
        }
	}

    void ThrowProjectile()
    {
        GameObject go = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
        //FacePlayer();

        //determine if the player is infront or behind the enemy
        int rotationFactor = detectPlayer();
        go.GetComponent<Rigidbody2D>().velocity = new Vector2(rotationFactor * transform.localScale.x, 1) * force;
    }

    int detectPlayer()
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
}
