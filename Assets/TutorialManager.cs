using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    GameObject investigator;
    GameObject ghost;
    public static int playerWardCount = 0;
    public static int playerTrapCount = 0;
    public static int playerBoozeCount = 0;
    public static int playerEvidenceCount = 0;
    public static int playerDoorCount = 0;
    public static int playerPentagramCount = 0;
    public static float playerHauntedbyGhost = 0f; //Round the number later
    public static float playerHasDamagedGhostValue = 0f; //Round the number later

    float waitTimer = 10f;

    public string[] textArray;

    // Start is called before the first frame update
    void Start()
    {
        investigator = GameObject.FindGameObjectWithTag("Player");       
        ghost = GameObject.FindGameObjectWithTag("Ghost");
        StartCoroutine(StartTutorial());
    }

    IEnumerator StartTutorial()
    {
        print(textArray[0]);
        yield return new WaitForSeconds(waitTimer);
        if(playerTrapCount == 0)
        {
            print(textArray[1]);
        }
        yield return new WaitForSeconds(waitTimer);
        if (playerWardCount == 0)
        {
            print(textArray[2]);
            print(playerWardCount);
        }
        yield return new WaitForSeconds(waitTimer);
        if (playerEvidenceCount == 0)
        {
            print(textArray[3]);
        }
        yield return new WaitForSeconds(waitTimer);
        if (playerDoorCount == 0)
        {
            print(textArray[4]);
        }
        yield return new WaitForSeconds(waitTimer);
        if (playerPentagramCount == 0)
        {
            print(textArray[5]);
        }
        yield return new WaitForSeconds(waitTimer);
        if (playerHasDamagedGhostValue == 0)
        {
            print(textArray[6]);
        }
        yield return new WaitForSeconds(waitTimer);
        if (playerHauntedbyGhost > 0 && playerBoozeCount == 0)
        {
            print(textArray[7]);
        }
        yield return new WaitForSeconds(waitTimer);
        print("TUTORIAL COMPELETE");
    }

    void SaveStats()
    {
        //Use this function to set player prefs to save stats
    }

    void LoadStats()
    {
        //This function will load the player prefs
    }

    void ResetStats()
    {
        //Reset player preff values to 0
    }

}
