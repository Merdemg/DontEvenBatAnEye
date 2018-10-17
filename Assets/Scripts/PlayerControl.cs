using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {

    [SerializeField] Text sanityText, boozeText;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float sanity = 100.0f;
    [SerializeField] float drinkingSpeed = 1.0f;
    [SerializeField] float boozeSanity = 33.3f;

    [SerializeField] Image BoozeProg;
    [SerializeField] Image WardProg;
    [SerializeField] Image TrapProg;

    const float maxSanity = 100.0f;
    int boozeNum = 1;
    bool isDrinking = false;
    public Image SanUI;
    public GameObject object2interact;
    bool isInteracting = false;
    bool isDroppingMine = false;
    [SerializeField] float mineCost = 25f;
    [SerializeField] float mineDropTime = 2.0f;
    [SerializeField] GameObject mine;
    float timer = 0;

    [SerializeField] GameObject ward;
    [SerializeField] float wardCost = 25f;
    [SerializeField] float wardDropTime = 1.0f;
    bool isDroppingWard = false;

    int evidence = 0;
    int evidenceRequired = 5;
    [SerializeField] Text evidenceText;

    float blinkTimer = 0;
    const float blinkTimerMax = 0.5f;
    //bool isBlinking = false;
    const float blinkSpeed = 0.2f;
    float blinkSwitchTimer = 0;


    [SerializeField] Image iconBooze, iconTrap, iconWard, buttonBooze, buttonTrap, buttonWard;

    // Use this for initialization
    void Start () {
        updateSanityUI();
	}
	
	// Update is called once per frame
	void Update () {
        // UI ICONS
        if(boozeNum > 0)
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

        // TEMP, Turn investigator green, yellow, red depending on sanity amount
        if(blinkTimer > 0)
        {   // BLINKING AFFECT when sanit dmg taken
            blinkTimer -= Time.deltaTime;
            //isBlinking = true;
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


        // BUTTONS (skills)
        if (Input.GetButtonDown("Interact1") && isDroppingMine == false && isDroppingWard == false && isDrinking == false)
        {
            Debug.Log("Button X, player");
            isInteracting = true;

            interact();
        }
        if (Input.GetButtonUp("Interact1"))
        {
            isInteracting = false;
            uninteract();
        }

        if (Input.GetButtonDown("Mine") && isInteracting == false && sanity > mineCost && isDroppingWard == false && isDrinking == false)
        {
            Debug.Log("Button O, player");
            isDroppingMine = true;
            timer = 0;
        }
        if (Input.GetButtonUp("Mine"))
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


        if (Input.GetButtonDown("Ward") && isInteracting == false && sanity > wardCost && isDroppingMine == false && isDrinking == false)
        {
            Debug.Log("Button 1, player");
            isDroppingWard = true;
            timer = 0;
        }
        if (Input.GetButtonUp("Ward"))
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


        if (Input.GetButtonDown("Booze") && isInteracting == false && isDroppingMine == false && isDroppingWard == false && boozeNum>0)
        {
            Debug.Log("Button 3, player");
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


        if (isInteracting == false && isDroppingMine == false && isDroppingWard == false &&
            (Input.GetAxis("Horizontal1") > 0.1f || Input.GetAxis("Horizontal1") < -0.1f 
            || Input.GetAxis("Vertical1") > 0.1f || Input.GetAxis("Vertical1") < -0.1f))
        {
            Debug.Log(Input.GetAxis("Horizontal1"));
            Debug.Log(Input.GetAxis("Vertical1"));
            //ROTATION
            float angle = Mathf.Atan2(Input.GetAxis("Horizontal1"), -Input.GetAxis("Vertical1")) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
           // Debug.Log(angle);
            //MOVEMENT
            Vector2 temp = new Vector2(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1"));
            temp.Normalize();
            temp *= moveSpeed;
            GetComponent<Rigidbody2D>().AddForce(temp);
        }

        
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
                Debug.Log("interaction w a power s");
                object2interact.GetComponent<PowerSource>().getInteracted(this.gameObject);
            }
            else if (object2interact.GetComponent<Stairs>())
            {
                Debug.Log("interaction w stairs");
                object2interact.GetComponent<Stairs>().getInteracted();
            }
            else if (object2interact.GetComponent<Door>())
            {
                object2interact.GetComponent<Door>().getInteracted(this.gameObject);
            }
            else if (object2interact.GetComponent<Container>())
            {
                Debug.Log("interaction w CONTAINER");
                object2interact.GetComponent<Container>().getInteracted();
            }
            // ADD more scripts later, like containers and doors



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
            print("Investigator Wins!");
        }
    }

    public void getBooze()
    {
        boozeNum++;
        updateSanityUI();
    }

}
