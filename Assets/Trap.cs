using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    public float delayTimer;
    public GameObject bolt;
	private AudioSource ActivateSound;
	void Start(){
		ActivateSound = GetComponent<AudioSource> ();
	}
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ghost")
        {
            StartCoroutine(TrapGhost());
        }
    }

    IEnumerator TrapGhost()
    {
		ActivateSound.Play ();
        print("Trapped Ghost!");
        SimpleControl.isTrapped = true;
        bolt.SetActive(true);
        yield return new WaitForSeconds(delayTimer);
        print("Trap Destroyed!");
        bolt.SetActive(false);
        SimpleControl.isTrapped = false;
        Destroy(gameObject);
    }

}
