using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigatorAnimations : MonoBehaviour
{
    private Animator anim;
    public static bool isWalking, isUnlocking;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            anim.SetBool("isWalking", true);
            print("is Walking!");
        }
        else
            anim.SetBool("isWalking", false);
        
        if(isUnlocking)
        {
            anim.SetBool("isUnlocking", true);
            print("is Unlocking!");

        }
        else
            anim.SetBool("isUnlocking", false);

    }
}
