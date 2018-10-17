using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerProcedure : MonoBehaviour {
    GameObject[] containers;
    [SerializeField] int boozeNum = 5;
    [SerializeField] int evidenceNum = 5;

    // Use this for initialization
    void Start () {
        containers = GameObject.FindGameObjectsWithTag("Container");




        // Shuffle containers
        for (int i = 0; i < 100; i++)
        {
            int a = Random.Range(0, containers.Length);
            int b;
            do
            {
                b = Random.Range(0, containers.Length);
            } while (a == b);

            GameObject temp = containers[a];
            containers[a] = containers[b];
            containers[b] = temp;
        }

        //Add booze
        for (int i = 0; i < boozeNum; i++)
        {
            containers[i].GetComponent<Container>().getBooze();
        }

        // Shuffle again
        for (int i = 0; i < 100; i++)
        {
            int a = Random.Range(0, containers.Length);
            int b;
            do
            {
                b = Random.Range(0, containers.Length);
            } while (a == b);

            GameObject temp = containers[a];
            containers[a] = containers[b];
            containers[b] = temp;
        }
        // Add evidence, a container can have both booze and evidence
        for (int i = 0; i < evidenceNum; i++)
        {
            containers[i].GetComponent<Container>().getEvidence();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
