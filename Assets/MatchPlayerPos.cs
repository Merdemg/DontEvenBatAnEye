using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPlayerPos : MonoBehaviour {
    public Transform player;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = player.transform.position;

	}
}
