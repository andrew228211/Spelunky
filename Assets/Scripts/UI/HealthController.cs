using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{  
    public int playerHealth;
    private Image[] hearts = new Image[4];
    private void Awake()
    {
        hearts[0] = GameObject.Find("Heart").GetComponent<Image>();
        hearts[1] = GameObject.Find("Heart1").GetComponent<Image>();
        hearts[2] = GameObject.Find("Heart2").GetComponent<Image>();
        hearts[3] = GameObject.Find("Heart3").GetComponent<Image>();
    }
    private void Start()
    {
        UpdateHealth();
    }
    public void UpdateHealth()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth)
            {
                hearts[i].color = Color.red;
            }
            else
            {
                hearts[i].color = Color.black;
            }
        }
    }
}
