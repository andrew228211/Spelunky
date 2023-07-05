using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public bool isFacingRight = false;
    public float facingDirection = -1;

    public int maxHealth=1;

    public GameObject bloodParticle;
    [HideInInspector] public new SpriteRenderer renderer;
    [HideInInspector] public SpriteAnimator animator;
    [HideInInspector] public Cotroller controller;
    public virtual void Awake()
    {
        renderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<SpriteAnimator>();
        controller = GetComponent<Cotroller>();
    }
    private void OnEnable()
    {
        renderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<SpriteAnimator>();
        controller = GetComponent<Cotroller>();
    }
    public void Flip()
    {
        renderer.flipX = !renderer.flipX;
        facingDirection *= -1;
        isFacingRight = !isFacingRight;
    }
    public virtual void TakeDamage(int damage)
    {
        maxHealth = 0;
        Instantiate(bloodParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }   
}