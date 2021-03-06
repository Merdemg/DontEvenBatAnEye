﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using XInputDotNetPure;
using TMPro;

public class LivingController : MonoBehaviour {

    [SerializeField] Animator dmgAnimController;
    [SerializeField] Animator dmgAC_UI;

    [SerializeField] float sanity = 100.0f;
    public static float staticSanity;
    public Slider SanUI;
    public Slider SanUIPlayer;
    [Header("UI Text")]
    [SerializeField] TextMeshProUGUI evidenceText;
    [SerializeField] TextMeshProUGUI boozeText;
    public GameObject object2interact;
    public float protectionValue = 0.25f;
    public static bool isProtected = false;

    bool isInteracting = false;
    public static bool isLit = false;

    float timer = 0;
    float blinkTimer = 0;
    float blinkSwitchTimer = 0;

    const float maxSanity = 100.0f;
    const float blinkTimerMax = 0.5f;
    const float blinkSpeed = 0.2f;

    public static bool isHaunted = false;

    int evidence = 0;
    int evidenceRequired = 5;
    int indexWard = 0;
    private Player player;
    [SerializeField] LayerMask myMask;

    //public GameObject altar;
    EndGame altarScript;

    //public RawImage feedbackUI;

    //public Texture[] images;

    public static bool isContainer, isStairs, isPentagram, isDoor, isDrinking,
       isTrap, isWard = false;

    PlayerIndex pIndex;

	AudioSource FindBooze;
	AudioSource FindEvidence;
    AudioSource DrinkBooze;

    void Start ()
    {
        updateSanityUI();
        int playerIdentity = Movement.playerId;
        player = ReInput.players.GetPlayer(playerIdentity);
        //feedbackUI = GameObject.Find("PlayerFeedbackUI").GetComponent<RawImage>();
        //feedbackUI.texture = images[7];
        altarScript = FindObjectOfType<EndGame>();

		AudioSource[] Sources = GetComponents<AudioSource>();
		FindBooze = Sources [0];
		FindEvidence = Sources [1];
        DrinkBooze = Sources[2];
    }
	
	void Update ()
    {


        staticSanity = sanity;
        if (FindObjectOfType<ward>())
        {
           isProtected = CheckProtected();
        }
        //Highlight Interactable Objects
        if (player.GetButton("Highlight"))
        {
            isLit = true;
        }
        else
            isLit = false;

        //if (isContainer) //Works
        //{
        //    feedbackUI.texture = images[6];
        //}
        //else if (isStairs)
        //{
        //    feedbackUI.texture = images[1];
        //}
        //else if (isPentagram) //Pentagram Works
        //{
        //    feedbackUI.texture = images[4];
        //}
        //else if (isDoor) //Works
        //{
        //    feedbackUI.texture = images[5];
        //}
        //else if (isDrinking) //Works
        //{
        //    feedbackUI.texture = images[3];
        //}
        //else if (isTrap) //Works
        //{
        //    feedbackUI.texture = images[2];
        //}
        //else if (isWard) //Works
        //{
        //    feedbackUI.texture = images[0];
        //}
        //else
        //    feedbackUI.texture = images[7];
    }

    
    public void setObject2Interact(GameObject obj)
    {
        object2interact = obj;
    }

    public GameObject getObj2Interact()
    {
        return object2interact;
    }

    public void interact()
    {
        if (object2interact)
        {
            if (object2interact.GetComponent<PowerSource>())
            {
                object2interact.GetComponent<PowerSource>().getInteracted(this.gameObject);
            }
            else if (object2interact.GetComponent<Stairs>())
            {
                object2interact.GetComponent<Stairs>().getInteracted();
            }
            else if (object2interact.GetComponent<Door>())
            {
                object2interact.GetComponent<Door>().getInteracted(this.gameObject);
            }
            else if (object2interact.GetComponent<Container>())
            {
                object2interact.GetComponent<Container>().getInteracted();
            }
        }
    }


    public void uninteract()
    {
        if (object2interact)
        {
            if (object2interact.GetComponent<PowerSource>())
            {
                object2interact.GetComponent<PowerSource>().stopBeingInteracted(this.gameObject);
            }
            else if (object2interact.GetComponent<Stairs>())
            {
                object2interact.GetComponent<Stairs>().stopBeingInteracted();
            }
            else if (object2interact.GetComponent<Door>())
            {
                object2interact.GetComponent<Door>().stopBeingInteracted(this.gameObject);
            }
            else if (object2interact.GetComponent<Container>())
            {
                object2interact.GetComponent<Container>().stopBeingInteracted();
            }

        }
    }

    public void interactiondone()
    {
        isInteracting = false;
    }

    public void drainSanity(float amount)
    {
        dmgAnimController.SetTrigger("Dmg");
        dmgAC_UI.SetTrigger("Dmg");

        if (isProtected)
            amount *= protectionValue;

        sanity -= amount;
        StartCoroutine(ControllerVibrate());
        updateSanityUI();

        if (sanity <=0)
        {
            FindObjectOfType<GameEndingScript>().GhostWin();
            Time.timeScale = 0;
        }

        if(amount >0)
        blinkTimer = blinkTimerMax;
    }

    IEnumerator ControllerVibrate()
    {
        GamePad.SetVibration(PlayerIndex.One, 1f, 1f);
        yield return new WaitForSeconds(3f);
        GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
    }
    public void gainSanity(float amount)
    {
        sanity += amount;

        if (sanity > maxSanity)
        {
            sanity = maxSanity;
        }

        updateSanityUI();
    }

    public float getSanity()
    {
        return sanity;
    }

    void updateSanityUI()
    {
        //SanUI.fillAmount = sanity / maxSanity;
        //SanUIPlayer.fillAmount = sanity / maxSanity;
        SanUI.value = sanity;

        if(SanUIPlayer)
        SanUIPlayer.value = sanity;

        boozeText.text = Investigator.boozeNum.ToString();
        evidenceText.text = "Evidence: " + evidence + "/" + evidenceRequired;
    }

    public void getEvidence()
    {
        evidence++;
		FindEvidence.Play ();
        TutorialManager.playerEvidenceCount += 1;
        updateSanityUI();

        if (evidence >= evidenceRequired)
        {
            //WIN!!!
            //EndGame.isActive = true;
            altarScript.endGameIsHere();
        }
    }

    public void getBooze()
    {
        Investigator.boozeNum++;
		FindBooze.Play ();
        updateSanityUI();
    }
    bool CheckProtected()
    {
        ward[] wards = FindObjectsOfType<ward>();
        foreach (ward ward in wards)
        {
            RaycastHit2D temp = Physics2D.Raycast(ward.transform.position, this.transform.position - ward.transform.position, myMask);
            if (temp.transform.gameObject.GetComponent<LivingController>())
            {
                return true;
            }

        }
        return false;
    }

}
