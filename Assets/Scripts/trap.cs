using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap : MonoBehaviour {
    GameObject ghost;


	// Use this for initialization
	void Start () {
        ghost = GameObject.FindGameObjectWithTag("Ghost");
        ghost.GetComponent<GhostController>().getTrapped();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        ghost.GetComponent<GhostController>().getUntrapped();
    }
}
