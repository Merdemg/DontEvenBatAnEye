using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container : MonoBehaviour {
    public Color containerColor;

    bool hasBooze = false;
    bool hasEvidence = false;
    [SerializeField] float useTime = 4.0f;
    public Image FeedbackTimer;
    float Percentage;

    GameObject player;
    [SerializeField] GameObject feedbackObj;
    float interactDistance = 0.5f;
    bool playerCanInteract = false;
    float timer = 0;
    bool isPlayerInteracting = false;
    GameObject highlight;
    [SerializeField] GameObject containerObj;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        feedbackObj.GetComponent<Image>().enabled = false;

        foreach (Transform child in transform)
        {
            if (child.tag == "Highlight")
            {
                highlight = child.gameObject;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(this.transform.position, player.transform.position) <= interactDistance)
        {
            feedbackObj.GetComponent<Image>().enabled = true;

            player.GetComponent<PlayerControl>().setObject2Interact(this.gameObject);
            playerCanInteract = true;

        }
        else
        {
            feedbackObj.GetComponent<Image>().enabled = false;
            playerCanInteract = false;
            timer = 0;
        }


        if (isPlayerInteracting)
        {
            timer += Time.deltaTime;
            Percentage = timer / useTime;
            FeedbackTimer.fillAmount = 1 - Percentage;
            if (timer >= useTime)
            {
                timer = 0;
                // do stuff
                if (hasEvidence)
                {
                    player.GetComponent<PlayerControl>().getEvidence();
                }
                if (hasBooze)
                {
                    player.GetComponent<PlayerControl>().getBooze();
                }

                feedbackObj.GetComponent<Image>().enabled = false;
                Destroy(highlight);
                Destroy(this);
                gameObject.GetComponent<SpriteRenderer>().color = containerColor;
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
        FeedbackTimer.fillAmount = 1;
    }


    public void getEvidence()
    {
        hasEvidence = true;
    }

    public void getBooze()
    {
        hasBooze = true;
    }
}
