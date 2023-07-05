using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookState : State
{
    public override bool CanEnter()
    {
        if (player.controller.collisions.below==false)
        {
            return false;
        }
        return true;
    }
    public override void Enter()
    {    
        base.Enter();
    }
    private void Update()
    {       

        if (player.directionalInput.x != 0)
        {
            player.stateMachine.ChangeState(player.runState);
            return;
        }
        if (player.directionalInput.y != 0)
        {
            player.lookTime += Time.deltaTime;
            if (player.directionalInput.y > 0)
            {
                player.animator.Play("PlayerLookUp");

            }
            else
            {
                player.animator.Play("PlayerLookDown");
            }
            if (player.lookTime > player.timeBeforelook)
            {
                float offset = 32f * Mathf.Sign(player.directionalInput.y);
                player.cam.SetPosition(offset);
                return;
            }
        }
        else
        {
            player.lookTime = 0;
            player.cam.SetPosition(0);
            player.stateMachine.ChangeState(player.runState);
        }
    }
}
