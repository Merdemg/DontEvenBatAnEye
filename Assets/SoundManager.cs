using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource mainAud;
    public AudioSource invAud;
    public AudioSource ghostAud;

    GhostController ghost;

    // Start is called before the first frame update
    void Start()
    {
        if (mainAud)
        {
            mainAud.volume = 1.0f;
        }

        ghost = FindObjectOfType<GhostController>();
    }


    // Update is called once per frame
    void Update()
    {
        //DEBUG ONLY
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("GHOST OFF");
            GhostController.staticPower = 230.0f;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            print("GHOST ON");
            GhostController.staticPower = 100.0f;
        }

        float sanInvAdvantage = LivingController.staticSanity / 2.0f;   /// 0 - 50
        float powerInvAdvantage = (4.0f - ghost.getPowerLevel()) * 12.5f;   /// 0 - 50
        float invAdvantage = (sanInvAdvantage + powerInvAdvantage) / 100f;

        SetVolume(invAdvantage);

    }

    void SetVolume(float invAdv)
    {
        //mainAud.volume = mainAudioVlume;
        invAud.volume = invAdv;
        ghostAud.volume = 1.0f - invAdv;
    }


}
