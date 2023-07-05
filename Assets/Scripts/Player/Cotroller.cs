using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static RaycastController;

public class Cotroller : RaycastController
{
    public CollisionInfo collisions;
    public static Vector2 _gravity = new Vector2(0f, -530f);
    public override void Start()
    {
        base.Start();
    }
    public void Move(Vector2 velocity)
    {
        collisions.GroundedLastFrame = collisions.below;
        collisions.collidedLastFrame = collisions.below || collisions.right || collisions.left || collisions.above;
        UpdateRayCastOrigins();
        collisions.Reset();
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }
        transform.Translate(velocity);
        if (!collisions.GroundedLastFrame && collisions.below)
        {
            collisions.GroundedThisFrame = true;
        }
        //Kiem tra xem co va cham bat ki trong khung hien tai hay khong
        collisions.collidedThisFrame= collisions.below || collisions.right || collisions.left || collisions.above;
    }
    //Va cham theo chieu ngang
    void HorizontalCollisions(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);          
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit)
                {
                    if (hit.collider == collider)
                    {
                        continue;
                    }
                    if (hit.collider.CompareTag("LadderTop"))
                    {
                        continue;
                    }

                    if (hit.distance == 0)
                    {
                        continue;
                    }

                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;

                    if (collisions.left)
                    {
                        collisions.colliderLeft = hit.collider;
                    }
                    if (collisions.right)
                    {
                        collisions.colliderRight = hit.collider;
                    }
                }
            }
        }
    }
    //Va cham theo chieu doc
    void VerticalCollisions(ref Vector2 velocity)
    {
        //Huong Y neu di chuyen xuong = -1 va di chuyen len =1
        float directionY = Mathf.Sign(velocity.y);
        //Tao do dai cac tia
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            //Tao rayOrigin de xem no di chuyen theo huong nao bat dau o goc duoi cung ben trai va tren cung ben trai
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            //Xac dinh vi tri tia goc
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY*rayLength, Color.red);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit)
                {
                    if (hit.collider == collider)
                    {
                        continue;
                    }
                    if (hit.collider.CompareTag("LadderTop"))
                    {
                        if (directionY == 1 || hit.distance == 0)
                        {
                            continue;
                        }
                        if (collisions.fallThrough)
                        {
                            continue;
                        }
                    }
                    velocity.y = (hit.distance - skinWidth) * directionY;
                    rayLength = hit.distance;

                    collisions.below = directionY == -1;
                    collisions.above = directionY == 1;

                    if (collisions.below)
                    {
                        collisions.colliderBelow = hit.collider;
                    }
                    if (collisions.above)
                    {
                        collisions.colliderAbove = hit.collider;
                    }
                }
            }
        }
    }
}
