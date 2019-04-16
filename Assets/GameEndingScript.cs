using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using UnityEngine.SceneManagement;
public class GameEndingScript : MonoBehaviour
{

    public Image LeftImage;
    public Text LeftText;

    public Text RightText;
    public Image RightImage;

    private Player player;
    private Player ghost;

    public Sprite GhostWinImage;
    public Sprite GhostLoseImage;

    public Sprite PlayerWinImage;
    public Sprite PlayerLoseImage;

    public Canvas canvas;

    public string SceneToLoad = "3dscene";

    // Start is called before the first frame update
    void Start()
    {
        int playerIdentity = 0;
        player = ReInput.players.GetPlayer(playerIdentity);
        int ghostIdentity = 1;
        ghost = ReInput.players.GetPlayer(ghostIdentity);

    }

    // Update is called once per frame
    void Update()
    {
        //Load scene on either one or both players button press, also ask what scene should be loaded
          if (ghost.GetButtonDown("Start")){
                //GhostWin();
            //Invoke("RestartGame", 2);
        }

        if (player.GetButtonDown("Start")){
                //PlayerWin();
                //Debug.Log("Button Pressed");
               // Invoke("RestartGame", 2);
        }
        
    }

    public void GhostWin() {
        canvas.gameObject.SetActive(true);
        LeftImage.sprite = GhostWinImage;
        LeftText.text = "Winner";

        RightImage.sprite = PlayerLoseImage;
        RightText.text = "Loser";
    }

    public void PlayerWin()
    {
        canvas.gameObject.SetActive(true);
        LeftImage.sprite = GhostLoseImage;
        LeftText.text = "Loser";

        RightImage.sprite = PlayerWinImage;
        RightText.text = "Winner";
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneToLoad);


    }
}

