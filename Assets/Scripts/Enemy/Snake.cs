using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
[RequireComponent(typeof(Cotroller))]
public class Snake : Enemy
{
    [Header("Movement")]
    public float moveSpeed=64f;
    public int damage=1;

    private float minX, maxX;
    public float distance;

    public LayerMask layerMask;

    [HideInInspector]
    public Vector3 velocity;
    public override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        maxX = transform.position.x + (distance / 2);
        minX = maxX - distance;
    }
    private void Reset()
    {
        moveSpeed = 16f;
        damage = 1;
    }
    private void Update()
    {
        CheckCollision();
        CaculateVelocity();
        if (velocity.x != 0)
        {
            animator.Play("RunSnake");
        }
    }
    void CaculateVelocity()
    {
        
        velocity.x = moveSpeed*facingDirection ;
        if (controller.collisions.below)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y += ControllerManager.gravity.y * Time.deltaTime;
        }
        controller.Move(velocity*Time.deltaTime);
    }
    private void CheckCollision()
    {     
        //Doan code nay kiem tra phia duoi co rong hay khong
        Vector3 offsetBelow = new Vector3(4f * facingDirection, 1, 0);
        RaycastHit2D hitBelow = Physics2D.Raycast(transform.position + offsetBelow, Vector2.down, 8f, controller.collisionMask);
        Debug.DrawRay(transform.position + offsetBelow, Vector2.down * 8f, Color.green);    
        if(controller.collisions.below && hitBelow.collider == null)
        {
            Flip();
        }

        if (facingDirection == -1)
        {
            if(!controller.collisions.below && hitBelow.collider!=null && transform.position.x < minX)
            {          
                Flip();
            }
        }
        else
        {
            if (!controller.collisions.below&&hitBelow.collider != null && transform.position.x >maxX )
            {              
                Flip();
            }
        }
        if (controller.collisions.right )
        {
            if (controller.collisions.colliderRight.tag == "Player")
            {
                Attack(controller.collisions.colliderRight);               
            }
            Flip();
        }
        if (controller.collisions.left)
        {
            if (controller.collisions.colliderLeft.tag == "Player")
            {
                Attack(controller.collisions.colliderLeft);              
            }
            Flip();
        }


    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
    private void Attack(Collider2D colliderAttack)
    {
        animator.Play("AttackSnake");
        colliderAttack.GetComponent<Health>().TakeDamage(damage);
    }
}
