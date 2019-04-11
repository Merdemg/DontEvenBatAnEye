using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stairs : MonoBehaviour {

    [SerializeField] float useTime = 2.0f;
    [SerializeField] GameObject feedbackObj;
    [SerializeField] float interactDistance = 1.0f;

    public Image FeedbackTimer;
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
        feedbackObj.GetComponent<Image>().enabled = false;
    }
	
	void Update ()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) <= interactDistance)
        {
           // LoadTexture.isStairs = true;

            feedbackObj.GetComponent<Image>().enabled = true;
           player.GetComponent<LivingController>().setObject2Interact(this.gameObject);
            playerCanInteract = true;
        }
        else
        {
            feedbackObj.GetComponent<Image>().enabled = false;
            playerCanInteract = false;
            timer = 0;
           // LoadTexture.isStairs = false;
        }

        if (isPlayerInteracting)
        {
            timer += Time.deltaTime;
            Percentage = timer / useTime;
            FeedbackTimer.fillAmount = 1 - Percentage;
            if (timer >= useTime)
            {               
                player.transform.position = myPair.transform.position;
                LookAtCamera.changedFloors = !LookAtCamera.changedFloors;
                timer = 0;
            }
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
        FeedbackTimer.fillAmount = 1;

        InvestigatorAnimations.isFireplace = false;
        InvestigatorAnimations.isStairs = false;
        print("DONE FIREPLACE " + InvestigatorAnimations.isFireplace);
        print("DONE STAIRS " + InvestigatorAnimations.isStairs);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<LivingController>())
        {
            LivingController.isStairs = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<LivingController>())
        {
            LivingController.isStairs = false;
        }
    }


}
