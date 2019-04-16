using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour {

    GameObject player, ghost;
    [SerializeField] bool isLocked = false;

    [SerializeField] float ghostInteractTime = 2f;
    [SerializeField] float playerInteractTime = 4.5f;
    [SerializeField] float interactDistance = 2f;
    [SerializeField] float doorLockedChance = 30f;

    [SerializeField] GameObject feedbackObj;
    public Image FeedbackTimer;
    [SerializeField] Image FeedbackBG;
    [SerializeField] Image blueLock;
    float Percentage;

    const float lockPrice = 15f;
    bool feedbackOn = false;
    bool ghostCanInter = false;
    bool playerCanInter = false;

    bool isPlayerInteracting = false;
    bool isGhostInteracting = false;

    float timer = 0;

    //public GameObject front;
    //public GameObject back;

    // Use this for initialization
    void Start () {
        blueLock.transform.rotation = FeedbackTimer.transform.rotation;

        float randomValue = Random.Range(0f, 100f);
        if (randomValue >= 0 && randomValue <= doorLockedChance)
        {
            isLocked = true;
            Percentage = 0;
        }
            

        player = GameObject.FindGameObjectWithTag("Player");
        ghost = GameObject.FindGameObjectWithTag("Ghost");

        //front = gameObject.transform.Find("Front").gameObject;
        //back = gameObject.transform.Find("Back").gameObject;


        //feedbackObj.SetActive(false);
        Percentage = 0;
    }
	
	void Update () {


        if (isLocked == false && Vector3.Distance(this.transform.position, player.transform.position) <= interactDistance)
        {
            setOpen(true);
            

        }
        else if (isLocked == false)
        {   
            setOpen(false);

        }

        if (isLocked == true && Vector3.Distance(this.transform.position, player.transform.position) <= interactDistance)
        {
            //playerCanInter = true;
            //player.GetComponent<LivingController>().setObject2Interact(this.gameObject);
            blueLock.transform.gameObject.SetActive(false);
        }
        else 
        if (isLocked == false && Vector3.Distance(this.transform.position, ghost.transform.position) <= interactDistance 
            && ghost.GetComponent<GhostController>().getPowerLevel() > 1)
        {   
            ghostCanInter = true;
            ghost.GetComponent<GhostController>().setObject2Interact(this.gameObject);
            FeedbackBG.fillAmount = 1;
            blueLock.transform.gameObject.SetActive(true);
        }
        else
        {
            //playerCanInter = false;
            ghostCanInter = false;
            blueLock.transform.gameObject.SetActive(false);

        }


        if (playerCanInter || ghostCanInter)
        {
            //feedbackObj.SetActive(true);

        }
        //else
        //{
        //    feedbackObj.SetActive(false);
        //}

        if (isGhostInteracting)
        {
            timer += Time.deltaTime;
            Percentage = timer / ghostInteractTime;
            FeedbackTimer.fillAmount = Percentage;
            FeedbackBG.fillAmount = Percentage;
            if (timer >= ghostInteractTime)
            {
                isLocked = true;
                isGhostInteracting = false;
                ghost.GetComponent<GhostController>().losePowerWithoutBlinking(10);
                setOpen(false);
                timer = 0;
                Percentage = timer / playerInteractTime;
            }
        }
        else if (isPlayerInteracting)
        {

            timer += Time.deltaTime;
            Percentage = timer / playerInteractTime;
            FeedbackTimer.fillAmount = 1 - Percentage;
            FeedbackBG.fillAmount = 1 - Percentage;
            if (timer >= playerInteractTime)
            {
                isLocked = false;
                isPlayerInteracting = false;
                timer = 0;
                InvestigatorAnimations.isUnlocking = false;
            }
        }
        else if (isLocked) // It's locked but no one is interacing.
        {
            FeedbackTimer.fillAmount = 1f;  // Perc of being unlocked
            FeedbackBG.fillAmount = 1;
        }
        else    /// It's not locked and no one is interacting
        {
            if (ghostCanInter)
            {
                FeedbackTimer.fillAmount = 0;
                FeedbackBG.fillAmount = 1;
                blueLock.enabled = true;
                blueLock.fillAmount = 1;
            }
            else
            {
                FeedbackTimer.fillAmount = 0;
                FeedbackBG.fillAmount = 0;
                blueLock.fillAmount = 0;
            }
            
        }

    }

    void setOpen(bool isOpen)
    {
        GetComponent<BoxCollider2D>().enabled = !isOpen;
        //GetComponent<SpriteRenderer>().enabled = !isOpen;
        GetComponentInChildren<MeshRenderer>().enabled = !isOpen;
    }

    public void getInteracted(GameObject obj)
    {
        if (obj == ghost && ghostCanInter && isGhostInteracting == false)
        {
            isGhostInteracting = true;
            
        }
        else if (obj == player && playerCanInter && isPlayerInteracting == false)
        {
            isPlayerInteracting = true;

            //float dist1 = Vector3.Distance(player.transform.position, front.transform.position);
            //float dist2 = Vector3.Distance(player.transform.position, back.transform.position);
            //print(dist1 + "AND" + dist2);
            //if (dist1 < dist2)
            //{
            //    print("PLAYER IS IN FRONT");
            //    player.transform.position = front.transform.position;
            //    player.transform.rotation = front.transform.rotation;
            //}
            //else if (dist2 < dist1)
            //{
            //    print("PLAYER IS BACK");
            //    player.transform.position = back.transform.position;
            //    player.transform.rotation = back.transform.rotation;

            //}

            InvestigatorAnimations.isUnlocking = true;
        }
    }

    public void stopBeingInteracted(GameObject obj)
    {
        if (obj == player)
        {
            isPlayerInteracting = false;
            InvestigatorAnimations.isUnlocking = false;
            //FeedbackTimer.fillAmount = 1f - Percentage;
        }
        else if (obj == ghost)
        {
            isGhostInteracting = false;
            //isLocked = false;
            timer = 0;
            Percentage = timer / ghostInteractTime;
            //FeedbackTimer.fillAmount = 0;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        LivingController.isDoor = true;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        LivingController.isDoor = false;
    //        InvestigatorAnimations.isUnlocking = false;


    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isLocked)
        {
            playerCanInter = true;
            LivingController.isDoor = true; //ANim?
            GetComponent<Outline>().enabled = true;
            //feedbackObj.SetActive(true);
            player.GetComponent<LivingController>().setObject2Interact(this.gameObject);
            //playerCanInteract = true;
            //playerIsColliding = true;
            //LoadTexture.isContainer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerCanInter = false;
            LivingController.isDoor = false;
            InvestigatorAnimations.isUnlocking = false;
            GetComponent<Outline>().enabled = false;
            //feedbackObj.SetActive(false);
            isPlayerInteracting = false;
            //playerCanInteract = false;
            //playerIsColliding = false;
            //LoadTexture.isContainer = false;
            //outline.OutlineColor = Color.yellow;
            player.GetComponent<LivingController>().setObject2Interact(null);
            //timer = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.GetComponent<LivingController>().getObj2Interact() == null && isLocked)
        {
            LivingController.isDoor = true; //ANim?
            GetComponent<Outline>().enabled = true;
            playerCanInter = true;
            //feedbackObj.SetActive(true);
            player.GetComponent<LivingController>().setObject2Interact(this.gameObject);
            //playerCanInteract = true;
            //playerIsColliding = true;
            //LoadTexture.isContainer = true;
            //outline.OutlineColor = Color.white;
        }
    }

}
