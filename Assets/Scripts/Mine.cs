using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {

    [SerializeField] float trapRange = 1.5f;
    [SerializeField] float visibleTime = 3f;
    [SerializeField] GameObject trap;
    [SerializeField] LayerMask mask;


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
                timer = 0;
            }
        }
        else if (CheckGhostValid() && Vector3.Distance(this.transform.position, ghost.transform.position) < trapRange)
        {
            ghost.GetComponent<GhostController>().getTrapped();
            GetComponent<SpriteRenderer>().enabled = true;
            timer += Time.deltaTime;
            if (timer >= visibleTime)
            {
                ghost.GetComponent<GhostController>().getUntrapped();
                Destroy(gameObject);
                timer = 0;
            }
        }
	}

    bool CheckGhostValid()
    {
        if (Physics2D.Raycast(this.transform.position, ghost.transform.position - this.transform.position))
        {
            RaycastHit2D temp = Physics2D.Raycast(this.transform.position, ghost.transform.position - this.transform.position, mask);
            if (temp.transform.tag == "Ghost")
            {
                return true;
            }
        }
        return false;
    }



}
