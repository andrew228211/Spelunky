using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bat : Enemy
{
    public float moveSpeed;
    public int damage;
    private Vector3 velocity;
    private Transform targetToMove;

    public int longRect = 20;
    public int wideRect = 10;    
    public LayerMask layerMask;
    public Vector2  topRightCorner;
    public Vector2 bottomLeftCorner;
    private int flag = 1;
    public override void Awake()
    {
        base.Awake();
    }   
    private void Update()
    {
        if (flag == 1)
        {
            DetectPlayer();
        }
        if (targetToMove == null)
        {
            return;
        }
        if (velocity.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (velocity.x < 0 && isFacingRight)
        {
            Flip();
        }
        velocity = (targetToMove.position - transform.position).normalized * moveSpeed;
        controller.Move(velocity * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other);
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().TakeDamage(damage);
        }
    }
    private void DetectPlayer()
    {
        Vector2 position = transform.position;
        topRightCorner = new Vector2(position.x + 60, position.y - 1);
        bottomLeftCorner = new Vector2(position.x-60, topRightCorner.y - 80);
        Collider2D playerCollider = Physics2D.OverlapArea(topRightCorner, bottomLeftCorner, layerMask);
        if (playerCollider!=null)
        {
            animator.Play("FlyBat");
            targetToMove = playerCollider.transform;
            flag = 0;
        }       
    }
    private void OnDrawGizmosSelected()
    {
        DrawRectange.OnDrawRectange(topRightCorner, bottomLeftCorner);
    }
}
