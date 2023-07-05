using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HangState : State
{
    public bool useGlove;
    public Collider2D colliderToHang;
    public override bool CanEnter()
    {
        if (colliderToHang == null)
        {
            return false;
        }
        return true;
    }
    public override void Enter()
    {
        base.Enter();
        Vector2 hangPosition = new Vector2(transform.position.x, colliderToHang.transform.position.y + 6);
        if (player.isFacingRight)
        {
            if (colliderToHang.transform.position.x < player.transform.position.x)
            {
                player.Flip();
            }
        }
        else
        {
            if (colliderToHang.transform.position.x > player.transform.position.x)
            {
                player.Flip();
            }
        }
        if (useGlove)
        {
            hangPosition.y = transform.position.y;
        }
        transform.position = new Vector2(hangPosition.x, hangPosition.y);
        player.animator.Play("Hang");
    }
    private void Update()
    {
        if (colliderToHang == null)
        {
            player.stateMachine.ChangeState(player.jumpState);
            return;
        }
        if (player.directionalInput.y == 0)
        {
            player.animator.Play("Hang");
        }
    }
    public override void OnDirectionInput(Vector2 input)
    {
        input.x = 0;
        base.OnDirectionInput(input);
    }
    public override void ChangePlayerVelocity(ref Vector2 velocity)
    {
        velocity = Vector2.zero;
    }
}
