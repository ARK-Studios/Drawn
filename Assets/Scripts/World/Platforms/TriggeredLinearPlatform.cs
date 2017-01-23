using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utility.Timers;


[System.Serializable]
public class TriggeredLinearPlatform : LinearMovementPlatform {

    public bool triggerOnTouch;

    public float triggerDelay;

    private bool triggeredOnTouch = false;
    private FrameTimer timer;


	// Use this for initialization
	new void Start () {
        base.Start();
        timer = new FrameTimer(triggerDelay);
	}

	
	// Update is called once per frame
	new void Update () {
        
        if (timer.UpdateAndCheck())
        {
            triggeredOnTouch = true;
        }

        if (triggeredOnTouch)
        {
            base.Update();
        }
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        print("OnCollisionEnter2D");
        if (coll.gameObject.tag == "Player")
        {
            if (triggeredOnTouch == false)
            {
                timer.Start();
            }
        }
    }
}
