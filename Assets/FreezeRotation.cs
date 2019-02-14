using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour
{
    Quaternion rot;

    // Start is called before the first frame update
    void Awake()
    {
        rot = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = rot;
    }
}
