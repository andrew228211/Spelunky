using System.Collections;
using UnityEngine;

public class AttackState : State
{
    public LayerMask enemylayerMask;   

    public Transform attackPoint;
    public float attackRange;

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
        if (!player.isAttack)
        {
            player.stateMachine.ChangeState(player.runState);
        }
        else
        {
            Attack();
        }
    }
    private bool isAttacking = false;

    public void Attack()
    {
        if (!isAttacking)
        {
            StartCoroutine(DoAttack());
        }
    }

    private IEnumerator DoAttack()
    {
        isAttacking = true;
        player.isAttack = true;
        player.animator.fps = 24;
        player.animator.Play("Attack");     
        yield return new WaitForSeconds(player.animator.GetAnimationLength("Attack"));
        //Vector3 offsetForward = new Vector3(transform.position.x, transform.position.y + 8, transform.position.z);
        //RaycastHit2D hitForward = Physics2D.Raycast(offsetForward, Vector2.right * player.facingDirection * 23, 23, enemylayerMask);
        //Debug.DrawRay(offsetForward, Vector2.right * player.facingDirection * 23, Color.red);
        Collider2D[] hit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemylayerMask);
        foreach(Collider2D enemy in hit)
        {          
            enemy.GetComponent<Enemy>().TakeDamage(1);
        }
        player.animator.fps = 12;
        player.isAttack = false;
        isAttacking = false;      
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}