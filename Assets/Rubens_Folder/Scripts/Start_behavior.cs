using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class Start_behavior : MonoBehaviour
{
	[SerializeField] Slider ins_Bar;

	[SerializeField] Slider ghost_Bar;

    private Player investigator;
    private Player ghost;
    public static int invesID = 0;
    public static int ghostID = 1;


    void Start()
    {
        investigator = ReInput.players.GetPlayer(invesID);
        ghost = ReInput.players.GetPlayer(ghostID);
        print(investigator);
        print(ghost);
    }

    // Update is called once per frame
    void Update()
    {

		if ((ins_Bar.value == 5 && ghost_Bar.value == 5) || Input.GetKeyDown(KeyCode.Return)) {

            SceneManager.LoadScene ("3dKinda");
            //StartCoroutine(LoadYourAsyncScene());
        }
		#region Inspector
		if ((Input.GetKey (KeyCode.Joystick1Button7) || investigator.GetButtonDown("Start")) && ins_Bar.value <= 5f) {
		
			ins_Bar.value += 0.2f;
			print ("adding");
		} 

		if (ins_Bar.value > 5f) {
		
			ins_Bar.value = 5f;
		}

		if (ins_Bar.value < 5f) {

			ins_Bar.value -= Time.deltaTime;
		} 
		#endregion

		#region Ghost
		if ((Input.GetKey (KeyCode.Joystick2Button7) || ghost.GetButtonDown("Start")) && ghost_Bar.value <= 5f) {

			ghost_Bar.value += 0.2f;
			print ("adding");
		} 

		if (ghost_Bar.value > 5f) {

			ghost_Bar.value = 5f;
		}

		if (ghost_Bar.value < 5f) {

			ghost_Bar.value -= Time.deltaTime;
		} 
		#endregion

        
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("3DKinda");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }


}
