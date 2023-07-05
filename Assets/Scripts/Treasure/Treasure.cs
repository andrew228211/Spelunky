using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public int value;
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.LogError("Da cham");
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.inventory.PickupGold(value);
            Destroy(gameObject);
        }
        else if (player == null)
        {
            if (other.CompareTag("Explosion"))
            {
                Destroy(gameObject);
            }
        }
    }
}
