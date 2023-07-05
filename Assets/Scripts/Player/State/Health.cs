using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject bloodParticle; 

    public int maxHealth;
    public int currentHealth; 

    [Header("Invulerability")]
    public SpriteRenderer sprite;
    public float UndeadTime;
    public bool isInvulnerable;
    public Color InvulerabilityColor;

    private void Awake()
    {
        SetHHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
        {
            return;
        }
        Instantiate(bloodParticle, transform.position, Quaternion.identity);
        currentHealth -= damage;
        
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        if (currentHealth > 0)
        {
            StartCoroutine(InvulerabilityTime());
        }
    }
    private void SetHHealth(int value)
    {
        currentHealth = value;
    }
    private IEnumerator InvulerabilityTime()
    {
        isInvulnerable = true;
        Color originalColor = sprite.color;
        float interval = 1 / 10f;
        float time=0;
        while (time < UndeadTime)
        {
            time += interval;
            sprite.color = InvulerabilityColor;
            yield return new WaitForSeconds(interval / 2);
            sprite.color = originalColor;
            yield return new WaitForSeconds(interval / 2);
        }
        isInvulnerable = false;
    }
}
