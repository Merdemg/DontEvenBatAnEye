using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class Intro_behavior : MonoBehaviour
{

    [SerializeField] GameObject ins_Booze;
    [SerializeField] GameObject ins_Trap;
    [SerializeField] GameObject ins_Ward;
    [SerializeField] GameObject ins_Interaction;
    [SerializeField] TextMeshProUGUI ins_Description;
    [SerializeField] GameObject ins_PressStart;
    [SerializeField] GameObject ins_Ready;


    [SerializeField] bool isInsReady;

    [SerializeField] GameObject ghost_Teleport;
    [SerializeField] GameObject ghost_Phase;
    [SerializeField] GameObject ghost_Haunt;
    [SerializeField] GameObject ghost_Interaction;
    [SerializeField] TextMeshProUGUI ghost_Description;
    [SerializeField] GameObject ghost_PressStart;
    [SerializeField] GameObject ghost_Ready;
    [SerializeField] bool isGhostReady;

    private Player investigator;
    private Player ghost;
    public static int invesID = 0;
    public static int ghostID = 1;

    private void Awake()
    {

        ins_Ward.SetActive(false);
        ins_Trap.SetActive(false);
        ins_Booze.SetActive(false);
        ins_Interaction.SetActive(false);
        ins_Ready.SetActive(false);

        ghost_Teleport.SetActive(false);
        ghost_Phase.SetActive(false);
        ghost_Haunt.SetActive(false);
        ghost_Interaction.SetActive(false);
        ghost_Ready.SetActive(false);

        investigator = ReInput.players.GetPlayer(invesID);
        ghost = ReInput.players.GetPlayer(ghostID);
        print(investigator);
        print(ghost);
    }

    void Update()
    {
        if ((isInsReady && isGhostReady) || Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene("Intro_02");
        }

        #region Inspector Inputs

        if (/*Input.GetKeyDown (KeyCode.Joystick1Button0) || */ investigator.GetButtonDown("Interact")) {
            print("INTERACT");
            InspectorInteraction();
        }

        if (/*Input.GetKeyDown(KeyCode.Joystick1Button1) ||*/ investigator.GetButtonDown("Mine")) {
            Trap();
        }

        if (/*Input.GetKeyDown(KeyCode.Joystick1Button3) ||*/ investigator.GetButtonDown("Booze")) {
            Booze();
        }

        if (/*Input.GetKeyDown(KeyCode.Joystick1Button2) ||*/ investigator.GetButtonDown("Trap")) {
			Ward ();
		}

		if (/*Input.GetKeyDown (KeyCode.Joystick1Button7) ||*/ investigator.GetButtonDown("Start")) {
			InspectorReady ();
		}
		#endregion

		#region Ghost Inputs

		if (/*Input.GetKeyDown (KeyCode.Joystick2Button0) ||*/ ghost.GetButtonDown("Interact")) {
			GhostInteraction ();
		}

		if (/*Input.GetKeyDown (KeyCode.Joystick2Button1) ||*/ ghost.GetButtonDown("Phase")) {
			Phase ();
		}

		if (/*Input.GetKeyDown (KeyCode.Joystick2Button2) ||*/ ghost.GetButtonDown("Haunt")) {
			Haunt ();
		}

		if (/*Input.GetKeyDown (KeyCode.Joystick2Button3) ||*/ ghost.GetButtonDown("Fly")) {
			Teleport ();
		}

		if (/*Input.GetKeyDown (KeyCode.Joystick2Button7) ||*/ ghost.GetButtonDown("Start")) {
			GhostReady ();
		}
		#endregion
	}

 #region Inspector Side

	public void InspectorReady (){

		ins_PressStart.SetActive (!ins_PressStart.activeSelf);
		ins_Ready.SetActive (!ins_Ready.activeSelf);
		isInsReady = !isInsReady;
	}
	public void GhostReady (){

		ghost_PressStart.SetActive (!ghost_PressStart.activeSelf);
		ghost_Ready.SetActive (!ghost_Ready.activeSelf);
		isGhostReady = !isGhostReady;
	}

	public void Ward (){

		ins_Ward.SetActive (true);
		ins_Trap.SetActive (false);
		ins_Booze.SetActive (false);
		ins_Interaction.SetActive (false);
        //ins_Description.text = "Ward - Creates a barrier that prevents ghosts from entering the room and consumes the ghost power when in contact."; 
        ins_Description.text = "Ward - Pushes the Entity away and protects the Investigator. Doesn't work through WALLS.";
    }

	public void Trap (){

		ins_Ward.SetActive (false);
		ins_Trap.SetActive (true);
		ins_Booze.SetActive (false);
		ins_Interaction.SetActive (false);
		ins_Description.text = "Trap - Snares the Entity for a small period of time, creating an opportunity to gaze on it."; 


	}

	public void Booze (){

		ins_Ward.SetActive (false);
		ins_Trap.SetActive (false);
		ins_Booze.SetActive (true);
		ins_Interaction.SetActive (false);
		ins_Description.text = "Booze - Regains a percentage of the Investigator's Sanity."; 


	}

	public void InspectorInteraction (){

		ins_Ward.SetActive (false);
		ins_Trap.SetActive (false);
		ins_Booze.SetActive (false);
		ins_Interaction.SetActive (true);
		ins_Description.text = "Interaction - Use it to interect with the objects within the scene. Works on pentagrams, containers, stairs, doors and fireplaces."; 


	}
	#endregion

	#region Ghost side
	public void Teleport (){

		ghost_Teleport.SetActive (true);
		ghost_Haunt.SetActive (false);
		ghost_Phase.SetActive (false);
		ghost_Interaction.SetActive (false);
		ghost_Description.text = "Teleport - Fly between floors freely."; 

	}

	public void Haunt (){

		ghost_Teleport.SetActive (false);
		ghost_Haunt.SetActive (true);
		ghost_Phase.SetActive (false);
		ghost_Interaction.SetActive (false);
		ghost_Description.text = "Haunt - Damage the Investigator's sanity. Works through WALLS."; 

	}

	public void Phase (){

		ghost_Teleport.SetActive (false);
		ghost_Haunt.SetActive (false);
		ghost_Phase.SetActive (true);
		ghost_Interaction.SetActive (false);
		ghost_Description.text = "Phase - Become invulnerable to investigator's GAZE and WARD. Also move faster."; 

	}

	public void GhostInteraction (){

		ghost_Teleport.SetActive (false);
		ghost_Haunt.SetActive (false);
		ghost_Phase.SetActive (false);
		ghost_Interaction.SetActive (true);
		ghost_Description.text = "Interaction - Use it to interect with the objects within the scene. Works on pentagrams and doors."; 

	}
	#endregion

}
