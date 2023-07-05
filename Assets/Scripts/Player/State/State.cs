using UnityEngine;
public abstract class State : MonoBehaviour
{
    [HideInInspector] public Player player;
    public virtual bool CanEnter()
    {
        return true;
    }
    public virtual void Enter()
    {
        enabled = true;
    }
    public virtual void Exit()
    {
        enabled = false;
    }
    public virtual void OnDirectionInput(Vector2 input)
    {
        player.directionalInput = input;
    }
    public virtual void OnJumpInputDown()
    {
        player.velocity.y = player.maxJumpVelocity;

        if (player.directionalInput.y < 0 && player.controller.collisions.colliderBelow != null)
        {
            player.velocity.y = 0;
        }
        if (player.directionalInput.y < 0 && player.controller.collisions.colliderBelow != null && player.controller.collisions.colliderBelow.CompareTag("LadderTop"))
        {
            player.velocity.y = 0;
            player.controller.collisions.fallThrough = true;
        }
        Invoke("ResetFallThrough", .1f);
        player.stateMachine.ChangeState(player.jumpState);

    }
    public void ResetFallThrough()
    {
        player.controller.collisions.fallThrough = false;
    }
    public virtual void OnJumpInputUp()
    {
        if (player.velocity.y > player.minJumpVelocity)
        {
            player.velocity.y = player.minJumpVelocity;
        }
    }
    public virtual void OnThrowInput()
    {
        player.stateMachine.ChangeState(player.throwState);
    }
    public virtual void OnRopeInput()
    {
        player.ThrowRope();
    }
    public virtual void OnUseInputDown()
    {
        player.Use();
    }
    public virtual void OnAttackInputDown()
    {
        player.stateMachine.ChangeState(player.attackState);
    }
    public virtual void ChangePlayerVelocity(ref Vector2 velocity)
    {

    }

}