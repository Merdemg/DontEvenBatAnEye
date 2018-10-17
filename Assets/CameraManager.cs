using UnityEngine;
using System.Collections;
/*
    This script will calculate the position between the 2 players and adjust the camera accordingly. The players will still be able to see each other as well as the 
    objectives.
    
*/


public class CameraManager : MonoBehaviour
{
    public Transform player1, player2;
    public float minSizeY = 12.61f;
    public float maxSizeY = 15.50f;

    void SetCameraPos()
    {
        Vector3 middle = (player1.position + player2.position) * 0.5f;
        Camera camera = GetComponent<Camera>();
        camera.transform.position = new Vector3
        (
            middle.x,
            middle.y,
            camera.transform.position.z
        );
    }

    void SetCameraSize()
    {
        Camera camera = GetComponent<Camera>();
        float width = Mathf.Abs(player1.position.x - player2.position.x) * 0.5f;
        float minSizeX = minSizeY * Screen.width / Screen.height;
        float height = Mathf.Abs(player1.position.y - player2.position.y) * 0.5f;
        float camSizeX = Mathf.Max(width, minSizeX);
        camera.orthographicSize = Mathf.Clamp(Mathf.Max (height, camSizeX* Screen.height / Screen.width, minSizeY), minSizeY, maxSizeY);
    }

    void Update()
    {
        SetCameraPos();
        SetCameraSize();
    }
}