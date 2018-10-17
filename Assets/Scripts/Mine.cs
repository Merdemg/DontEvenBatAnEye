using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {

    [SerializeField] float trapRange = 1.5f;
    [SerializeField] float visibleTime = 3f;
    [SerializeField] GameObject trap;

    GameObject ghost;

    float timer = 0;
    bool isActive = false;

	void Start ()
    {
        ghost = GameObject.FindGameObjectWithTag("Ghost");
	}
	
	void Update ()
    {
        if (isActive == false)
        {
            timer += Time.deltaTime;

            if (timer >= visibleTime)
            {
                isActive = true;
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else if (Vector3.Distance(this.transform.position, ghost.transform.position) < trapRange)
        {
            Instantiate(trap, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);

        }
	}
}
