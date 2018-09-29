using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour {
    [SerializeField] float moveSpeed = 1f;
    GameObject object2interact;

    [SerializeField] float power = 10.0f;
    [SerializeField] const float insanityMultiplier = 1.0f;
    [SerializeField] const float insanityRange = 3.0f;


    bool isInteracting = false;

    GameObject player;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        


        if (Input.GetButtonDown("Interact2"))
        {
            Debug.Log("Button X, ghost");
            isInteracting = true;

            interact();
        }
        if (Input.GetButtonUp("Interact2"))
        {
            isInteracting = false;
            uninteract();
        }


        if (isInteracting == false && (Input.GetAxis("Horizontal2") > 0.1f || Input.GetAxis("Horizontal2") < -0.1f 
            || Input.GetAxis("Vertical2") > 0.1f || Input.GetAxis("Vertical2") < -0.1f))
        {
            Vector2 temp = new Vector2(Input.GetAxis("Horizontal2"), -Input.GetAxis("Vertical2"));
            temp.Normalize();
            temp *= moveSpeed;
            GetComponent<Rigidbody2D>().AddForce(temp);
        }


    }

    public void losePower(float amount)
    {
        power -= amount;


        if (power < 0)
        {
            power = 0;
        }
    }

    public void getPower(float amount)
    {
        power += amount;
    }

    void drainSanity()
    {
        if (power > 0.1f && Vector3.Distance(this.transform.position, player.transform.position) <= insanityRange)
        {
            RaycastHit2D temp = Physics2D.Raycast(this.transform.position, player.transform.position - this.transform.position, insanityRange);
            Debug.DrawRay(this.transform.position, player.transform.position - this.transform.position);

            if (temp && temp.transform.gameObject == player)
            {

                player.GetComponent<PlayerControl>().drainSanity(Time.deltaTime * insanityMultiplier);


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
            // ADD more scripts later, like containers and doors


        }
    }

    public void interactiondone()
    {
        isInteracting = false;
    }

}
