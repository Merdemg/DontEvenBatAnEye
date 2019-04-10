using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimController : MonoBehaviour
{
    public Animator anim;
    public static bool isWalk, isHaunt;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalk)
        {
            anim.SetBool("isWalk", true);
        }
        else
            anim.SetBool("isWalk", false);

        if (isHaunt)
        {
            anim.SetBool("isHaunt", true);

        }
        else
            anim.SetBool("isHaunt", false);
    }
}
