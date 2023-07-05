using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : State
{
    [HideInInspector] public RaycastHit2D lastEdge;
    public override void Enter()
    {
        base.Enter();
    }
    public override void OnDirectionInput(Vector2 input)
    {
        base.OnDirectionInput(input);
        if (player.directionalInput.y > 0)
        {
            player.stateMachine.ChangeState(player.climbState);
        }
    }
    public override void OnJumpInputDown()
    {
        if (player.groundedGraceTimer > player.groundeedGracePeriod)
        {
            return;
        }
        base.OnJumpInputDown();
    }
   
    private void Update()
    {
        //Khi chuyen sang trang thai DieState thi Animation van tiep tuc chay trang thai Jump den khi het thi thoi
        //De khac phuc can them co isDead  o class Player cho biet Player da chet va chan trang thai Jump
        if (player.isDead)
        {
            player.Splat();
            return;
        }
        CheckCollison();   
        holdTheGround();
        player.groundedGraceTimer += Time.deltaTime;       
        player.animator.Play("PlayerJump");

    }
    private void holdTheGround()
    {
        Vector2 direction = Vector2.right * player.facingDirection;
        const float yPos = 10f;
        const float rayLength = 9f;
        //Tao 1 raycast kiem tra luc nhay len xem co cham gi ben trai hoac ben phai khong
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up * yPos, direction, rayLength, player.edgeLayerMask);
        Debug.DrawRay(transform.position + Vector3.up * yPos, direction * rayLength, Color.red);
        //Di chuyen sang trai hoac sang phai tim tuong gan nhat
        bool movingOnTheLeft = player.controller.collisions.left && player.directionalInput.x < 0 && !player.isFacingRight;
        bool movingOnTheRight= player.controller.collisions.right && player.directionalInput.x > 0 && player.isFacingRight;
        if((movingOnTheLeft || movingOnTheRight) && player.velocity.y < 0 && hit.collider != null) {
            // Neu co gang tay
            if (player.inventory.hasGlove)
            {
                //Lay tat ca moi thu
                player.hangState.colliderToHang = hit.collider;
                player.hangState.useGlove = true;
                player.stateMachine.ChangeState(player.hangState);
            }
            
            else if (lastEdge.collider == null)
            {
                //Chi lay goc canh
                player.hangState.colliderToHang = hit.collider;
                player.stateMachine.ChangeState(player.hangState);
            }
        }
        lastEdge = hit;
    }
    private void CheckCollison()
    {
        if (player.controller.collisions.GroundedThisFrame)
        {
            if (player.controller.collisions.colliderBelow.CompareTag("Enemy")){
                player.controller.collisions.colliderBelow.GetComponent<Enemy>().TakeDamage(1);
            }
            else if (player.controller.collisions.colliderBelow.CompareTag("Spike")){
     
                player.isDead = true;          
                //player.health.currentHealth = 0;
            }
            else
            {
                player.stateMachine.ChangeState(player.runState);
            }
        }
    }
}
