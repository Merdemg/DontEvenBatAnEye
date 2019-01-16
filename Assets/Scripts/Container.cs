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
    [SerializeField]GameObject feedbackObj;
    float interactDistance = 0.5f;
    bool playerCanInteract = false;
    float timer = 0;
    bool isPlayerInteracting = false;
    GameObject highlight;
    [SerializeField] GameObject containerObj;
    Rigidbody2D rb2D;
    public static bool playerTouch = false;
    void Start () {

        rb2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        feedbackObj.GetComponent<Image>().enabled = false;
        playerCanInteract = false;
        timer = 0;
        foreach (Transform child in transform)
        {
            if (child.tag == "Highlight")
            {
                highlight = child.gameObject;
            }
        }
    }

    void Update()
    {
        if (LivingController.isLit)
            feedbackObj.GetComponent<Image>().enabled = true;
        else if(!playerCanInteract)
            feedbackObj.GetComponent<Image>().enabled = false;

        if (isPlayerInteracting)
        {
            timer += Time.deltaTime;
            Percentage = timer / useTime;
            FeedbackTimer.fillAmount = 1 - Percentage;
            if (timer >= useTime)
            {
                if (hasEvidence)
                {
                    player.GetComponent<LivingController>().getEvidence();
                }
                if (hasBooze)
                {
                    player.GetComponent<LivingController>().getBooze();
                }

                feedbackObj.GetComponent<Image>().enabled = false;
                Destroy(highlight);
                Destroy(this);
                gameObject.GetComponent<SpriteRenderer>().color = containerColor;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            feedbackObj.GetComponent<Image>().enabled = true;
            player.GetComponent<LivingController>().setObject2Interact(this.gameObject);
            playerCanInteract = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            feedbackObj.GetComponent<Image>().enabled = false;
            playerCanInteract = false;
            //timer = 0;
        }
    }
    public void getInteracted()
    {
        if (playerCanInteract)
        {
            isPlayerInteracting = true;
            //FeedbackTimer.fillAmount = 1 - Percentage;

        }

        //timer = 0;
    }

    public void stopBeingInteracted()
    {
        //timer = 0;
        isPlayerInteracting = false;
        FeedbackTimer.fillAmount = 1 - Percentage;
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
