using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform player;
    public Transform anchor;
    Quaternion anchorRot;
    float constrainedAngle;
	// Use this for initialization
	void Start ()
    {
        constrainedAngle = transform.rotation.y;

        //anchorRot = this.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {


        //transform.position = new Vector3(player.position.x, 60.0f, player.position.z);
        //transform.LookAt(anchor);
        //
        //Quaternion rot = transform.rotation;
        //Quaternion quat = transform.rotation;
        //quat.y = constrainedAngle;
        //transform.rotation = quat;


        //Vector3 ignoreDim = new Vector3(0, 30, 0);
        //Quaternion rotation = Quaternion.Euler(rotationVector);

    }
}
