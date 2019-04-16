using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stairs : MonoBehaviour {
    [SerializeField] Transform pos;
    [SerializeField] float useTime = 2.0f;
    [SerializeField] GameObject feedbackObj;
    [SerializeField] float interactDistance = 1.0f;

    public Image FeedbackTimerBase;
    public Image FeedbackTimerIcon;
    float Percentage;

    [SerializeField] GameObject myPair;

    bool feedbackOn = false;
    GameObject player;
    bool playerCanInteract = false;
    bool isPlayerInteracting = false;

    float timer = 0;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        feedbackObj.SetActive(false);
    }
	
	void Update ()
    {
        //if (Vector3.Distance(this.transform.position, player.transform.position) <= interactDistance)
        //{
        //   // LoadTexture.isStairs = true;

        //    feedbackObj.SetActive(true);
        //   player.GetComponent<LivingController>().setObject2Interact(this.gameObject);
        //    playerCanInteract = true;
        //}
        //else
        //{
        //    feedbackObj.SetActive(false);
        //    playerCanInteract = false;
        //    timer = 0;
        //   // LoadTexture.isStairs = false;
        //}

        if (isPlayerInteracting)
        {
            timer += Time.deltaTime;
            Percentage = timer / useTime;
            FeedbackTimerBase.fillAmount = 1 - Percentage;
            FeedbackTimerIcon.fillAmount = 1 - Percentage;
            if (timer >= useTime)
            {
                isPlayerInteracting = false;
                player.GetComponent<LivingController>().setObject2Interact(null);


                player.transform.position = myPair.GetComponent<Stairs>().getPos().position;
                LookAtCamera.changedFloors = !LookAtCamera.changedFloors;
                timer = 0;
            }
        }
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colliding");
        //if (collision.GetComponent<LivingController>())
        //{
        //    LivingController.isStairs = true;
        //}
        if (collision.gameObject.tag == "Player")
        {
            feedbackObj.SetActive(true);
            player.GetComponent<LivingController>().setObject2Interact(this.gameObject);
            playerCanInteract = true;
            //playerIsColliding = true;
            //LoadTexture.isContainer = true;
            //outline.OutlineColor = Color.white;
            //LivingController.isStairs = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.GetComponent<LivingController>())
        //{
        //    LivingController.isStairs = false;
        //}

        if (collision.gameObject.tag == "Player")
        {
            feedbackObj.SetActive(false);
            playerCanInteract = false;
            player.GetComponent<LivingController>().setObject2Interact(null);
            //playerIsColliding = false;
            //LoadTexture.isContainer = false;
            //LivingController.isStairs = false;
            //outline.OutlineColor = Color.yellow;
            //timer = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.GetComponent<LivingController>().getObj2Interact() == null)
        {
            feedbackObj.SetActive(true);
            player.GetComponent<LivingController>().setObject2Interact(this.gameObject);
            playerCanInteract = true;
        }
    }

    public void getInteracted()
    {
        if (playerCanInteract)
        {
            isPlayerInteracting = true;

            if (this.tag == "Fireplace")
                InvestigatorAnimations.isFireplace = true;
            else if (this.tag == "Stairs")
                InvestigatorAnimations.isStairs = true;

            print("DONE FIREPLACE " + InvestigatorAnimations.isFireplace);
            print("DONE STAIRS " + InvestigatorAnimations.isStairs);

        }

        timer = 0;
    }

    public void stopBeingInteracted()
    {
        timer = 0;
        isPlayerInteracting = false;
        FeedbackTimerBase.fillAmount = 1;
        FeedbackTimerIcon.fillAmount = 1;

        InvestigatorAnimations.isFireplace = false;
        InvestigatorAnimations.isStairs = false;
        print("DONE FIREPLACE " + InvestigatorAnimations.isFireplace);
        print("DONE STAIRS " + InvestigatorAnimations.isStairs);
    }


    public Transform getPos()
    {
        if (pos)
        {
            return pos;
        }
        return this.transform;
    }
}
