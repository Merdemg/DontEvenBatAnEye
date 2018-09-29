using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {
    [SerializeField] Text sanityText;

    [SerializeField] float moveSpeed = 1f;

    [SerializeField] float sanity = 100.0f;
    const float boozeSanity = 33.3f;
    const float maxSanity = 100.0f;
    int boozeNum = 0;
    

    public GameObject object2interact;
    bool isInteracting = false;


    // Use this for initialization
    void Start () {
        updateSanityUI();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Interact1"))
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




        //Vector3 dir = new Vector3(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1"), 0);
        //dir += this.transform.position;
        //transform.forward = dir;

        // temp += this.transform.position;
        //transform.LookAt(temp);

        //Debug.Log(Input.GetAxis("Horizontal1"));
        //Debug.Log(Input.GetAxis("Vertical1"));

        if (isInteracting == false &&  (Input.GetAxis("Horizontal1") > 0.1f || Input.GetAxis("Horizontal1") < -0.1f 
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

    void updateSanityUI()
    {
        sanityText.text = "Sanity: " + (int)sanity;
    }



}
