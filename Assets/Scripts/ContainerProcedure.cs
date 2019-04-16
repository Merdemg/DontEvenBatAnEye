using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerProcedure : MonoBehaviour {

    GameObject[] containers;
    [SerializeField] int boozeNum = 5;
    [SerializeField] int evidenceNum = 3;
    public Container[] level1, level2;
    void Start () {
        containers = GameObject.FindGameObjectsWithTag("Container");
        level1 = GameObject.Find("Floor1").GetComponentsInChildren<Container>();
        level2 = GameObject.Find("Floor2").GetComponentsInChildren<Container>();

        //Booze Loop
        for (int i = 0; i < 50; i++)
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

        for (int i = 0; i < boozeNum; i++)
        {
            containers[i].GetComponent<Container>().getBooze();
        }

        //Floor-Evidence Loop
        for (int m = 1; m <= 2; m++)
        {
            if (m == 1)
            {
                for (int i = 0; i < 50; i++)
                {
                    int a = Random.Range(0, level1.Length);
                    int b;
                    do
                    {
                        b = Random.Range(0, level1.Length);
                    } while (a == b);

                    Container temp = level1[a];
                    level1[a] = level1[b];
                    level1[b] = temp;
                }

                for (int i = 0; i < evidenceNum; i++)
                {
                    level1[i].GetComponent<Container>().getEvidence();
                }
            }
            if(m==2)
            {
                for (int i = 0; i < 50; i++)
                {
                    int a = Random.Range(0, level2.Length);
                    int b;
                    do
                    {
                        b = Random.Range(0, level2.Length);
                    } while (a == b);

                    Container temp = level2[a];
                    level2[a] = level2[b];
                    level2[b] = temp;
                }

                for (int i = 0; i < evidenceNum; i++)
                {
                    level2[i].GetComponent<Container>().getEvidence();
                }
            }
        }





    }
}
