using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using XInputDotNetPure;
public class LivingController : MonoBehaviour {


    [SerializeField] float sanity = 100.0f;
    public Image SanUI;
    public Image SanUIPlayer;
    [Header("UI Text")]
    [SerializeField] Text evidenceText;
    [SerializeField] Text sanityText, boozeText;
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

    public RawImage feedbackUI;

    public Texture[] images;

    public static bool isContainer, isStairs, isPentagram, isDoor, isDrinking,
       isTrap, isWard = false;

    PlayerIndex pIndex;

    void Start ()
    {
        updateSanityUI();
        int playerIdentity = Movement.playerId;
        player = ReInput.players.GetPlayer(playerIdentity);
        feedbackUI = GameObject.Find("PlayerFeedbackUI").GetComponent<RawImage>();
        feedbackUI.texture = images[7];
        altarScript = FindObjectOfType<EndGame>();
    }
	
	void Update ()
    {

        if(Input.GetKey(KeyCode.L))
        {
            GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
        }
        else
        {
            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
        }

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

        if (isContainer) //Works
        {
            feedbackUI.texture = images[6];
            print("Container");
        }
        else if (isStairs)
        {
            feedbackUI.texture = images[1];
            print("Stairs");

        }
        else if (isPentagram) //Pentagram Works
        {
            feedbackUI.texture = images[4];
            print("Pentagram");

        }
        else if (isDoor) //Works
        {
            feedbackUI.texture = images[5];
            print("Door");

        }
        else if (isDrinking) //Works
        {
            feedbackUI.texture = images[3];
            print("Door");

        }
        else if (isTrap) //Works
        {
            feedbackUI.texture = images[2];
            print("Door");

        }
        else if (isWard) //Works
        {
            feedbackUI.texture = images[0];
            print("Door");

        }
        else
            feedbackUI.texture = images[7];
    }


    public void setObject2Interact(GameObject obj)
    {
        object2interact = obj;
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
        
        if (isProtected)
            amount *= protectionValue;

        sanity -= amount;
        updateSanityUI();

        if (sanity <=0)
        {
            Time.timeScale = 0;
        }

        if(amount >0)
        blinkTimer = blinkTimerMax;
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

    void updateSanityUI()
    {
        SanUI.fillAmount = sanity / maxSanity;
        SanUIPlayer.fillAmount = sanity / maxSanity;
        boozeText.text = Investigator.boozeNum.ToString();
        evidenceText.text = "Evidence: " + evidence + "/" + evidenceRequired;
    }

    public void getEvidence()
    {
        evidence++;
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
