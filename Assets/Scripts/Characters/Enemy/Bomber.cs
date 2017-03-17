using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour {
    public float cooldown = 0.5f;           //In Seconds
    public GameObject bomb;
    public float coolDownTimer;
    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
        if(coolDownTimer > 0)
        {
            coolDownTimer -= Time.deltaTime;
        }
        if(coolDownTimer < 0)
        {
            coolDownTimer = 0;
        }
        if(coolDownTimer == 0)
        {
            DropBomp();
            coolDownTimer = cooldown;
        }
	}

    private void DropBomp()
    {
        GameObject go = (GameObject)Instantiate(bomb);
        go.transform.position = transform.position;
    }
}
