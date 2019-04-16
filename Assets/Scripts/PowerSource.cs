using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerSource : MonoBehaviour {

    [SerializeField] float powerAmount = 5.0f;
    [SerializeField] float powerInterval = 10.0f;
    [SerializeField] float ghostInteractTime = 4.5f;
    [SerializeField] float playerInteractTime = 2f;
    [SerializeField] float interactDistance = 2.5f;
    [SerializeField] GameObject feedbackObj;
    [SerializeField] float activationBonus = 0;
    [SerializeField] float deactivationPenalty = 0;

	[SerializeField] Animator anim_Pentagram;

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

   // GameObject pLight1;
    GameObject pLight2;
    //public GameObject[] fire; //Set to 5 in editor
    ParticleSystem[] particles;

    //Pentagram reset
    [SerializeField] float resetTick = 0.01f;

    void Start()
    {
        particles = GetComponentsInChildren<ParticleSystem>();

        player = GameObject.FindGameObjectWithTag("Player");
        ghost = GameObject.FindGameObjectWithTag("Ghost");
        //feedbackObj.GetComponent<Image>().enabled = false;
//        pLight1 = this.transform.Find("Point Light1").gameObject;
        pLight2 = this.transform.Find("Activated_Light").gameObject;

        LoadCandles();

    }

    void LoadCandles()
    {
        //for (int i = 0; i <= 4; i++)
        //{
        //    fire[i] = this.transform.Find("Candles/candle" + i + "/Fire_B" + i).gameObject;
        //}
    }

	// Update is called once per frame
	void Update ()
    {
        if (isActive)
        {
         //   GetComponent<SpriteRenderer>().color = Color.black;
         //   pLight1.SetActive(true);

			anim_Pentagram.SetBool ("Active", true);

            pLight2.SetActive(true);
            for (int i = 0; i <= 4; i++)
            {
                //fire[i].SetActive(true);
                foreach (ParticleSystem partcle in particles)
                {
                    partcle.Play();
                }
            }
        }
        else
        {
           // GetComponent<SpriteRenderer>().color = Color.white;
//            pLight1.SetActive(false);
            pLight2.SetActive(false);
			anim_Pentagram.SetBool ("Active", false);
            //for (int i = 0; i <= 4; i++)
            //{
            //    //fire[i].SetActive(false);
            //}
            foreach (ParticleSystem partcle in particles)
            {
                partcle.Stop();
            }
        }

        if (!isActive && isGhostInteracting && Vector3.Distance(this.transform.position, ghost.transform.position) > interactDistance)
        {
            isGhostInteracting = false;
            anim_Pentagram.SetTrigger("GStop");
            ghostCanInter = false;

            if (ghost.GetComponent<GhostController>().getObj2Interact() == this.gameObject)
            {
                ghost.GetComponent<GhostController>().setObject2Interact(null);
                //GhostAnimController.isHaunt = false;
            }

            if (feedbackOn)
            {
                deactivateFeedback();
            }

            if (!playerCanInter)
            {
                feedbackOn = false;
            }

            
            
        }

        //if(isActive == false && Vector3.Distance(this.transform.position, ghost.transform.position) <= interactDistance)
        //{
        //    RaycastHit2D temp = Physics2D.Raycast(this.transform.position, ghost.transform.position - this.transform.position, interactDistance);
        //    Debug.DrawRay(this.transform.position, ghost.transform.position - this.transform.position);
        //    if(temp && temp.transform.gameObject == ghost)
        //    {
        //        if (feedbackOn == false)
        //        {
        //            //FeedbackTimer.fillAmount = 1f - Percentage; 
        //            activateFeedback();
        //            ghostCanInter = true;
        //            GhostAnimController.isHaunt = true;
        //            ghost.GetComponent<GhostController>().setObject2Interact(this.gameObject);

        //        }
        //    }
        //    else if (feedbackOn)
        //    {
        //        deactivateFeedback();
        //        ghostCanInter = false;
        //        GhostAnimController.isHaunt = false;
        //    }
        //}
        //else if (isActive == true && Vector3.Distance(this.transform.position, player.transform.position) <= interactDistance)
        //{
            
        //    RaycastHit2D temp = Physics2D.Raycast(this.transform.position, player.transform.position - this.transform.position, interactDistance);
        //    Debug.DrawRay(this.transform.position, player.transform.position - this.transform.position);

        //    if (temp && temp.transform.gameObject == player)
        //    {
        //        if (feedbackOn == false)
        //        {
        //            //FeedbackTimer.fillAmount = 1f - Percentage; // ??????
        //            activateFeedback();
        //            playerCanInter = true;
                    
        //            player.GetComponent<LivingController>().setObject2Interact(this.gameObject);
        //        }
        //    }
        //    else if (feedbackOn)
        //    {
        //        deactivateFeedback();
        //        ghostCanInter = false;
        //    }
        //}
        //else if (feedbackOn)
        //{
        //    deactivateFeedback();
        //    ghostCanInter = false;
        //    playerCanInter = false;
        //}


       
        if (isGhostInteracting && ghostCanInter)
        {
            Debug.Log("GHOST CAN INTERACT::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::" + ghostCanInter);
            timer += Time.deltaTime;
            Percentage = timer / ghostInteractTime;
            //FeedbackTimer.fillAmount = 1 - Percentage;
            GhostAnimController.isHaunt = true;

            if (timer >= ghostInteractTime)
            {
                isGhostInteracting = false;
                isActive = true;
                timer = 0;
                Percentage = timer / playerInteractTime;
                ghost.GetComponent<GhostController>().interactiondone();
                ghost.GetComponent<GhostController>().getPower(activationBonus);
                //GhostAnimController.isHaunt = false;
            }          
        }
        else if (isPlayerInteracting && playerCanInter)
        {
            timer += Time.deltaTime;
            Percentage = timer / playerInteractTime;
            //FeedbackTimer.fillAmount = 1 - Percentage;

            if (timer >= playerInteractTime)
            {
                isActive = false;
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
            //FeedbackTimer.fillAmount = 1f - Percentage;
        }
        else if (!isActive && ghostCanInter)
        {
            Percentage = timer / ghostInteractTime;
            //FeedbackTimer.fillAmount = 1f - Percentage;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hi. ima a pentagram.");
        if (collision.gameObject.tag == "Player" && isActive)
        {
            player.GetComponent<LivingController>().setObject2Interact(this.gameObject);
            //activateFeedback();
            LivingController.isPentagram = true;
            feedbackOn = true;
            playerCanInter = true;
        }
        else if (collision.gameObject.tag == "Ghost" && !isActive)
        {
            feedbackOn = true;
            ghostCanInter = true;
            GhostAnimController.isHaunt = true;
            ghost.GetComponent<GhostController>().setObject2Interact(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ghost"){
            ghostCanInter = false;
            //GhostAnimController.isHaunt = false;
            ghost.GetComponent<GhostController>().setObject2Interact(null);
            if (!playerCanInter)
            {
                feedbackOn = false;
            }
        }
        else if (collision.gameObject.tag == "Player")
        {
            playerCanInter = false;
            //LivingController.isPentagram = false;
            player.GetComponent<LivingController>().setObject2Interact(null);
            if (!ghostCanInter)
            {
                feedbackOn = true;
            }
        }      
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isActive && player.GetComponent<LivingController>().getObj2Interact() == null)
        {
            player.GetComponent<LivingController>().setObject2Interact(this.gameObject);
            //activateFeedback();
            LivingController.isPentagram = true;
            feedbackOn = true;
            playerCanInter = true;
        }
        else if (collision.gameObject.tag == "Ghost" && !isActive && ghost.GetComponent<GhostController>().getObj2Interact() == null)
        {
            feedbackOn = true;
            ghostCanInter = true;
            GhostAnimController.isHaunt = true;
            ghost.GetComponent<GhostController>().setObject2Interact(this.gameObject);
        }
    }

    void activateFeedback()
    {
        feedbackOn = true;
        //feedbackObj.GetComponent<Image>().enabled = true;
        LivingController.isPentagram = true;
    }
    void deactivateFeedback()
    {
        feedbackOn = false;
        //feedbackObj.GetComponent<Image>().enabled = false;
        LivingController.isPentagram = false;
    }
    public void getInteracted(GameObject obj)
    {   
        if(obj == ghost && ghostCanInter && !isGhostInteracting)
        {
            isGhostInteracting = true;
			anim_Pentagram.SetTrigger ("GInteract");
            GhostAnimController.isHaunt = true;
            print("GHOST - POWERSOURCE");
            //timer = 0;
        }
        else  if(obj == player && playerCanInter && !isPlayerInteracting)
        {
            isPlayerInteracting = true;
			anim_Pentagram.SetTrigger ("PInteract");
            //timer = 0;
        }
        else
        {
            //GhostAnimController.isHaunt = false;
        }
    }
    public void stopBeingInteracted(GameObject obj)
    {
        if(obj == player && isPlayerInteracting)
        {
            isPlayerInteracting = false;
			anim_Pentagram.SetTrigger ("PStop");
            //FeedbackTimer.fillAmount = 1f - Percentage;
        }
        else if (obj == ghost & isGhostInteracting)
        {
            isGhostInteracting = false;
			anim_Pentagram.SetTrigger ("GStop");
            //GhostAnimController.isHaunt = false;
            //print ("interactttttttttt")
            //FeedbackTimer.fillAmount = 1f - Percentage;
            //timer = 0;
            //Percentage = timer / ghostInteractTime;
        }
        //timer = 0;
    }
}
