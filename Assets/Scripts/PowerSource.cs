using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerSource : MonoBehaviour {
    [SerializeField] float powerAmount = 5.0f;
    [SerializeField] float powerInterval = 10.0f;
    [SerializeField] float ghostInteractTime = 4.5f;
    [SerializeField] float playerInteractTime = 2f;
    [SerializeField] float interactDistance = 2f;
    [SerializeField] GameObject feedbackObj;
    [SerializeField] float activationBonus = 0;
    [SerializeField] float deactivationPenalty = 0;
    public bool isActive = false;
    bool feedbackOn = false;
    public Image FeedbackTimer;
    float Percentage;

    bool ghostCanInter = false;
    bool playerCanInter = false;


    bool isGhostInteracting = false;
    bool isPlayerInteracting = false;

    GameObject player, ghost;

    float timer = 0;

    float powerTimer = 0;

    

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        ghost = GameObject.FindGameObjectWithTag("Ghost");

        feedbackObj.GetComponent<Image>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (isActive)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }


        if(isActive == false && Vector3.Distance(this.transform.position, ghost.transform.position) <= interactDistance)
        {
            RaycastHit2D temp = Physics2D.Raycast(this.transform.position, ghost.transform.position - this.transform.position, interactDistance);
            Debug.DrawRay(this.transform.position, ghost.transform.position - this.transform.position);
            if(temp && temp.transform.gameObject == ghost)
            {
                if (feedbackOn == false)
                {
                    activateFeedback();
                    ghostCanInter = true;
                    ghost.GetComponent<GhostController>().setObject2Interact(this.gameObject);

                }


            }
            else if (feedbackOn)
            {
                deactivateFeedback();
                ghostCanInter = false;

            }

        }
        else if (isActive == true && Vector3.Distance(this.transform.position, player.transform.position) <= interactDistance)
        {
            //Debug.Log("Playr is nearby");
            RaycastHit2D temp = Physics2D.Raycast(this.transform.position, player.transform.position - this.transform.position, interactDistance);
            
            Debug.DrawRay(this.transform.position, player.transform.position - this.transform.position);
            

            //Debug.Log(temp.transform.gameObject);
            if (temp && temp.transform.gameObject == player)
            {
                if (feedbackOn == false)
                {
                    activateFeedback();
                    playerCanInter = true;
                    player.GetComponent<PlayerControl>().setObject2Interact(this.gameObject);
                }
            }
            else if (feedbackOn)
            {
                deactivateFeedback();
                ghostCanInter = false;
            }


        }
        else if (feedbackOn)
        {
            deactivateFeedback();
            ghostCanInter = false;
            playerCanInter = false;
        }



        if (isGhostInteracting)
        {
            timer += Time.deltaTime;
            Percentage = timer / ghostInteractTime;
            FeedbackTimer.fillAmount = 1 - Percentage;
            if (timer >= ghostInteractTime)
            {
                isActive = true;
                timer = 0;
                ghost.GetComponent<GhostController>().interactiondone();
                ghost.GetComponent<GhostController>().getPower(activationBonus);
                
            }
            
        }

        if (isPlayerInteracting)
        {
            timer += Time.deltaTime;
            Percentage = timer / playerInteractTime;
            FeedbackTimer.fillAmount = 1 - Percentage;
            if (timer >= playerInteractTime)
            {
                isActive = false;
                powerTimer = 0;
                timer = 0;
                ghost.GetComponent<GhostController>().losePowerWithoutBlinking(deactivationPenalty);

            }
        }

        if (isActive)
        {
            powerTimer += Time.deltaTime;

            if(powerTimer >= powerInterval)
            {
                powerTimer -= powerInterval;
                ghost.GetComponent<GhostController>().getPower(powerAmount);
            }

        }


    }




    void activateFeedback()
    {
        Debug.Log("Activating feedback");
        feedbackOn = true;
        feedbackObj.GetComponent<Image>().enabled = true;
    }

    void deactivateFeedback()
    {
        Debug.Log("Deactivating feedback");
        feedbackOn = false;
        feedbackObj.GetComponent<Image>().enabled = false;
    }


    public void getInteracted(GameObject obj)
    {       // day 4: THIS IS BEGINNING TO FEEL LIKE SPAGETTI CODE. MAKE THE INTERNS FIX THIS MAYBE?
        if(obj == ghost && ghostCanInter && isGhostInteracting == false)
        {
            isGhostInteracting = true;
            timer = 0;


        }else if(obj == player && playerCanInter && isPlayerInteracting == false)
        {
            isPlayerInteracting = true;
            timer = 0;
        }
     
    }



    public void stopBeingInteracted(GameObject obj)
    {
        if(obj == player)
        {
            isPlayerInteracting = false;
            FeedbackTimer.fillAmount = 1;
        }
        else if (obj == ghost)
        {
            isGhostInteracting = false;
            FeedbackTimer.fillAmount = 1;
        }

        timer = 0;

    }
}
