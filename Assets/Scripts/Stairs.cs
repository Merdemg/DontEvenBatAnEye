using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour {
    [SerializeField] float useTime = 2.0f;
    [SerializeField] GameObject feedbackObj;
    [SerializeField] float interactDistance = 1.0f;

    [SerializeField] GameObject myPair;

    bool feedbackOn = false;
    GameObject player;
     bool playerCanInteract = false;
     bool isPlayerInteracting = false;

    float timer = 0;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        feedbackObj.GetComponent<SpriteRenderer>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(this.transform.position, player.transform.position) <= interactDistance)
        {
            feedbackObj.GetComponent<SpriteRenderer>().enabled = true;
            player.GetComponent<PlayerControl>().setObject2Interact(this.gameObject);
            playerCanInteract = true;
            // Programmers of the future, if you're reading this, I'm terribly sorry for these scripts
        }
        else
        {
            feedbackObj.GetComponent<SpriteRenderer>().enabled = false;
            playerCanInteract = true;
            timer = 0;
        }



        if (isPlayerInteracting)
        {
            timer += Time.deltaTime;

            if (timer >= useTime)
            {
                timer = 0;
                player.transform.position = myPair.transform.position;
            }
        }
	}


    public void getInteracted()
    {
        if (playerCanInteract)
        {
            isPlayerInteracting = true;

        }

        timer = 0;


    }


    public void stopBeingInteracted()
    {
        timer = 0;
        isPlayerInteracting = false;
    }
}
