using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    public Button btnExit;  
    private void OnTriggerEnter2D(Collider2D other)
    {       
        Player player = other.GetComponent<Player>();

        if (player != null)
        {         
            btnExit.gameObject.SetActive(true);           
            player.EnteredDoor(this);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {        
            btnExit.gameObject.SetActive(false);
            player.ExitedDoor(this);
        }
    }
}
