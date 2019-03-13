using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{

    public static int playerWardCount = 0;
    public static int playerTrapCount = 0;
    public static int playerBoozeCount = 0;
    public static int playerEvidenceCount = 0;
    public static int playerDoorCount = 0;
    public static int playerPentagramCount = 0;
    public static float playerHauntedbyGhost = 0f; //Round the number later
    public static float playerHasDamagedGhostValue = 0f; //Round the number later

    //float waitTimer = 10f;
    float timeElapsed = 0f;
    public string[] textArray;

    float WARD_CHECK_TIME = 10F;
    float TRAP_CHECK_TIME = 20F;
    float BOOZE_CHECK_TIME = 30F;
    float EVIDENCE_CHECK_TIME = 40f;
    float DOOR_CHECK_TIME = 50f;
    float PENTAGRAM_CHECK_TIME = 60f;
    float pHAUNT_CHECK_TIME = 70f;
    float pDAMAGE_CHECK_TIME = 80F;

    bool isUIDisplayed = false;

    void Start()
    {
        //StartCoroutine(StartTutorial());
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        print(Mathf.Round(timeElapsed));

        if (timeElapsed >= WARD_CHECK_TIME && playerWardCount == 0 && isUIDisplayed == false)
        {
            print("START PLACING WARDS");
        }
        if (timeElapsed >= TRAP_CHECK_TIME && playerTrapCount == 0 && isUIDisplayed == false)
        {
            print("START PLACING TRAPS");
        }
        if (timeElapsed >= EVIDENCE_CHECK_TIME && playerEvidenceCount == 0 && isUIDisplayed == false)
        {
            print("START FINDING EVIDENCE");
        }
        if (timeElapsed >= DOOR_CHECK_TIME && playerDoorCount == 0 && isUIDisplayed == false)
        {
            print("START UNLOCKING ROOMS");
        }
        if (timeElapsed >= PENTAGRAM_CHECK_TIME && playerPentagramCount == 0 && isUIDisplayed == false)
        {
            print("START DEPOWERING THE GHOST");
        }
        if (timeElapsed >= pDAMAGE_CHECK_TIME && playerHasDamagedGhostValue == 0 && isUIDisplayed == false)
        {
            print("ATTACK THE GHOST");
        }
        if (timeElapsed >= pHAUNT_CHECK_TIME && playerHauntedbyGhost > 0 && playerBoozeCount == 0 && isUIDisplayed == false)
        {
            print("DRINK BOOZE, YOUR SANITY IS LOW");
        }
        

    }

    //IEnumerator StartTutorial()
    //{
    //    print(textArray[0]);
    //    yield return new WaitForSeconds(waitTimer);
    //    if(playerTrapCount == 0)
    //    {
    //        displayUI(textArray[1]);
    //        yield return new WaitForSeconds(waitTimer);
    //    }
    //    if (playerWardCount == 0)
    //    {
    //        displayUI(textArray[2]);
    //        yield return new WaitForSeconds(waitTimer);
    //    }
    //    if (playerEvidenceCount == 0)
    //    {
    //        displayUI(textArray[3]);
    //        yield return new WaitForSeconds(waitTimer);
    //    }
    //    if (playerDoorCount == 0)
    //    {
    //        displayUI(textArray[4]);
    //        yield return new WaitForSeconds(waitTimer);
    //    }
    //    if (playerPentagramCount == 0)
    //    {
    //        displayUI(textArray[5]);
    //        yield return new WaitForSeconds(waitTimer);
    //    }
    //    if (playerHasDamagedGhostValue == 0)
    //    {
    //        displayUI(textArray[6]);
    //        yield return new WaitForSeconds(waitTimer);
    //    }
    //    if (playerHauntedbyGhost > 0 && playerBoozeCount == 0)
    //    {
    //        displayUI(textArray[7]);
    //        yield return new WaitForSeconds(waitTimer);
    //    }
    //    print("TUTORIAL COMPELETE");
    //}

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

    void displayUI(string infoText) //Info text is a parameter that will be passed to UI.Text
    {
        print(infoText);
    }

}
