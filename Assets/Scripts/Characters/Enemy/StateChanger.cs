using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utility;
using UnityEngine;

public class StateChanger : Enemy{
    private float nextState = 0.0f;
    private BoxCollider2D boxCollider2d;

    public float period = 5f;      //in seconds
    public Sprite hostileSprite;
    public Sprite friendlySprite;
    // Use this for initialization
    public override void Start () {
        base.Start();
        boxCollider2d = GetComponent<BoxCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {
		if(Time.time > nextState)
        {
            nextState += period;
            switchStates();
        }
	}

    private void switchStates()
    {
        ARKLogger.LogMessage(eLogCategory.Combat, eLogLevel.Info, isHostile.ToString());
        if (isHostile)
        {
            isHostile = false;
            spriteRenderer.sprite = friendlySprite;
        }
        else
        {
            isHostile = true;
            spriteRenderer.sprite = hostileSprite;
        }
    }
}
