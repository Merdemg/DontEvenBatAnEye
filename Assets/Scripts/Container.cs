using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container : MonoBehaviour
{
    public Color containerColor;

    [SerializeField] BoxCollider2D triggerBox;

    bool hasBooze = false;
    bool hasEvidence = false;
    [SerializeField] float useTime = 4.0f;
    public Image FeedbackTimerBase;
    public Image FeedbackTimerIcon;
    float Percentage;
    GameObject player;
    [SerializeField] GameObject feedbackObj;
    float interactDistance = 0.5f;
    bool playerCanInteract = false;
    float timer = 0;
    bool isPlayerInteracting = false;
    GameObject highlight;
    [SerializeField] GameObject containerObj;
    Rigidbody2D rb2D;
    public static bool playerTouch = false;
    private Outline outline;

    [Range(0.0f, 10.0f)]
    public float shimmerSpeed = 5f;
    [Range(0.0f, 5.0f)]
    public float outlineThicc = 2f;

    bool playerIsColliding = false;

	private AudioSource SearchSound;
	public AudioClip clip;

	private float HighlightRadius;

    void Start()
    {
        

        //outline = gameObject.AddComponent<Outline>();
        outline = GetComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        Color myColor = Color.green;
        //Color myColor = new Color(46, 176, 110);
        myColor.a = 0.5f;
        outline.OutlineColor = myColor;
        outline.OutlineWidth = 0f;
        rb2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        feedbackObj.SetActive(false);
        playerCanInteract = false;
        timer = 0;
        foreach (Transform child in transform)
        {
            if (child.tag == "Highlight")
            {
                highlight = child.gameObject;
            }
        }

		SearchSound = gameObject.AddComponent (typeof(AudioSource)) as AudioSource;
		SearchSound.clip = clip; 
		HighlightRadius = 0;
    }

    void Update()
    {
        //When Investigator presses LT and LT sticks
        if (LivingController.isLit && !playerIsColliding)
        {
            feedbackObj.SetActive(true);
            outline.OutlineWidth = Mathf.PingPong(Time.time * shimmerSpeed, outlineThicc);

        }


        //Player cannot interact with object anymore
        else if (!playerCanInteract)
        {
            feedbackObj.SetActive(false);
            outline.OutlineWidth = 0f;
        }
        else if (playerIsColliding)
        {
            outline.OutlineWidth = 2f;
        }
        //Player has stopped presing LT and RT
        else
        {
            outline.OutlineWidth = 0f;
        }
		if (Vector3.Distance (gameObject.transform.position, player.gameObject.transform.position) < HighlightRadius) {
            feedbackObj.SetActive(true);
		}

        if (isPlayerInteracting)
        {
            InvestigatorAnimations.isSearching = true;
            timer += Time.deltaTime;
            Percentage = timer / useTime;
            FeedbackTimerBase.fillAmount = 1 - Percentage;
            FeedbackTimerIcon.fillAmount = 1 - Percentage;

            if (!SearchSound.isPlaying) {
				SearchSound.Play ();
			}

            if (timer >= useTime)
            {
                if (hasEvidence)
                {
                    player.GetComponent<LivingController>().getEvidence();
                }
                if (hasBooze)
                {
                    player.GetComponent<LivingController>().getBooze();
                }
                LivingController.isContainer = false;
                feedbackObj.SetActive(false);
                Destroy(highlight);
                outline.enabled = false; //Object cannot be highlighted once search is complete
                Destroy(this);
                gameObject.GetComponent<SpriteRenderer>().color = containerColor;
                InvestigatorAnimations.isSearching = false;
            }
        }

		if (Vector3.Distance (gameObject.transform.position, player.gameObject.transform.position) < HighlightRadius) {
			feedbackObj.SetActive(true);
			//print(Vector3.Distance (gameObject.transform.position, player.gameObject.transform.position));
		}

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Player")
        //{
        //    feedbackObj.GetComponent<Image>().enabled = true;
        //    player.GetComponent<LivingController>().setObject2Interact(this.gameObject);
        //    playerCanInteract = true;
        //    playerIsColliding = true;
        //    //LoadTexture.isContainer = true;
        //    outline.OutlineColor = Color.white;
        //    LivingController.isContainer = true;
        //}
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Player")
        //{
        //    feedbackObj.GetComponent<Image>().enabled = false;
        //    playerCanInteract = false;
        //    playerIsColliding = false;
        //    //LoadTexture.isContainer = false;
        //    LivingController.isContainer = false;
        //    outline.OutlineColor = Color.yellow;
        //    //timer = 0;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colliding");
        if (collision.gameObject.tag == "Player")
        {
            feedbackObj.SetActive(true);
            player.GetComponent<LivingController>().setObject2Interact(this.gameObject);
            playerCanInteract = true;
            playerIsColliding = true;
            //LoadTexture.isContainer = true;
            outline.OutlineColor = Color.white;
            LivingController.isContainer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            feedbackObj.SetActive(false);
            playerCanInteract = false;
            playerIsColliding = false;
            //LoadTexture.isContainer = false;
            LivingController.isContainer = false;
            outline.OutlineColor = Color.yellow;
            //timer = 0;
        }
    }

    public void getInteracted()
    {
        if (playerCanInteract)
        {
            isPlayerInteracting = true;
            //FeedbackTimer.fillAmount = 1 - Percentage;

        }

        //timer = 0;
    }

    public void stopBeingInteracted()
    {
        //timer = 0;
        isPlayerInteracting = false;
        FeedbackTimerBase.fillAmount = 1 - Percentage;
        FeedbackTimerIcon.fillAmount = 1 - Percentage;
    }


    public void getEvidence()
    {
        hasEvidence = true;
    }

    public void getBooze()
    {
        hasBooze = true;
    }
}
