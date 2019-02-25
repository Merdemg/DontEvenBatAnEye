using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public GameObject pCam;
    Transform target;
    int count = 1;
    public static bool changedFloors = false;

    void Update()
    {
        pCam = GameObject.Find("Main Camera (" + count + ")");
        target = pCam.transform;
        this.transform.LookAt(target);
        count = (changedFloors) ? 2 : 1;
    }
}
