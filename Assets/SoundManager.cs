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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GhostController.power = 230.0f;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GhostController.power = 100.0f;
        }



        if (LivingController.sanity <= 25.0f )
        {
            print("LOW SANITY");
            mainAud.volume = 0.0f;
            lowSanityAud.volume = 1.0f;
            maxPowerAud.volume = 0.0f;
        }
        else if(GhostController.power >= 220.0f)
        {
            print("GHOST TIME");
            mainAud.volume = 0.0f;
            lowSanityAud.volume = 0.0f;
            maxPowerAud.volume = 1.0f;
        }
        else if (LivingController.sanity > 25f && GhostController.power < 220f)
        {
            print("MAIN BGM");
            mainAud.volume = 1.0f;
            lowSanityAud.volume = 0.0f;
            maxPowerAud.volume = 0.0f;
        }
        else
        {
            print("DEFAULT CASE");
        }

    }
}
