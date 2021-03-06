﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ward : MonoBehaviour {

    GameObject ghost;
    [SerializeField] float pushForce = 15f;
    [SerializeField] float powerDrainMultiplier = 60f;
    [SerializeField] float lifeTime;
    [SerializeField] LayerMask mask;

    float timer = 0;


	private AudioSource WardPlace;
	private AudioSource WardPush;
	private AudioSource WardConstant;
	// Use this for initialization
	void Start ()
    {
        ghost = GameObject.FindGameObjectWithTag("Ghost");
        lifeTime = 25f;
		AudioSource[] Sources = GetComponents<AudioSource> ();
		WardPlace = Sources [0];
		WardPush = Sources [1];
		WardConstant = Sources [2];
		WardConstant.loop = true;
		WardConstant.Play ();

	}

	void Update ()
    {
        if (checkGhostVisibility() && ghost.GetComponent<GhostController>().getIfPhasing() == false)
        {
			if (!WardPush.isPlaying) {
				WardPush.Play ();
			}
            Vector2 force = ghost.transform.position - this.transform.position;
            force.Normalize();
            force *= pushForce;
            ghost.GetComponent<Rigidbody2D>().AddForce(force);
            drainGhostPower();
        }

        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            Destroy(this.gameObject);
        }
	}

    bool checkGhostVisibility()
    {
        if(Physics2D.Raycast(this.transform.position, ghost.transform.position - this.transform.position))
        {
            RaycastHit2D temp = Physics2D.Raycast(this.transform.position, ghost.transform.position - this.transform.position, mask);
            if (temp && temp.transform.tag == "Ghost")
            {
                return true;
            }
        }
        return false;
    }

    void drainGhostPower()
    {
        ghost.GetComponent<GhostController>().losePower(Time.deltaTime * powerDrainMultiplier);
    }
}
