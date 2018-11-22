using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
public class Movement : MonoBehaviour {

    [Header("Rewired Player Index")]
    private Player player;
    public static int playerId = 0;

    [SerializeField] float moveSpeed = 1f;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    // Use this for initialization
    void Start () {
		
	}
	

	// Update is called once per frame
	void Update ()
    {
        if ((player.GetAxis("Horizontal") > 0.1f || player.GetAxis("Horizontal") < -0.1f
             || player.GetAxis("Vertical") > 0.1f || player.GetAxis("Vertical") < -0.1f))
        {
            float angle = Mathf.Atan2(player.GetAxis("Horizontal"), -player.GetAxis("Vertical")) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            Vector2 temp = new Vector2(player.GetAxis("Horizontal"), player.GetAxis("Vertical"));
            temp.Normalize();
            temp *= moveSpeed;
            GetComponent<Rigidbody2D>().AddForce(temp);
        }

    }
}
