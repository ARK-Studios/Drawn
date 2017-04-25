using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : Enemy {

    public GameObject projectile;
    public float fireRate = 0.5f;
    private float nextFire = 0.0F;
    public float throwingForce = 5f;

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

        //determine if the player is infront or behind the enemy
        int rotationFactor = detectPlayer();
        //Throw object at player direction
        go.GetComponent<Rigidbody2D>().velocity = new Vector2(rotationFactor * transform.localScale.x, 1) * throwingForce;
    }

   
}
