﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour {
    [SerializeField] float rayDistance = 100f;
    [SerializeField] Transform c, r1, r2, r3, r4, l1, l2, l3, l4;
    [SerializeField] float pushForce = 1f;

    [SerializeField] float powerDrainMultiplier = 1.5f;

    GameObject ghost;

    public LayerMask playerMask;
    
    List<Transform> positions;

    // Use this for initialization
    void Start () {
        positions = new List<Transform> { c, r1, r2, r3, r4, l1, l2, l3, l4 };
        Debug.Log(positions.Count);
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {



        if (checkGhostVisibility() && ghost.GetComponent<GhostController>().getIfPhasing() == false)
        {
            Debug.Log("Ghost visible");
            Vector2 force = ghost.transform.position - this.transform.position;
            force.Normalize();
            force *= pushForce;
            ghost.GetComponent<Rigidbody2D>().AddForce(force);

            drainGhostPower();
        }

    }

    bool checkGhostVisibility()
    {
        for (int i = 0; i < positions.Count; i++)
        {
            //Debug.Log("Checking");
            Debug.DrawRay(this.transform.position, positions[i].position - this.transform.position, Color.red);
            if(Physics2D.Raycast(this.transform.position, positions[i].position - this.transform.position, rayDistance, playerMask))
            {
                RaycastHit2D temp = Physics2D.Raycast(this.transform.position, positions[i].position - this.transform.position, rayDistance, playerMask);
                if (temp.transform.tag == "Ghost")
                {
                    ghost = temp.transform.gameObject;
                    return true;
                }
            }
            
        }

        return false;
    }


    void drainGhostPower()
    {
        ghost.GetComponent<GhostController>().losePower(Time.deltaTime * powerDrainMultiplier);
    }
}
