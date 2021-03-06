﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using TMPro;

public class EndGame : MonoBehaviour {

    private bool isInteracting = false;
    public Image feedbackTimerBase;
    public Image feedbackTimerIcon;
    public GameObject feedbackObj;
    float percentage;
    float timer = 0.0f;
    Player player;
    public float useTime = 2.0f;
    public static bool isActive = false;

    Outline outline;
    float counter = 0;
    float blinkTime = 0.33f;

    [SerializeField] GameObject finalText;

    //bool isEndTimes = false;
    GameObject playerObj;
    private void Start()
    {
        outline = GetComponent<Outline>();

        int playerIdentity = Movement.playerId;
        player = ReInput.players.GetPlayer(playerIdentity);

        feedbackObj.SetActive(false);

        playerObj = GameObject.FindGameObjectWithTag("Player");
    }
    void Update ()
    {
        //feedbackObj.SetActive(false);
        if (player.GetButton("Interact") && isInteracting && isActive)
        {
            timer += Time.deltaTime;
            print(timer);
            percentage = timer / useTime;
            feedbackTimerBase.fillAmount = 1 - percentage;
            feedbackTimerIcon.fillAmount = 1 - percentage;
            if (timer >= useTime)
            {
                print("GAME OVER! Investigator Wins!");
                timer = 0;
                FindObjectOfType<GameEndingScript>().PlayerWin();
                Time.timeScale = 0;
            }
        }

        if (isActive)
        {
            counter += Time.deltaTime;
            if (counter >= blinkTime)
            {
                counter -= blinkTime;
                outline.enabled = !outline.enabled;
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && isActive)
        {
            feedbackObj.SetActive(true);
            //playerObj.GetComponent<LivingController>().setObject2Interact(this.gameObject);
            isInteracting = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && isActive)
        {
            feedbackObj.SetActive(false);
            isInteracting = false;
        }
    }


    public void endGameIsHere()
    {
        isActive = true;
        finalText.SetActive(true);
    }


}
