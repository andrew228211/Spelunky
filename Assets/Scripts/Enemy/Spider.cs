using UnityEngine;
public class Spider:Enemy
{
    public float minJumpWaitTime;
    public float maxJumpWaitTime;
    public Vector2 jumpVelocity;
    public int damage;

    public float targetDetectionDis;

    public LayerMask targetDetectionMask;

    public LayerMask colliderToHangMask;

    public Collider2D colliderToHang;

    private Vector2 veclocity;

    private Transform targetToMove;

    private bool landed;
    private float idleDuration;
    public override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {     
        float rayLength = 24f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, rayLength, colliderToHangMask);
        Debug.DrawRay(transform.position, Vector2.up * rayLength, Color.blue);
        colliderToHang = hit.collider;
    }
    private void Update()
    {
        if (colliderToHang == null &&!landed)
        {
            DetectTarget();
        }
        if (targetToMove == null)
        {
            DetectTargetHanging();
            return;
        }  

        veclocity.y += ControllerManager.gravity.y*Time.deltaTime;
        if (controller.collisions.above)
        {
            veclocity.y = 0;
        }
        if (controller.collisions.left || controller.collisions.right)
        {
            veclocity *= -0.25f;
        }
        if (controller.collisions.GroundedThisFrame)
        {
            idleDuration = Random.Range(minJumpWaitTime, maxJumpWaitTime);
            landed = true;
        }
        controller.Move(veclocity * Time.deltaTime);

        idleDuration -= Time.deltaTime;

        if (!landed)
        {
            return;
        }
        JumpToTarget();
    }
    private void DetectTarget()
    {
        float rad = 4f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, rad, targetDetectionMask);
        foreach (Collider2D collider in colliders)
        {
            targetToMove = collider.transform;
            break;
        }
        animator.fps = 24;
        animator.Play("Flip");
    }
    private void DetectTargetHanging()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, targetDetectionDis, targetDetectionMask);
        Debug.DrawRay(transform.position, Vector2.down * targetDetectionDis, Color.green);
        if (hit.collider != null)
        {
            targetToMove = hit.transform;
            animator.fps = 24;
            animator.Play("Flip");
        }
    }
    private void JumpToTarget()
    {
        if (!controller.collisions.below)
        {
            if (veclocity.y > 0)
            {
                animator.fps = 12;
                animator.Play("Jump");
            }
            else
            {
                animator.fps = 12;
                animator.Play("Fall");
            }
        }
        else
        {
            veclocity = Vector2.zero;
            animator.fps = 12;
            animator.Play("Idle");
            if (idleDuration <= 0f)
            {
                DoJump();
            }
        }
    }
    void DoJump()
    {
        float sign = Mathf.Sign(targetToMove.position.x - transform.position.x);
        veclocity = new Vector3(jumpVelocity.x * sign, jumpVelocity.y);
    }

}

