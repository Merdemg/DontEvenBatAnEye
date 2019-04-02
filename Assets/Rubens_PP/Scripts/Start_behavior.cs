using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Start_behavior : MonoBehaviour
{
	[SerializeField] Slider ins_Bar;

	[SerializeField] Slider ghost_Bar;

    // Update is called once per frame
    void Update()
    {

		if (ins_Bar.value == 5 && ghost_Bar.value == 5) {
		
			SceneManager.LoadScene ("3dKinda");
		}
		#region Inspector
		if (Input.GetKey (KeyCode.Joystick1Button7) && ins_Bar.value <= 5f) {
		
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
		if (Input.GetKey (KeyCode.Joystick2Button7) && ghost_Bar.value <= 5f) {

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
}
