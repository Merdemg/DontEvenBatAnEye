using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PlayerControl : MonoBehaviour {

    [SerializeField] Text sanityText, boozeText;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float sanity = 100.0f;
    [SerializeField] float drinkingSpeed = 1.0f;
    [SerializeField] float boozeSanity = 33.3f;
    [SerializeField] Image BoozeProg;
    [SerializeField] Image WardProg;
    [SerializeField] Image TrapProg;
    [SerializeField] float mineCost = 25f;
    [SerializeField] float mineDropTime = 2.0f;
    [SerializeField] GameObject mine;
    [SerializeField] GameObject ward;
    [SerializeField] float wardCost = 25f;
    [SerializeField] float wardDropTime = 1.0f;
    [SerializeField] Text evidenceText;
    [SerializeField] Image iconBooze, iconTrap, iconWard, buttonBooze, buttonTrap, buttonWard;
    [SerializeField] float protectionValue = 0.25f;
    [SerializeField] LayerMask myMask;

    public GameObject[] wards;
    public Image SanUI;
    public GameObject object2interact;
    public Transform offset;

    bool isDrinking = false;
    bool isDroppingWard = false;
    bool isInteracting = false;
    bool isDroppingMine = false;
    bool isProtected = false;

    float timer = 0;
    float blinkTimer = 0;
    float blinkSwitchTimer = 0;
    const float maxSanity = 100.0f;
    const float blinkTimerMax = 0.5f;
    const float blinkSpeed = 0.2f;
    int evidence = 0;
    int evidenceRequired = 5;
    int boozeNum = 1;

    private Player player;
    public int playerId = 0;
    int indexWard = 0;


    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    void Start ()
    {
        updateSanityUI();
	}
	
	void Update ()
    {

        RaycastHit2D hit = Physics2D.Raycast(offset.position, -Vector2.up);

        if (FindObjectOfType<ward>())
        {
            isProtected = CheckProtected();
        }

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

        if (sanity > wardCost)
        {
            buttonWard.color = new Color(buttonWard.color.r, buttonWard.color.g, buttonWard.color.b, 1.0f);
            iconWard.color = new Color(iconWard.color.r, iconWard.color.g, iconWard.color.b, 1.0f);
        }
        else
        {
            buttonWard.color = new Color(buttonWard.color.r, buttonWard.color.g, buttonWard.color.b, 0.2f);
            iconWard.color = new Color(iconWard.color.r, iconWard.color.g, iconWard.color.b,0.2f);
        }

        if (sanity > mineCost)
        {
            buttonTrap.color = new Color(buttonTrap.color.r, buttonTrap.color.g, buttonTrap.color.b, 1.0f);
            iconTrap.color = new Color(iconTrap.color.r, iconTrap.color.g, iconTrap.color.b, 1.0f);
        }
        else
        {
            buttonTrap.color = new Color(buttonTrap.color.r, buttonTrap.color.g, buttonTrap.color.b, 0.2f);
            iconTrap.color = new Color(iconTrap.color.r, iconTrap.color.g, iconTrap.color.b, 0.2f);
        }

        if(blinkTimer > 0)
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


        if (player.GetButtonDown("Interact") && isDroppingMine == false && isDroppingWard == false && isDrinking == false)
        {
            isInteracting = true;
            interact();
        }
        else
        {
            //print("Not Interactable");
        }
        if (player.GetButtonUp("Interact"))
        {
            isInteracting = false;
            uninteract();
        }

        if (player.GetButtonDown("Mine") && isInteracting == false && sanity > mineCost && isDroppingWard == false && isDrinking == false)
        {
            isDroppingMine = true;
            timer = 0;
        }
        if (player.GetButtonUp("Mine"))
        {
            isDroppingMine = false;
            TrapProg.fillAmount = 0;
        }

        if (isDroppingMine)
        {
            timer += Time.deltaTime;
            TrapProg.fillAmount = timer / mineDropTime;
            if (timer >= mineDropTime)
            {
                isDroppingMine = false;
                Instantiate(mine, this.transform.position, this.transform.rotation);
                drainSanity(mineCost);
                TrapProg.fillAmount = 0;
            }
        }

        if (player.GetButtonDown("Ward") && isInteracting == false && sanity > wardCost && isDroppingMine == false && isDrinking == false)
        {
            isDroppingWard = true;
            timer = 0;
        }
        if (player.GetButtonUp("Ward"))
        {
            isDroppingWard = false;
            WardProg.fillAmount = 0;
        }

        if (isDroppingWard)
        {
            timer += Time.deltaTime;
            WardProg.fillAmount = timer / wardDropTime;
            if (timer >= wardDropTime)
            {
                isDroppingWard = false;
                Instantiate(ward, this.transform.position, this.transform.rotation);
                drainSanity(wardCost);
                WardProg.fillAmount = 0;
            }
        }


        if (player.GetButtonDown("Booze") && isInteracting == false && isDroppingMine == false && isDroppingWard == false && boozeNum>0)
        {
            isDrinking = true;
            timer = 0;
        }

        if (isDrinking)
        {
            timer += Time.deltaTime;
            BoozeProg.fillAmount = timer / drinkingSpeed;
            if (timer >= drinkingSpeed)
            {
                isDrinking = false;
                boozeNum--;
                gainSanity(boozeSanity);
                BoozeProg.fillAmount = 0;
            }
        }

        if (isInteracting == false && isDroppingMine == false && isDroppingWard == false && (player.GetAxis("Horizontal") > 0.1f || player.GetAxis("Horizontal") < -0.1f
            /*player.GetAxis("Horizontal1") > 0.1f || player.GetAxis("Horizontal1") < -0.1f*/ || player.GetAxis("Vertical") > 0.1f || player.GetAxis("Vertical") < -0.1f))
        {          
            float angle = Mathf.Atan2(player.GetAxis("Horizontal"), -player.GetAxis("Vertical")) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            Vector2 temp = new Vector2(player.GetAxis("Horizontal"), player.GetAxis("Vertical"));
            temp.Normalize();
            temp *= moveSpeed;
            GetComponent<Rigidbody2D>().AddForce(temp);
        }

        
        
        
    }

    bool CheckProtected()
    {
        ward[] wards = FindObjectsOfType<ward>();
        
        foreach (ward ward in wards)
        {
            RaycastHit2D temp = Physics2D.Raycast(ward.transform.position, this.transform.position - ward.transform.position, myMask);
            if (temp.transform.gameObject.GetComponent<PlayerControl>())
            {
                print("Player is protected");
                return true;
            }

        }
        print("Player is UNprotected");
        return false;
    }

    public void setObject2Interact(GameObject obj)
    {
        object2interact = obj;
    }

    void interact()
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


    void uninteract()
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
            // ADD more scripts later, like containers and doors


        }
    }

    public void interactiondone()
    {
        isInteracting = false;
    }

    public void drainSanity(float amount)
    {
        if (isProtected)
            amount *= protectionValue;

        sanity -= amount;
        updateSanityUI();

        if (sanity <=0)
        {
            //Player's dead. I mean insane

            Time.timeScale = 0;
        }

        if(amount >0)
        blinkTimer = blinkTimerMax;
    }

    void gainSanity(float amount)
    {
        sanity += amount;

        if (sanity > maxSanity)
        {
            sanity = maxSanity;
        }

        updateSanityUI();
    }

    void updateSanityUI()
    {
        SanUI.fillAmount = sanity / maxSanity;
        boozeText.text = boozeNum.ToString();
        evidenceText.text = "Evidence: " + evidence + "/" + evidenceRequired;
    }

    public void getEvidence()
    {
        evidence++;
        updateSanityUI();

        if (evidence >= evidenceRequired)
        {   //WIN!!!
            Time.timeScale = 0;
        }
    }

    public void getBooze()
    {
        boozeNum++;
        updateSanityUI();
    }

}
