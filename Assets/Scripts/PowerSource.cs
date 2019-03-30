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

    GameObject pLight1;
    GameObject pLight2;
    public GameObject[] fire; //Set to 5 in editor

    //Pentagram reset
    [SerializeField] float resetTick = 0.01f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ghost = GameObject.FindGameObjectWithTag("Ghost");
        feedbackObj.GetComponent<Image>().enabled = false;
        pLight1 = this.transform.Find("Point Light1").gameObject;
        pLight2 = this.transform.Find("Point Light2").gameObject;

        LoadCandles();

    }

    void LoadCandles()
    {
        for (int i = 0; i <= 4; i++)
        {
            fire[i] = this.transform.Find("Candles/candle" + i + "/Fire_B" + i).gameObject;
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (isActive)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
            pLight1.SetActive(true);
            pLight2.SetActive(true);
            for (int i = 0; i <= 4; i++)
            {
                fire[i].SetActive(true);
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            pLight1.SetActive(false);
            pLight2.SetActive(false);
            for (int i = 0; i <= 4; i++)
            {
                fire[i].SetActive(false);
            }
        }

        if(isActive == false && Vector3.Distance(this.transform.position, ghost.transform.position) <= interactDistance)
        {
            RaycastHit2D temp = Physics2D.Raycast(this.transform.position, ghost.transform.position - this.transform.position, interactDistance);
            Debug.DrawRay(this.transform.position, ghost.transform.position - this.transform.position);
            if(temp && temp.transform.gameObject == ghost)
            {
                if (feedbackOn == false)
                {
                    FeedbackTimer.fillAmount = 1f - Percentage; 
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
            
            RaycastHit2D temp = Physics2D.Raycast(this.transform.position, player.transform.position - this.transform.position, interactDistance);
            Debug.DrawRay(this.transform.position, player.transform.position - this.transform.position);

            if (temp && temp.transform.gameObject == player)
            {
                if (feedbackOn == false)
                {
                    FeedbackTimer.fillAmount = 1f - Percentage; // ??????
                    activateFeedback();
                    playerCanInter = true;
                    
                    player.GetComponent<LivingController>().setObject2Interact(this.gameObject);
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


       
        if (isGhostInteracting && ghostCanInter)
        {
            Debug.Log("GHOST CAN INTERACT::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::" + ghostCanInter);
            timer += Time.deltaTime;
            Percentage = timer / ghostInteractTime;
            FeedbackTimer.fillAmount = 1 - Percentage;
            if (timer >= ghostInteractTime)
            {
                isGhostInteracting = false;
                isActive = true;
                timer = 0;
                Percentage = timer / playerInteractTime;
                ghost.GetComponent<GhostController>().interactiondone();
                ghost.GetComponent<GhostController>().getPower(activationBonus);
                
            }          
        }
        else if (isPlayerInteracting && playerCanInter)
        {
            timer += Time.deltaTime;
            Percentage = timer / playerInteractTime;
            FeedbackTimer.fillAmount = 1 - Percentage;
            if (timer >= playerInteractTime)
            {
                isActive = false;
                powerTimer = 0;
                timer = 0;
                Percentage = timer / playerInteractTime;
                ghost.GetComponent<GhostController>().losePowerWithoutBlinking(deactivationPenalty);
                isPlayerInteracting = false;
                TutorialManager.playerPentagramCount += 1;
            }
        }
        else if (isActive && playerCanInter)
        {
            Percentage = timer / playerInteractTime;
            FeedbackTimer.fillAmount = 1f - Percentage;
        }
        else if (!isActive && ghostCanInter)
        {
            Percentage = timer / ghostInteractTime;
            FeedbackTimer.fillAmount = 1f - Percentage;
        }

        if (isActive && !isPlayerInteracting && timer > 0)
        {
            timer -= resetTick * Time.deltaTime;
            if(timer < 0)
            {
                print("MAX RESET");
                timer = 0;
            }
        }
        if (!isActive && !isGhostInteracting && timer > 0)
        {
            timer -= resetTick * Time.deltaTime;
            if (timer < 0)
            {
                print("MAX RESET");
                timer = 0;
            }
        }


        // POWER GENERATION
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
        feedbackOn = true;
        feedbackObj.GetComponent<Image>().enabled = true;
        LivingController.isPentagram = true;
    }
    void deactivateFeedback()
    {
        feedbackOn = false;
        feedbackObj.GetComponent<Image>().enabled = false;
        LivingController.isPentagram = false;
    }
    public void getInteracted(GameObject obj)
    {   
        if(obj == ghost && ghostCanInter && !isGhostInteracting)
        {
            isGhostInteracting = true;
            //timer = 0;
        }
        else if(obj == player && playerCanInter && !isPlayerInteracting)
        {
            isPlayerInteracting = true;
            //timer = 0;
        }
    }
    public void stopBeingInteracted(GameObject obj)
    {
        if(obj == player)
        {
            isPlayerInteracting = false;
            //FeedbackTimer.fillAmount = 1f - Percentage;
        }
        else if (obj == ghost)
        {
            isGhostInteracting = false;
            //FeedbackTimer.fillAmount = 1f - Percentage;
            //timer = 0;
            //Percentage = timer / ghostInteractTime;
        }
        //timer = 0;
    }
}
