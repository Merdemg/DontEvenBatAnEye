using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    GameObject ghost;

    void Start ()
    {
        ghost = GameObject.FindGameObjectWithTag("Ghost");
        ghost.GetComponent<GhostController>().getTrapped();
	}

    private void OnDestroy()
    {
        ghost.GetComponent<GhostController>().getUntrapped();
    }
}
