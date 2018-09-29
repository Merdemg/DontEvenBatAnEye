using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    GameObject player, ghost;
    bool isLocked = false;
    [SerializeField] float ghostInteractTime = 2f;
    [SerializeField] float playerInteractTime = 4.5f;
    [SerializeField] float interactDistance = 2f;
    [SerializeField] GameObject feedbackObj;


    bool feedbackOn = false;
    bool ghostCanInter = false;
    bool playerCanInter = false;

    bool isPlayerInteracting = false;
    bool isGhostInteracting = false;

    float timer = 0;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        ghost = GameObject.FindGameObjectWithTag("Ghost");

        feedbackObj.GetComponent<SpriteRenderer>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (isLocked == false && Vector3.Distance(this.transform.position, player.transform.position) <= interactDistance)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (isLocked == false)
        {
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }


        if (isLocked == true && Vector3.Distance(this.transform.position, player.transform.position) <= interactDistance)
        {
            playerCanInter = true;
            player.GetComponent<PlayerControl>().setObject2Interact(this.gameObject);
        }else if (isLocked == false && Vector3.Distance(this.transform.position, ghost.transform.position) <= interactDistance 
            && ghost.GetComponent<GhostController>().getPowerLevel() > 1)
        {
            ghostCanInter = true;
            ghost.GetComponent<GhostController>().setObject2Interact(this.gameObject);
        }
        else
        {
            playerCanInter = false;
            ghostCanInter = false;
        }


        if(playerCanInter || ghostCanInter)
        {
            feedbackObj.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            feedbackObj.GetComponent<SpriteRenderer>().enabled = false;
        }


        if (isGhostInteracting)
        {
            timer += Time.deltaTime;

            if (timer >= ghostInteractTime)
            {
                isLocked = true;
                isGhostInteracting = false;
            }
        }
        else if (isPlayerInteracting)
        {
            timer += Time.deltaTime;

            if (timer >= playerInteractTime)
            {
                isLocked = false;
                isPlayerInteracting = false;
            }
        }

	}


    public void getInteracted(GameObject obj)
    {
        if (obj == ghost && ghostCanInter && isGhostInteracting == false)
        {
            isGhostInteracting = true;
            timer = 0;
        }
        else if (obj == player && playerCanInter && isPlayerInteracting == false)
        {
            isPlayerInteracting = true;
            timer = 0;
        }
    }

    public void stopBeingInteracted(GameObject obj)
    {
        if (obj == player)
        {
            isPlayerInteracting = false;
        }
        else if (obj == ghost)
        {
            isGhostInteracting = false;
        }
    }




}
