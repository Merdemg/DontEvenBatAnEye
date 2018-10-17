using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifetime : MonoBehaviour {

    [SerializeField] float lifeTime = 30;
    float timer = 0;

	void Update ()
    {
        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            Destroy(this.gameObject);
        }
	}

    public float getLifeTime()
    {
        return lifeTime;
    }
}
