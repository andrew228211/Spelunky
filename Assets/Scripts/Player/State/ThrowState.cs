using System.Collections;
using UnityEngine;
public class ThrowState : State
{
    private const int bombFrame = 4; // Frame mà Bomb s? xu?t hi?n trong animation
    private const float bombXOffset = -7f; // Khoang cách ngang tu nguoii choi den vi trí xuat hien cua Bomb
    private const float bombYOffset = 12f; // Khoang cách d?c t? nguoi choi ??n vi trí xuat hien cua Bomb
    private bool isThrowingBomb = false;

    public override bool CanEnter()
    {
        if (player.stateMachine.previosState == player.climbState)
        {
            return false;
        }
        return true;
    }
    private void Update()
    {
        if (!player.hasThrown)
        {
            player.stateMachine.ChangeState(player.runState);
        }
        else
        {
            if (player.inventory.numberOfBombs == 0)
            {
                player.stateMachine.ChangeState(player.runState);
            }
            ThrownBomb();
        }
    }

    private void ThrownBomb()
    {
        if (!isThrowingBomb && player.stateMachine.currentState == this)
        {
            StartCoroutine(DoThrownBomb());
        }
    }

    private IEnumerator DoThrownBomb()
    {
        isThrowingBomb = true;
        player.hasThrown = true;

        // Chay animation
        player.animator.fps = 24;
        player.animator.Play("PlayerThrow");

        // Tính toán vi trí cua Bomb và tao nó
        Vector3 bombPosition = player.transform.position + new Vector3(player.facingDirection * bombXOffset, bombYOffset, 0);
        yield return new WaitUntil(() => player.animator.currentFrame == bombFrame);
        Bomb bomb = Instantiate(player.bomb, bombPosition, Quaternion.identity);

        // Tính toán van toc cua Bomb
        Vector2 bombVelocity = new Vector2(256 * player.facingDirection, 128);
        if (player.directionalInput.y == 1)
        {
            bombVelocity = new Vector2(128 * player.facingDirection, 256);
        }
        else if (player.directionalInput.y == -1)
        {
            if (player.controller.collisions.below)
            {
                bombVelocity = Vector2.zero;
            }
            else
            {
                bombVelocity = new Vector2(128 * player.facingDirection, -256);
            }
        }
        bomb.SetVeclocity(bombVelocity);
        player.inventory.UseBomb();
        yield return new WaitForSeconds(player.animator.GetAnimationLength("PlayerThrow"));
        isThrowingBomb = false;
        player.hasThrown = false;
    }
}
