using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDirSetter : MonoBehaviour
{
    public static bool playerFacingFront = false;
    public static bool playerFacingBack = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(this.name == "Front")
        {
            playerFacingFront = true;
            print("PLAYER IS FRONT");
        }

        if(this.name == "Back")
        {
            playerFacingBack = true;
            print("PLAYER IS BACK");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerFacingFront = false;
        playerFacingBack = false;
    }

}
