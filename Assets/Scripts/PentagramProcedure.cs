using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagramProcedure : MonoBehaviour {
    GameObject[] pentagrams;
    [SerializeField] const int pentagramsToDestroy = 5;


	// Use this for initialization
	void Start () {
        pentagrams = GameObject.FindGameObjectsWithTag("Pentagram");
        // Debug.Log("pentagrams:" +pentagrams.Length);


        for (int i = 0; i < 20; i++)
        {
            int a = Random.Range(0, pentagrams.Length);
            int b;
            do
            {
                b = Random.Range(0, pentagrams.Length);
            } while (a == b);

            GameObject temp = pentagrams[a];
            pentagrams[a] = pentagrams[b];
            pentagrams[b] = temp;

        }



        for (int i = 0; i < pentagramsToDestroy; i++)
        {
            Destroy(pentagrams[i]);
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
