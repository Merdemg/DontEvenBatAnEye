using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource mainAud;
    public AudioSource lowSanityAud;
    public AudioSource maxPowerAud;

    // Start is called before the first frame update
    void Start()
    {
        mainAud.volume = 1.0f;
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


        if (LivingController.staticSanity <= 25.0f )
        {
            SetVolume(0.0f, 1.0f, 0.0f);
        }
        else if(GhostController.staticPower >= 220.0f)
        {
            SetVolume(0.0f, 0.0f, 1.0f);

        }
        else if (LivingController.staticSanity > 25f && GhostController.staticPower < 220f)
        {
            SetVolume(1.0f, 0.0f, 0.0f);

        }
        else
        {
            print("DEFAULT CASE");
        }

    }

    void SetVolume(float mainAudioVlume, float lowSanityVolume, float maxGhostPower)
    {
        mainAud.volume = mainAudioVlume;
        lowSanityAud.volume = lowSanityVolume;
        maxPowerAud.volume = maxGhostPower;
    }


}
