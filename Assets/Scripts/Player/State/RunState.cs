using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RunState : State
{
    private Block blocktoPush;
    private Block lastBlockPushed;
    public override void Enter()
    {
        base.Enter();

    }
    public void Update()
    {       
        player.groundedGraceTimer = 0;
        Run();
        UnSteady(); 
        BlocktoPush();
    }
    private void Run()
    {
        player.isAttack = true;
        player.hasThrown = true;
        if (player.directionalInput.x != 0)
        {
            if (player.controller.collisions.left || player.controller.collisions.right)
            {
                player.animator.Play("Push");
            }
            else player.animator.Play("PlayerRun");
            player.animator.fps = 12;
        }
        else if(player.directionalInput.y>=0)
        {
            player.animator.Play("PlayerIdle");
        }
        if (!player.controller.collisions.below)
        {
            player.stateMachine.ChangeState(player.jumpState);
            return;
        }      
        if (player.directionalInput.y > 0 || (player.directionalInput.y<0 && player.controller.collisions.below&&player.controller.collisions.colliderBelow.CompareTag("LadderTop")))
        {       
            player.stateMachine.ChangeState(player.climbState);
        }
        else if (player.directionalInput.y < 0)
        {
            player.stateMachine.ChangeState(player.lookState);
        }
    }
    private void BlocktoPush()
    {
        blocktoPush = null;
        if (player.directionalInput.x < 0 && player.controller.collisions.left && player.controller.collisions.colliderLeft.CompareTag("Block"))
        {
            blocktoPush = player.controller.collisions.colliderLeft.GetComponent<Block>();
        }
        if (player.directionalInput.x > 0 && player.controller.collisions.right && player.controller.collisions.colliderRight.CompareTag("Block"))
        {
            blocktoPush = player.controller.collisions.colliderRight.GetComponent<Block>();
        }
        if (blocktoPush != null)
        {
            blocktoPush.pushSpeed = player.pushBlockSpeed * player.directionalInput.x;
        }
        if (blocktoPush == null && lastBlockPushed != null)
        {
            lastBlockPushed.pushSpeed = 0f;
        }
        lastBlockPushed = blocktoPush;
    }
    void UnSteady()
    {
        //Su dung Raycast de phat hien va cham voi mat dat
        //Tu tam nguoi choi den chan nguoi choi, neu ma ca 2 deu khong cham gi thi no se Unsteady
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position + Vector3.up, Vector2.down, 1.5f, player.controller.collisionMask);
        Debug.DrawRay(player.transform.position + Vector3.up, Vector2.down * 1.5f, Color.magenta);
        Vector3 offsetForward = new Vector3(0.2f * player.facingDirection, 1, 0);
        RaycastHit2D hitForward = Physics2D.Raycast(player.transform.position + offsetForward, Vector2.down, 1.5f, player.controller.collisionMask);
        Debug.DrawRay(player.transform.position + offsetForward, Vector2.down * 1.5f,Color.green);
        
        if (player.controller.collisions.below && hit.collider == null && hitForward.collider == null)
        {
            if (player.directionalInput.y >= 0)
            {
                player.animator.Play("Unsteady");
            }
        }
    }
}