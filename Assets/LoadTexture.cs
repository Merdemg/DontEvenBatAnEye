using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadTexture : MonoBehaviour
{
    public Texture ward;
    public Texture trap;
    public Texture booze;
    public Texture stairs;
    public Texture container;
    public Texture pentagrams;
    public Texture interact;
    public Texture def;

    public static bool isWard, isTrap, isBooze = false;
    public static bool isStairs = false; 
    public static bool isContainer, isPentagram, isInteract = false;

    RawImage img;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<RawImage>();
        img.texture = def;
    }

    // Update is called once per frame
    void Update()
    {
        if(isWard) //Works
        {
            img.texture = ward;
        }
        else if(isTrap) //Works
        {
            img.texture = trap;
        }
        else if(isBooze) //Works
        {
            img.texture = booze;
        }
        else if(isStairs)
        {
            print("STAIRS IMAGE");
            img.texture = stairs;
        }
        else if(isContainer) //Works
        {
            img.texture = container;
        }
        else if(isPentagram)
        {
            img.texture = pentagrams;
        }
        else if(isInteract)
        {
            img.texture = interact;
        }
        else
        {
            img.texture = def;
        }





    }
}
