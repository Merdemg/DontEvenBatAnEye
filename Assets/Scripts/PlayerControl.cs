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
    const float maxSanity = 100.0f;
    int boozeNum = 1;
    bool isDrinking = false;

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

    // Use this for initialization
    void Start () {
        updateSanityUI();
	}
	
	// Update is called once per frame
	void Update () {
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
        }

        if (isDroppingMine)
        {
            timer += Time.deltaTime;
            if (timer >= mineDropTime)
            {
                isDroppingMine = false;
                Instantiate(mine, this.transform.position, this.transform.rotation);
                drainSanity(mineCost);
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
        }

        if (isDroppingWard)
        {
            timer += Time.deltaTime;

            if (timer >= wardDropTime)
            {
                isDroppingWard = false;
                Instantiate(ward, this.transform.position, this.transform.rotation);
                drainSanity(wardCost);
            }
        }


        if (Input.GetButtonDown("Booze") && isInteracting == false && isDroppingMine == false && isDroppingWard == false)
        {
            Debug.Log("Button 3, player");
            isDrinking = true;
            timer = 0;
        }
        if (Input.GetButtonUp("Booze"))
        {
            isDrinking = false;
        }

        if (isDrinking)
        {
            timer += Time.deltaTime;
            if (timer >= drinkingSpeed)
            {
                isDrinking = false;
                gainSanity(boozeSanity);
            }
        }

        //Vector3 dir = new Vector3(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1"), 0);
        //dir += this.transform.position;
        //transform.forward = dir;

        // temp += this.transform.position;
        //transform.LookAt(temp);

        //Debug.Log(Input.GetAxis("Horizontal1"));
        //Debug.Log(Input.GetAxis("Vertical1"));

        if (isInteracting == false && isDroppingMine == false && isDroppingWard == false &&
            (Input.GetAxis("Horizontal1") > 0.1f || Input.GetAxis("Horizontal1") < -0.1f 
            || Input.GetAxis("Vertical1") > 0.1f || Input.GetAxis("Vertical1") < -0.1f))
        {
            //ROTATION
            float angle = Mathf.Atan2(Input.GetAxis("Horizontal1"), -Input.GetAxis("Vertical1")) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            //MOVEMENT
            Vector2 temp = new Vector2(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1"));
            temp.Normalize();
            temp *= moveSpeed;
            GetComponent<Rigidbody2D>().AddForce(temp);
        }
 


        
        //this.transform.eulerAngles = new Vector3(0, Mathf.Atan2(Input.GetAxis("Vertical1"), Input.GetAxis("Horizontal1")) * 180 / Mathf.PI, 0);


        
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
        sanityText.text = "Sanity: " + (int)sanity;
        boozeText.text = "Booze: " + boozeNum;
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
