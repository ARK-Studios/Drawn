using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utility;
using UnityEngine;

public class StateChanger : Enemy{
    public float period = 5f;      //in seconds
    public bool isHostile = false;
    private float nextState = 0.0f;
    public Sprite hostileSprite;
    public Sprite friendlySprite;
    private BoxCollider2D boxCollider2d;

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
            mSpriteRenderer.sprite = friendlySprite;
        }
        else
        {
            isHostile = true;
            mSpriteRenderer.sprite = hostileSprite;
        }
    }
}
