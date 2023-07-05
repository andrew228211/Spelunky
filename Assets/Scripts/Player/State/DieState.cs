using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DieState : State
{ 
    public override void Enter()
    {      
        player.animator.Play("PlayerDie");
        Instantiate(player.health.bloodParticle, transform.position, Quaternion.identity);
        player.animator.looping = false;
        player.controller.collider.enabled = false;
        
        player.gameOver.SetActive(true);
        
        
    }
    public override void ChangePlayerVelocity(ref Vector2 velocity)
    {
        velocity = Vector2.zero;
    }
}
