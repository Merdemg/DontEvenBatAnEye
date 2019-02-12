using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ReadyUpScript : MonoBehaviour {

	public bool P1Ready = false;
	public bool P2Ready = false;
	public string SceneToLoad = "3dscene";
	public Text Player1Text;
	public Text Player2Text;

	float timeLeft = 3.0f;
	public Text text;
	public GameObject CountdownBox;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.D)) {
			
			P2Ready = true;
			Player2Text.text = "Ready";
		}

		if (Input.GetKeyDown (KeyCode.A)) {
			P1Ready = true;
			Player1Text.text = "Ready";
		}
		if ((P1Ready) && (P2Ready)) {
			CountdownBox.gameObject.SetActive (true);
			timeLeft -= Time.deltaTime;
			text.text = "" + Mathf.Round(timeLeft);
			if(timeLeft < 0)
			{
				SceneManager.LoadScene(SceneToLoad);
			}
		}
	}
}
