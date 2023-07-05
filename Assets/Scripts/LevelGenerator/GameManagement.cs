using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    // Start is called before the first frame update
    public Player player;
    public CameraFollow playerCamera;
    public void SpawnPlayer(Vector3 pos)
    {
        Player playerInstance = Instantiate(player, pos + new Vector3(8, 8, 0), Quaternion.identity);
        CameraFollow cameraFollow= Instantiate(playerCamera, pos + new Vector3(8, 8, 0), Quaternion.identity);
       
    }
}
