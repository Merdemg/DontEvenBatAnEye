using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleControl : MonoBehaviour
{
    public float movementSpeed = 10;
    public static bool isTrapped = false;

    void Update()
    {
        if(!isTrapped)
        {
            if (Input.GetKey(KeyCode.W))
                transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
            if (Input.GetKey(KeyCode.S))
                transform.Translate(Vector3.back * Time.deltaTime * movementSpeed);
            if (Input.GetKey(KeyCode.D))
                transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
            if (Input.GetKey(KeyCode.A))
                transform.Translate(Vector3.right * Time.deltaTime * -movementSpeed);
        }
    }
}
