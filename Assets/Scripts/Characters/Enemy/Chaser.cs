using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : Enemy {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        isHostile = true;
    }

    // Update is called once per frame
    void Update () {
        if (chasePlayer && playerInRange)
        {
            MoveToPlayer();
        }
    }
}
