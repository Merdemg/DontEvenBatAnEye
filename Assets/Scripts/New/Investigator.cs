using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
//using XInputDotNetPure;

public class Investigator : MonoBehaviour {

    private Player player;
    public Transform offset;
    public static int boozeNum = 1;
    [SerializeField] Image iconBooze, iconTrap, iconxObject, buttonBooze, buttonTrap, buttonxObject;
    [Header("Float Variables")]
    [SerializeField] float sanity = 100.0f;
    [SerializeField] float drinkingSpeed = 1.0f;
    [SerializeField] float boozeSanity = 33.3f;
    [SerializeField] float bObjectCost = 25f;
    [SerializeField] float bObjectDropTime = 2.0f;
    [SerializeField] float xObjectCost = 25f;
    [SerializeField] float xObjectDropTime = 1.0f;
    [SerializeField] float protectionValue = 0.25f;
    float timer = 0;
    float blinkTimer = 0;
    float blinkSwitchTimer = 0;

    const float maxSanity = 100.0f;
    const float blinkTimerMax = 0.5f;
    const float blinkSpeed = 0.2f;
    public Image SanUI;

    [Header("Bools")]
    bool yAbility = false;
    bool xAbility = false;
    bool interactButton = false;
    bool bAbility = false;
    [SerializeField] Image TrapProg;
    [SerializeField] GameObject bObject;
    [SerializeField] GameObject xObject;
    [SerializeField] Image BoozeProgress;
    [SerializeField] Image WardProgress;
    public GameObject[] wards;
    int indexWard = 0;

    public static bool interacting = false;

    public float vibTimer = 3.0f;

    float vibrateTime = 3f;
    float startTime = 0f;

    // Use this for initialization
    void Start () {

        int playerIdentity = Movement.playerId;
        player = ReInput.players.GetPlayer(playerIdentity);
    }
	
    IEnumerator ControllerVibration()
    {
        //GamePad.SetVibration(PlayerIndex.One, 1f, 1f);
        yield return new WaitForSeconds(1f);
        //GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
    }

	// Update is called once per frame
	void Update ()
    {
        RaycastHit2D hit = Physics2D.Raycast(offset.position, -Vector2.up);


        //if (Input.GetKey(KeyCode.L))
        //{
        //    GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
        //}
        //else if(Input.GetKeyUp(KeyCode.L))
        //{
        //    GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
        //}

        if (boozeNum > 0)
        {
            buttonBooze.color = new Color(buttonBooze.color.r, buttonBooze.color.g, buttonBooze.color.b, 1.0f);
            iconBooze.color = new Color(iconBooze.color.r, iconBooze.color.g, iconBooze.color.b, 1.0f);
        }
        else
        {
            buttonBooze.color = new Color(buttonBooze.color.r, buttonBooze.color.g, buttonBooze.color.b, 0.2f);
            iconBooze.color = new Color(iconBooze.color.r, iconBooze.color.g, iconBooze.color.b, 0.2f);

        }

        if (sanity > xObjectCost)
        {
            buttonxObject.color = new Color(buttonxObject.color.r, buttonxObject.color.g, buttonxObject.color.b, 1.0f);
            iconxObject.color = new Color(iconxObject.color.r, iconxObject.color.g, iconxObject.color.b, 1.0f);
        }
        else
        {
            buttonxObject.color = new Color(buttonxObject.color.r, buttonxObject.color.g, buttonxObject.color.b, 0.2f);
            iconxObject.color = new Color(iconxObject.color.r, iconxObject.color.g, iconxObject.color.b, 0.2f);
        }

        if (sanity > bObjectCost)
        {
            buttonTrap.color = new Color(buttonTrap.color.r, buttonTrap.color.g, buttonTrap.color.b, 1.0f);
            iconTrap.color = new Color(iconTrap.color.r, iconTrap.color.g, iconTrap.color.b, 1.0f);
        }
        else
        {
            buttonTrap.color = new Color(buttonTrap.color.r, buttonTrap.color.g, buttonTrap.color.b, 0.2f);
            iconTrap.color = new Color(iconTrap.color.r, iconTrap.color.g, iconTrap.color.b, 0.2f);
        }

        if (blinkTimer > 0)
        {
            blinkTimer -= Time.deltaTime;
            blinkSwitchTimer += Time.deltaTime;

            if (blinkSwitchTimer >= blinkSpeed)
            {
                blinkSwitchTimer -= blinkSpeed;
                GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
            }
            if (sanity >= (.6 * maxSanity))
            {
                GetComponent<SpriteRenderer>().color = Color.green;
                SanUI.color = Color.green;
            }
            else if (sanity >= (.3 * maxSanity))
            {
                GetComponent<SpriteRenderer>().color = Color.yellow;
                SanUI.color = Color.yellow;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                SanUI.color = Color.red;
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;

        }


        if (player.GetButtonDown("Interact") && bAbility == false && xAbility == false && yAbility == false)
        {
            interactButton = true;
            this.GetComponent<LivingController>().interact();
            interacting = true;
        }
        else
        {
            interacting = false; //print("Not Interactable");
        }
        if (player.GetButtonUp("Interact"))
        {
            interactButton = false;
            this.GetComponent<LivingController>().uninteract();
        }

        if (player.GetButtonDown("Mine") && interactButton == false && sanity > bObjectCost && xAbility == false && yAbility == false)
        {
            bAbility = true;
            timer = 0;
        }
        if (player.GetButtonUp("Mine"))
        {
            bAbility = false;
            TrapProg.fillAmount = 0;
        }

        if (bAbility)
        {
            //LoadTexture.isTrap = true;
            LivingController.isTrap = true;
            timer += Time.deltaTime;
            TrapProg.fillAmount = timer / bObjectDropTime;

            InvestigatorAnimations.isTrap = true;

            if (timer >= bObjectDropTime)
            {
                bAbility = false;
                Instantiate(bObject, this.transform.position, this.transform.rotation);
                TutorialManager.playerTrapCount++;
                print(TutorialManager.playerTrapCount);
                this.GetComponent<LivingController>().drainSanity(bObjectCost);
                TrapProg.fillAmount = 0;
                //LoadTexture.isTrap = false;
                LivingController.isTrap = false;
                StartCoroutine(ControllerVibration());
                InvestigatorAnimations.isTrap = false;
            }

        }
        else
        {
            LivingController.isTrap = false;
        }

        if (player.GetButtonDown("Ward") && interactButton == false && sanity > xObjectCost && bAbility == false && yAbility == false)
        {
            xAbility = true;
            timer = 0;
        }
        if (player.GetButtonUp("Ward"))
        {
            xAbility = false;
            WardProgress.fillAmount = 0;
        }

        if (xAbility)
        {
            //LoadTexture.isWard = true;
            LivingController.isWard = true;
            timer += Time.deltaTime;
            WardProgress.fillAmount = timer / xObjectDropTime;
            InvestigatorAnimations.isWard = true;

            if (timer >= xObjectDropTime)
            {
                xAbility = false;
                Instantiate(xObject, this.transform.position, this.transform.rotation);
                TutorialManager.playerWardCount++;
                this.GetComponent<LivingController>().drainSanity(xObjectCost);
                WardProgress.fillAmount = 0;
                StartCoroutine(ControllerVibration());
                InvestigatorAnimations.isWard = false;

                //LoadTexture.isWard = false;
            }

        }
        else
        {
            LivingController.isWard = false;
        }


        if (player.GetButtonDown("Booze") && interactButton == false && bAbility == false && xAbility == false && boozeNum > 0)
        {
            yAbility = true;
            timer = 0;
        }

        if (yAbility)
        {
            //LoadTexture.isBooze = true;
            LivingController.isDrinking = true;
            timer += Time.deltaTime;
            BoozeProgress.fillAmount = timer / drinkingSpeed;
            if (timer >= drinkingSpeed)
            {
                print("BOOZE DRANK");
                yAbility = false;
                boozeNum--;
                this.GetComponent<LivingController>().gainSanity(boozeSanity);
                BoozeProgress.fillAmount = 0;
                //LoadTexture.isBooze = false;
                LivingController.isDrinking = false;
            }
        }
        else
        {
            LivingController.isDrinking = false;
        }

    }

}
