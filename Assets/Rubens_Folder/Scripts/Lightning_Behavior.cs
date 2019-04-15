using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning_Behavior : MonoBehaviour
{
    [SerializeField] Animator anim_Light;
    [SerializeField] float speed;
    [SerializeField] private float timer;
    //[SerializeField] GameObject gmObj_lighting;

    private void Awake()
    {
        
        timer = Random.Range(50f, 550f);
    }
    private void FixedUpdate()
    {
        transform.Rotate(0,1,0,Space.World);
        timer -= 1 / (Time.deltaTime /speed);
        if (timer < 0) {
            print("Thunder");
            anim_Light.SetTrigger("Thunder");
            ResetTimer();
        }
    }


    void ResetTimer()
    {
        timer = Random.Range(25f, 150f);
        
    }
}
