using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Cotroller))]
public class Block : MonoBehaviour
{
    private Vector3 velocity;
    public float pushSpeed; //Toc do dich chuyen 
    public Cotroller controller;
    private void Awake()
    {
        controller = GetComponent<Cotroller>();
    }
    private void Update()
    {
        CaculateVelocity();
        controller.Move(velocity * Time.deltaTime);
        if (controller.collisions.GroundedThisFrame)
        {
            if (controller.collisions.colliderBelow.CompareTag("Enemy"))
            {           
                controller.collisions.colliderBelow.GetComponent<Enemy>().TakeDamage(1);
            }
        }
        if (controller.collisions.below)
        {
            velocity.y = 0;
        }
    }
    private void CaculateVelocity()
    {
        velocity.x = pushSpeed;
        if (!controller.collisions.below)
        {
            Vector3 centerofBlock = transform.position + (Vector3)controller.collider.offset;
            velocity.x = 0;
        }
        velocity.y += ControllerManager.gravity.y * Time.deltaTime;
    }
}
