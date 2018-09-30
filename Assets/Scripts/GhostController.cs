using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostController : MonoBehaviour {
    [SerializeField] Transform anchor1, anchor2;
    [SerializeField] Text powerText, powerLevelText;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] GameObject object2interact;

    [SerializeField] float power = 10.0f;
    int powerLevel = 1;
    [SerializeField] float insanityMultiplier = 1.0f;
    [SerializeField] float insanityRange = 3.0f;


    bool isInteracting = false;
    bool isFlying = false;
    [SerializeField] const float flyTime = 1.0f;
    float flyTimer = 0;

    GameObject player;

    public LayerMask ghostMask;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        updatePowerLevel();
        updatePowerText();
	}

    // Update is called once per frame
    void Update() {



        if (Input.GetButtonDown("Interact2"))
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

        if (Input.GetButtonDown("Fly"))
        {
            Debug.Log("Button Y, ghost");
            isFlying = true;
            flyTimer = 0;
        } else
        if (Input.GetButtonUp("Interact2"))
        {
            isFlying = false;
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



            if (isInteracting == false && (Input.GetAxis("Horizontal2") > 0.1f || Input.GetAxis("Horizontal2") < -0.1f 
            || Input.GetAxis("Vertical2") > 0.1f || Input.GetAxis("Vertical2") < -0.1f))
        {
            Vector2 temp = new Vector2(Input.GetAxis("Horizontal2"), -Input.GetAxis("Vertical2"));
            temp.Normalize();
            temp *= moveSpeed;
            GetComponent<Rigidbody2D>().AddForce(temp);
        }

        drainSanity();
    }

    public void losePower(float amount)
    {
        power -= amount;


        if (power < 0)
        {
            power = 0;
        }

        updatePowerLevel();
        updatePowerText();
    }

    public void getPower(float amount)
    {
        power += amount;
        updatePowerLevel();
        updatePowerText();
    }

    void drainSanity()
    {
        if (power > 0.1f && Vector3.Distance(this.transform.position, player.transform.position) <= insanityRange)
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
}
