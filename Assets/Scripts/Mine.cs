using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {

    [SerializeField] float trapRange = 1.5f;
    [SerializeField] float visibleTime = 3f;
    float trapTime = 4f;
    [SerializeField] GameObject trap;
    [SerializeField] LayerMask mask;
	private AudioSource ActivateSound;

    //[SerializeField] GameObject animHolder;

    GameObject ghost;

    float timer = 0;
    bool isActive = false;

	void Start ()
    {
        ghost = GameObject.FindGameObjectWithTag("Ghost");
		ActivateSound = GetComponent<AudioSource> ();
	}
	
	void Update ()
    {
        if (isActive == false)
        {
            timer += Time.deltaTime;

            if (timer >= visibleTime)
            {
                isActive = true;
                //GetComponent<SpriteRenderer>().enabled = false;
               // animHolder.SetActive(false);

                timer = 0;
            }
        }
        else if (CheckGhostValid() && Vector3.Distance(this.transform.position, ghost.transform.position) < trapRange)
        {
			if (!ActivateSound.isPlaying) {
				ActivateSound.Play ();
			}

            ghost.GetComponent<GhostController>().getTrapped();
            //GetComponent<SpriteRenderer>().enabled = true;
            timer += Time.deltaTime;
            if (timer >= trapTime)
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
