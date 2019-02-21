using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class GhostController : MonoBehaviour {
    [SerializeField] Transform anchor1, anchor2;
    [SerializeField] Text powerText, powerLevelText;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float phasingSpeed = 0;   
    [SerializeField] GameObject object2interact;
    [SerializeField] Image PowUI;
    [SerializeField] Image HauntImage;
    [SerializeField] Image PhaseImage;
    [SerializeField] Image HauntButt;
    [SerializeField] Image PhaseButt;
    [SerializeField] Image Pow1;
    [SerializeField] Image Pow2;
    [SerializeField] Image Pow3;
    [SerializeField] Image Pow3a;
    [SerializeField] Image FlyArrow;
    public static float power = 10.0f;

    int powerLevel = 1;
    [SerializeField] float insanityMultiplier = 1.0f;
    bool isInteracting = false;
    bool isFlying = false;
    [SerializeField] const float flyTime = 1.0f;
    float flyTimer = 0;
    float speedActual;
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

    private Player ghost;
    public int playerId = 1;

    [SerializeField] GameObject rangeIndicator;
    [SerializeField] float phasingCost = 5;
    [SerializeField] float hauntingCost = 0;
    Color myColor;

    Rigidbody2D rb;

    GameObject hauntEffect;
    GameObject defaultHauntEffect;


    private void Awake()
    {
        ghost = ReInput.players.GetPlayer(playerId);
    }

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        updatePowerLevel();
        updatePowerText();
        myColor = GetComponent<SpriteRenderer>().color;

        hauntEffect = GameObject.FindGameObjectWithTag("Distortion");
        hauntEffect.SetActive(false);

        defaultHauntEffect = GameObject.FindGameObjectWithTag("DefaultDistortion");
	}

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

        if (ghost.GetButtonDown("Interact") && isPhasing == false && isHaunting == false)
        {
            Debug.Log("Button X, ghost");
            isInteracting = true;

            interact();
        } else
        if (ghost.GetButtonUp("Interact"))
        {
            isInteracting = false;
            uninteract();
        }

        if (ghost.GetButtonDown("Haunt") && isPhasing == false && isInteracting == false)
        {
            Debug.Log("Buton 1, ghost");
            isHaunting = true;
            hauntEffect.SetActive(true);
            defaultHauntEffect.SetActive(false);
        }
        else if (ghost.GetButtonUp("Haunt"))
        {
            isHaunting = false;
            hauntEffect.SetActive(false);
            defaultHauntEffect.SetActive(true);
        }

        if (isTrapped == false && ghost.GetButtonDown("Fly"))
        {
            Debug.Log("Button Y, ghost");
            isFlying = true;
            flyTimer = 0;
        }
        
        if (ghost.GetButtonDown("Phase") && isInteracting == false && isHaunting == false && powerLevel >= 3 && power > (phasingCost))
        {
            isPhasing = true;
            Color temp = this.GetComponent<SpriteRenderer>().color;
            temp.a = 0.5f;
            this.GetComponent<SpriteRenderer>().color = temp;
        }
        else if (ghost.GetButtonUp("Phase"))
        {
            isPhasing = false;
            this.GetComponent<SpriteRenderer>().color = myColor;
        }

        if (isFlying)
        {
            flyTimer += Time.deltaTime;
            FlyArrow.fillAmount = flyTimer / flyTime;
            if (flyTimer >= flyTime)
            {
                fly();
                isFlying = false;
                FlyArrow.fillAmount = 0;
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

        if (isHaunting)
        {
            Debug.Log("ishaunting");
            losePowerWithoutBlinking(Time.deltaTime * hauntingCost * (float)powerLevel);
        }

        if (isInteracting == false && isHaunting == false && (ghost.GetAxis("Horizontal") > 0.1f || ghost.GetAxis("Horizontal") < -0.1f 
            || ghost.GetAxis("Vertical") > 0.1f || ghost.GetAxis("Vertical") < -0.1f))
        {
            if (isPhasing)
            {
                speedActual = phasingSpeed;
            }
            else
            {
                speedActual = moveSpeed;
            }

            Vector2 temp = new Vector2(ghost.GetAxis("Horizontal"), -ghost.GetAxis("Vertical"));
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
        if (powerLevel >= 1 && isHaunting && Vector3.Distance(this.transform.position, player.transform.position) <= (range * powerLevel))
        {     
            {
                Debug.Log("Almost draining. my soul and motivation to live, i mean.");
                //player.GetComponent<PlayerControl>().drainSanity(Time.deltaTime * insanityMultiplier * powerLevel);
                player.GetComponent<LivingController>().drainSanity(Time.deltaTime * insanityMultiplier * powerLevel);
            }
        }
        else
        {
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
            PhaseButt.color = new Color(PhaseButt.color.r, PhaseButt.color.g, PhaseButt.color.b, 0.2f);
            PhaseImage.color = new Color(PhaseImage.color.r, PhaseImage.color.g, PhaseImage.color.b, 0.2f);
            HauntButt.color = new Color(HauntButt.color.r, HauntButt.color.g, HauntButt.color.b, 0.2f);
            HauntImage.color = new Color(HauntImage.color.r, HauntImage.color.g, HauntImage.color.b, 0.2f);
            Pow1.color = new Color(Pow1.color.r, Pow1.color.g, Pow1.color.b, 0.2f);
            Pow2.color = new Color(Pow2.color.r, Pow2.color.g, Pow2.color.b, 0.2f);
            Pow3.color = new Color(Pow3.color.r, Pow3.color.g, Pow3.color.b, 0.2f);
            Pow3a.color = new Color(Pow3a.color.r, Pow3a.color.g, Pow3a.color.b, 0.2f);
        }
        else if (power < 100f)
        {
            powerLevel = 1;
            PhaseButt.color = new Color(PhaseButt.color.r, PhaseButt.color.g, PhaseButt.color.b, 0.2f);
            PhaseImage.color = new Color(PhaseImage.color.r, PhaseImage.color.g, PhaseImage.color.b, 0.2f);
            HauntButt.color = new Color(HauntButt.color.r, HauntButt.color.g, HauntButt.color.b, 1.0f);
            HauntImage.color = new Color(HauntImage.color.r, HauntImage.color.g, HauntImage.color.b, 1.0f);
            Pow1.color = new Color(Pow1.color.r, Pow1.color.g, Pow1.color.b, 1.0f);
            Pow2.color = new Color(Pow2.color.r, Pow2.color.g, Pow2.color.b, 0.2f);
            Pow3.color = new Color(Pow3.color.r, Pow3.color.g, Pow3.color.b, 0.2f);
            Pow3a.color = new Color(Pow3a.color.r, Pow3a.color.g, Pow3a.color.b, 0.2f);
        }
        else if (power < 150)   // Can damage
        {
            powerLevel = 2;
            PhaseButt.color = new Color(PhaseButt.color.r, PhaseButt.color.g, PhaseButt.color.b, 0.2f);
            PhaseImage.color = new Color(PhaseImage.color.r, PhaseImage.color.g, PhaseImage.color.b, 0.2f);
            HauntButt.color = new Color(HauntButt.color.r, HauntButt.color.g, HauntButt.color.b, 1.0f);
            HauntImage.color = new Color(HauntImage.color.r, HauntImage.color.g, HauntImage.color.b, 1.0f);
            Pow1.color = new Color(Pow1.color.r, Pow1.color.g, Pow1.color.b, 1.0f);
            Pow2.color = new Color(Pow2.color.r, Pow2.color.g, Pow2.color.b, 1.0f);
            Pow3.color = new Color(Pow3.color.r, Pow3.color.g, Pow3.color.b, 0.2f);
            Pow3a.color = new Color(Pow3a.color.r, Pow3a.color.g, Pow3a.color.b, 0.2f);
        }
        else if (power < 200)   // Can lock doors
        {
            powerLevel = 3;
            PhaseButt.color = new Color(PhaseButt.color.r, PhaseButt.color.g, PhaseButt.color.b, 1.0f);
            PhaseImage.color = new Color(PhaseImage.color.r, PhaseImage.color.g, PhaseImage.color.b, 1.0f);
            HauntButt.color = new Color(HauntButt.color.r, HauntButt.color.g, HauntButt.color.b, 1.0f);
            HauntImage.color = new Color(HauntImage.color.r, HauntImage.color.g, HauntImage.color.b, 1.0f);
            Pow1.color = new Color(Pow1.color.r, Pow1.color.g, Pow1.color.b, 1.0f);
            Pow2.color = new Color(Pow2.color.r, Pow2.color.g, Pow2.color.b, 1.0f);
            Pow3.color = new Color(Pow3.color.r, Pow3.color.g, Pow3.color.b, 1.0f);
            Pow3a.color = new Color(Pow3a.color.r, Pow3a.color.g, Pow3a.color.b, 1.0f);
        }
        else
        {                       // Can do possession attack
            powerLevel = 4;
            PhaseButt.color = new Color(PhaseButt.color.r, PhaseButt.color.g, PhaseButt.color.b, 1.0f);
            PhaseImage.color = new Color(PhaseImage.color.r, PhaseImage.color.g, PhaseImage.color.b, 1.0f);
            HauntButt.color = new Color(HauntButt.color.r, HauntButt.color.g, HauntButt.color.b, 1.0f);
            HauntImage.color = new Color(HauntImage.color.r, HauntImage.color.g, HauntImage.color.b, 1.0f);
            Pow1.color = new Color(Pow1.color.r, Pow1.color.g, Pow1.color.b, 1.0f);
            Pow2.color = new Color(Pow2.color.r, Pow2.color.g, Pow2.color.b, 1.0f);
            Pow3.color = new Color(Pow3.color.r, Pow3.color.g, Pow3.color.b, 1.0f);
            Pow3a.color = new Color(Pow3a.color.r, Pow3a.color.g, Pow3a.color.b, 1.0f);
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
        PowUI.fillAmount = power / 200;
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
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }
    public void getUntrapped()
    {
        isTrapped = false;
        rb.constraints = RigidbodyConstraints2D.None;
    }
}
