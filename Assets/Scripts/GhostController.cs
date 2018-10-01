using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostController : MonoBehaviour {
    [SerializeField] Transform anchor1, anchor2;
    [SerializeField] Text powerText, powerLevelText;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float phasingSpeed = 0;
    float speedActual;
    [SerializeField] GameObject object2interact;

    [SerializeField] float power = 10.0f;
    int powerLevel = 1;
    [SerializeField] float insanityMultiplier = 1.0f;
    //  [SerializeField] float insanityRange = 3.0f;


    bool isInteracting = false;
    bool isFlying = false;
    [SerializeField] const float flyTime = 1.0f;
    float flyTimer = 0;

    GameObject player;

    public LayerMask ghostMask;

    float blinkTimer = 0;
    const float blinkTimerMax = 0.5f;
    bool isBlinking = false;
    const float blinkSpeed = 0.2f;
    float blinkSwitchTimer = 0;

    bool isHaunting;

    float range = 0.75f;

    bool isTrapped = false;
    bool isPhasing = false;

    [SerializeField] GameObject rangeIndicator;

    [SerializeField] float phasingCost = 25;
    Color myColor;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        updatePowerLevel();
        updatePowerText();
        myColor = GetComponent<SpriteRenderer>().color;
	}

    // Update is called once per frame
    void Update() {
        if (blinkTimer > 0)
        {
            blinkTimer -= Time.deltaTime;
            blinkSwitchTimer += Time.deltaTime;

            if (blinkSwitchTimer >= blinkSpeed)
            {
                blinkSwitchTimer -= blinkSpeed;
                GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
            }


        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;

        }


        if (Input.GetButtonDown("Interact2") && isPhasing == false && isHaunting == false)
        {
            Debug.Log("Button X, ghost");
            isInteracting = true;

            interact();
        } else
        if (Input.GetButtonUp("Interact2"))
        {
            isInteracting = false;
            uninteract();
        }

        if (Input.GetButtonDown("Haunt") && isPhasing == false && isInteracting == false)
        {
            Debug.Log("Buton 1, ghost");
            isHaunting = true;
        }
        else if (Input.GetButtonUp("Haunt"))
        {
            isHaunting = false;
        }

        if (isTrapped == false && Input.GetButtonDown("Fly"))
        {
            Debug.Log("Button Y, ghost");
            isFlying = true;
            flyTimer = 0;
        }

        //else
        //if (Input.GetButtonUp("Fly"))
        //{
        //    isFlying = false;
        //}
        
        if (Input.GetButtonDown("Phase") && isInteracting == false && isHaunting == false && power > (phasingCost))
        {
            isPhasing = true;
            Color temp = this.GetComponent<SpriteRenderer>().color;
            temp.a = 0.5f;
            this.GetComponent<SpriteRenderer>().color = temp;
        }
        else if (Input.GetButtonUp("Phase"))
        {
            isPhasing = false;
            this.GetComponent<SpriteRenderer>().color = myColor;
        }

        if (isFlying)
        {
            flyTimer += Time.deltaTime;

            if (flyTimer >= flyTime)
            {
                fly();
                isFlying = false;
            }
        }


        if (isPhasing)
        {
            losePowerWithoutBlinking(Time.deltaTime * phasingCost);
            if (power < 1)
            {
                isPhasing = false;
                this.GetComponent<SpriteRenderer>().color = myColor;
            }
            updatePowerText();
        }

        if (isInteracting == false && isHaunting == false && (Input.GetAxis("Horizontal2") > 0.1f || Input.GetAxis("Horizontal2") < -0.1f 
            || Input.GetAxis("Vertical2") > 0.1f || Input.GetAxis("Vertical2") < -0.1f))
        {
            if (isPhasing)
            {
                speedActual = phasingSpeed;
            }
            else
            {
                speedActual = moveSpeed;
            }


            Vector2 temp = new Vector2(Input.GetAxis("Horizontal2"), -Input.GetAxis("Vertical2"));
            temp.Normalize();
            temp *= speedActual;
            GetComponent<Rigidbody2D>().AddForce(temp);
        }


        drainSanity();
    }

    public void losePower(float amount)
    {
        if (isPhasing == false)
        {
            losePowerWithoutBlinking(amount);

            if (amount > 0)
                blinkTimer = blinkTimerMax;
        }    
    }

    public void losePowerWithoutBlinking(float amount)
    {
        power -= amount;


        if (power < 0)
        {
            power = 0;
        }

        updatePowerLevel();
        updatePowerText();
    }

    public bool getIfPhasing()
    {
        return isPhasing;
    }

    public void getPower(float amount)
    {
        power += amount;
        updatePowerLevel();
        updatePowerText();
    }

    void drainSanity()
    {
        if (power > 0.1f && isHaunting && Vector3.Distance(this.transform.position, player.transform.position) <= (range * powerLevel))
        {
           // RaycastHit2D temp = Physics2D.Raycast(this.transform.position, player.transform.position - this.transform.position, 
           //     insanityRange, ghostMask);
           // Debug.DrawRay(this.transform.position, player.transform.position - this.transform.position, Color.red);

            //Debug.Log(temp);
            //Debug.Log(temp.transform.gameObject);
            //if (temp && temp.transform.gameObject == player)
            {
                Debug.Log("Almost draining. my soul and motivation to live, i mean.");
                player.GetComponent<PlayerControl>().drainSanity(Time.deltaTime * insanityMultiplier * powerLevel);


            }

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
                object2interact.GetComponent<PowerSource>().getInteracted(this.gameObject);
            }
            else if (object2interact.GetComponent<Door>())
            {
                object2interact.GetComponent<Door>().getInteracted(this.gameObject);
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
            else if (object2interact.GetComponent<Door>())
            {
                object2interact.GetComponent<Door>().stopBeingInteracted(this.gameObject);
            }
            // ADD more scripts later, like containers and doors


        }
    }

    public void interactiondone()
    {
        isInteracting = false;
    }


    void updatePowerLevel()
    {
        if (power < 50f)
        {
            powerLevel = 0;
        }
        else if (power < 100)   // Can damage
        {
            powerLevel = 1;
        }
        else if (power < 250)   // Can lock doors
        {
            powerLevel = 2;
        }
        else
        {                       // Can do possession attack
            powerLevel = 3;
        }

        updateRangeIndicator();
    }

    void updateRangeIndicator()
    {
        Vector3 temp = rangeIndicator.transform.localScale; 
        temp.x = powerLevel * 3.5f;
        temp.y = powerLevel * 3.5f;
        rangeIndicator.transform.localScale = temp;
    }

    void updatePowerText()
    {
        powerText.text = "Power: " + (int)power;
        powerLevelText.text = "Power Lvl: " + powerLevel;
    }

    void fly()
    {
        if(this.transform.position.x <= 0)  //First level
        {
            this.transform.position = (this.transform.position - anchor1.transform.position) + anchor2.transform.position;


        }
        else                                //Second level
        {
            this.transform.position = (this.transform.position - anchor2.transform.position) + anchor1.transform.position;
        }
    }

    public int getPowerLevel()
    {
        return powerLevel;
    }


    public void getTrapped()
    {
        isTrapped = true;
    }

    public void getUntrapped()
    {
        isTrapped = false;
    }
}
