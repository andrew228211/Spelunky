using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class ClimbState : State
{
    //Bi loi chua fix lai
    //Trang thai leo len ladder hoac rope
    public ContactFilter2D ladderFilter;
    public LayerMask ladderMask;
    //Tim cai gan nhat neu 2 cai gan nhau
    private Collider2D closestCollider;
    public override bool CanEnter()
    {      
        if (player.directionalInput.y == 0)
        {
            return false;
        }
        if (Mathf.Abs(player.directionalInput.y) < Mathf.Abs(player.directionalInput.x))
        {
            return false;
        }
        //Tim thang gan nhat
        closestCollider = FindLappedLadder();
        if (closestCollider == null)
        {
            player.stateMachine.ChangeState(player.lookState);
            return false;
        }
        Vector2 direction = Vector2.up;
        Vector3 position = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, 16, ladderMask);
        Debug.DrawRay(position, direction * 16, Color.magenta);
        if (hit.collider == null)
        {
            return false;
        }
        return true;
    }
    public override void Enter()
    {
        base.Enter();
        player.controller.collisions.fallThrough= true;
        float xPos = closestCollider.transform.position.x;
        player.animator.Play("ClimbRope");
        if (closestCollider.CompareTag("Ladder"))
        {
            xPos += Tile.Width / 2f;
            player.animator.Play("ClimbLadder");
        }
        transform.position = new Vector3(xPos, transform.position.y, 0);
    }
    //Tim thang vi tri gan nhat nguoi choi
    private void Update()
    {
        if (player.directionalInput.y != 0)
        {
            player.animator.fps = 12;
        }
        else
        {
            player.animator.fps = 0;
        }
        if (player.directionalInput.y < 0 && player.controller.collisions.below && !player.controller.collisions.colliderBelow.CompareTag("LadderTop")){
            player.stateMachine.ChangeState(player.runState);
        }
        closestCollider = FindLappedLadder();
        if (closestCollider == null)
        {
            player.stateMachine.ChangeState(player.jumpState);
        }
        else
        {
            player.animator.Play("ClimbRope");
            if (closestCollider.CompareTag("Ladder"))
            {
                
                player.animator.Play("ClimbLadder");
            }
        }
    }
    public override void ChangePlayerVelocity(ref Vector2 velocity)
    {
        velocity.y = player.directionalInput.y * player.climbSpeed;
        velocity.x = 0;
        Vector2 direction = Vector2.down;
        Vector3 position = transform.position + Vector3.up*8;
        if (player.directionalInput.y > 0)
        {
            direction = Vector2.up;
        }
        RaycastHit2D hit = Physics2D.Raycast(position, direction, 16, ladderMask);
        Debug.DrawRay(position, direction * 16, Color.magenta);
        if (hit.collider == null)
        {
            velocity.y = 0;
        }
    }
    private Collider2D FindLappedLadder()
    {
        //Tao 1 danh sach luu tru cac va cham 
        //Vi co the day xep chong len thang => co nhieu loai va cham can tao ra 1 list chua
        List<Collider2D> ladderCollider = new List<Collider2D>();
        player.controller.collider.OverlapCollider(ladderFilter, ladderCollider);
        //khong tim thay tra ve null
        if (ladderCollider.Count <= 0)
        {
            return null;
        }
        float closestDistance = Mathf.Infinity;
        //Tim khoang cach gan nhat de chon thang hay day
        Collider2D closestCollider = null;
        foreach(Collider2D ladder in ladderCollider)
        {
            float xPos = ladder.transform.position.x;
            //Tim thang
            if (ladder.CompareTag("Ladder"))
            {
                xPos += 8;
                //Tinh chieu rong cua thang
            }
            float currentDistance = Mathf.Abs(transform.position.x - xPos);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestCollider = ladder;
            }
            //Tim day 
            if (currentDistance == closestDistance)
            {
                if(ladder==CompareTag("Rope"))
                   {
                    closestCollider = ladder;
                }
            }
        }
        return closestCollider;
    }
}