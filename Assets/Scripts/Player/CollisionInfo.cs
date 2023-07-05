using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Biet chinh xac noi xay ra va cham du o ben duoi nhan vat hay ben tren no tu trai qua phai
public struct CollisionInfo
{
    public bool above, below;
    public bool left, right;
    public Collider2D colliderAbove, colliderBelow;
    public Collider2D colliderLeft, colliderRight;
    public bool GroundedThisFrame;
    public bool GroundedLastFrame;
    public bool collidedThisFrame, collidedLastFrame;
    public bool fallThrough;
    public void Reset()
    {
        above = false;
        below = false;
        left = false;
        right = false;
        colliderAbove = null;
        colliderBelow = null;
        colliderLeft = null;
        colliderRight = null;
        GroundedThisFrame = false;
        collidedThisFrame = true;
    }
}