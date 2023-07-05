using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Cotroller))]
public class Bomb : MonoBehaviour
{
    public Explosion explosion;

    public float timeToExplode;
    private Vector3 offset;
    private Vector3 veclocity;

    private Cotroller controller;
    public SpriteAnimator spriteAnimator;

    private void Awake()
    {
        controller = GetComponent<Cotroller>();
        spriteAnimator = GetComponent<SpriteAnimator>();
    }
    private void Start()
    {
        StartCoroutine(DelayeExplosion());
    }
    private void Update()
    {
        CalculateVelocity();
        HandleCollison();
        controller.Move(veclocity * Time.deltaTime);
    }
    public void SetVeclocity(Vector2 veclocity)
    {
        this.veclocity = veclocity;
    }
    private void HandleCollison()
    {
        if(controller.collisions.collidedThisFrame&& !controller.collisions.collidedLastFrame)
        {
            if (controller.collisions.right || controller.collisions.left)
            {
                veclocity.x *= -1f;
            }
            if (controller.collisions.above || controller.collisions.below)
            {
                veclocity.y *= -1f;
            }
            veclocity *= 0.5f;
        }
    }
    private void CalculateVelocity()
    {
        veclocity.y += ControllerManager.gravity.y * Time.deltaTime;
    }
    private IEnumerator DelayeExplosion()
    {
        spriteAnimator.fps = 0;
        //Neu doi tieng thi tru them o sau
        yield return new WaitForSeconds(timeToExplode);
        spriteAnimator.Play("Bomb");
        spriteAnimator.fps = 12;
        yield return new WaitForSeconds(0.1f);
        Explode();
    }
    public void  Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
