using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigatorAnimations : MonoBehaviour
{
    public Animator anim;
    public static bool isWalking, isUnlocking, isSearching, 
        isFireplace, isStairs, isTrap, isWard;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isWalking)
        {
            anim.SetBool("isWalking", true);
        }
        else
            anim.SetBool("isWalking", false);
        
        if(isUnlocking)
        {
            anim.SetBool("isUnlocking", true);

        }
        else
            anim.SetBool("isUnlocking", false);

        if (isSearching)
        {
            anim.SetBool("isSearching", true);
        }
        else
            anim.SetBool("isSearching", false);


        if (isFireplace)
        {
            anim.SetBool("isFireplace", true);

        }
        else
            anim.SetBool("isFireplace", false);


        if (isStairs)
        {
            anim.SetBool("isStairs", true);
        }
        else
            anim.SetBool("isStairs", false);

        if (isTrap)
        {
            anim.SetBool("isTrap", true);

        }
        else
            anim.SetBool("isTrap", false);

        if (isWard)
        {
            anim.SetBool("isWard", true);

        }
        else
            anim.SetBool("isWard", false);

    }
}
