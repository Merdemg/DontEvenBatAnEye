﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ward : MonoBehaviour {
    GameObject ghost;
    [SerializeField] float pushForce = 3f;
    [SerializeField] float powerDrainMultiplier = 6f;

    [SerializeField] float lifeTime = 60f;

    float timer = 0;


	// Use this for initialization
	void Start () {
        ghost = GameObject.FindGameObjectWithTag("Ghost");
	}
	
	// Update is called once per frame
	void Update () {
        if (checkGhostVisibility())
        {
            Vector2 force = ghost.transform.position - this.transform.position;
            force.Normalize();
            force *= pushForce;
            ghost.GetComponent<Rigidbody2D>().AddForce(force);
            drainGhostPower();
        }



        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(this.gameObject);
        }
	}


    bool checkGhostVisibility()
    {
        if(Physics2D.Raycast(this.transform.position, ghost.transform.position - this.transform.position))
        {
            RaycastHit2D temp = Physics2D.Raycast(this.transform.position, ghost.transform.position - this.transform.position);
            if (temp.transform.tag == "Ghost")
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
