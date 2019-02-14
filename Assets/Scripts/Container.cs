﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container : MonoBehaviour
{
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
    Rigidbody2D rb2D;
    public static bool playerTouch = false;
    private Outline outline;

    [Range(0.0f, 10.0f)]
    public float shimmerSpeed = 5f;
    [Range(0.0f, 5.0f)]
    public float outlineThicc = 2f;

    bool playerIsColliding = false;


    void Start()
    {
        //outline = gameObject.AddComponent<Outline>();
        outline = GetComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 0f;
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
        //When Investigator presses LT and LT sticks
        if (LivingController.isLit && !playerIsColliding)
        {
            feedbackObj.GetComponent<Image>().enabled = true;
            outline.OutlineWidth = Mathf.PingPong(Time.time * shimmerSpeed, outlineThicc);

        }
        //Player cannot interact with object anymore
        else if (!playerCanInteract)
        {
            feedbackObj.GetComponent<Image>().enabled = false;
            outline.OutlineWidth = 0f;
        }
        else if (playerIsColliding)
        {
            outline.OutlineWidth = 2f;
        }
        //Player has stopped presing LT and RT
        else
        {
            outline.OutlineWidth = 0f;
        }


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
                outline.enabled = false; //Object cannot be highlighted once search is complete
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
            playerIsColliding = true;
            outline.OutlineColor = Color.white;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            feedbackObj.GetComponent<Image>().enabled = false;
            playerCanInteract = false;
            playerIsColliding = false;
            outline.OutlineColor = Color.yellow;
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
