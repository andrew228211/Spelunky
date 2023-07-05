using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Cotroller))]
public class Player : MonoBehaviour
{
    public Inventory inventory;
    public HealthController healthController;
    public ItemUI itemUI;
    public CameraFollow cam;

    [Header("State")]
    public RunState runState;
    public JumpState jumpState;
    public ClimbState climbState;
    public HangState hangState;
    public AttackState attackState;
    public ThrowState throwState;
    public DieState dieState;
    public EnterDoorState enterDoorState;
    public LookState lookState;

    [Header("Movement")]
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    public float accelerationTime = .1f;
    public float moveSpeed = 64;
    public float climbSpeed;
    public bool isDead=false;

    [HideInInspector] public bool isFacingRight = false;
    [HideInInspector] public float facingDirection = -1;
    //Thiet lap thoi gian roi khoi mat dat
    public float groundeedGracePeriod = 0.1f;
    public float pushBlockSpeed;
    [HideInInspector] public float groundedGraceTimer;
    [HideInInspector] public bool isAttack=true;
    [HideInInspector] public bool hasThrown=true;
    [HideInInspector] public float lookTime = 0;
    [HideInInspector] public float timeBeforelook = 1f;


    [Header("Abilities")]
    public Rope rope;
    public Bomb bomb;

    float gravity;
    [HideInInspector]
    public float maxJumpVelocity;
    [HideInInspector] 
    public float minJumpVelocity;
    [HideInInspector]
    public Vector2 velocity;
    float velocityXSmoothing;
    [HideInInspector] 
    public Vector2 directionalInput;

    public LayerMask edgeLayerMask;
    [HideInInspector] public new SpriteRenderer renderer;
    [HideInInspector] public SpriteAnimator animator;
    [HideInInspector] public Cotroller controller;

    public Exit exitdoor;
    public Health health;
    public StateMachine stateMachine = new StateMachine();

    public GameObject gameOver;
    private void Awake()
    {
        renderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<SpriteAnimator>();
        controller = GetComponent<Cotroller>();
        gameOver = GameObject.Find("GameOver");
        cam = GameObject.FindObjectOfType<CameraFollow>();
    }

    void Start()
    {
        cam.virtualCam.Follow = transform;
        gameOver.SetActive(false);
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        //Van toc toi da vmax=g*t theo phuong thang dung
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //Van toc nho nhat khi tha nut nhay la v*v=vo*v*o+2gt (vo=0)
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        //Khoi tai trang thai bang false
        runState.player = this;
        runState.enabled = false;
        lookState.player = this;
        lookState.enabled = false;
        jumpState.player = this;
        jumpState.enabled = false; 
        climbState.player = this;
        climbState.enabled = false;
        hangState.player = this;
        hangState.enabled = false;
        attackState.player = this;
        attackState.enabled = false;
        enterDoorState.player = this;
        enterDoorState.enabled = false;
        throwState.player = this;
        throwState.enabled = false;
        dieState.player = this;
        dieState.enabled = false;
        stateMachine.ChangeState(runState);
        
    }

    void Update()
    {
        CalculateVelocity();
        Flip();
        //print(velocity);
        controller.Move(velocity * Time.deltaTime);
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        HealthChanged();
    }
    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);
        velocity.y += gravity * Time.deltaTime;
        stateMachine.currentState.ChangePlayerVelocity(ref velocity);
    }
    public void Flip()
    {
        if(directionalInput.x>0 && !isFacingRight)
        {
            FlipCharacter();
        }
        else if (directionalInput.x < 0 && isFacingRight)
        {
            FlipCharacter();
        }
    }
    void FlipCharacter()
    {
        //renderer.flipX = !renderer.flipX;
        Vector3 transformScale = transform.localScale;
        transformScale.x *= -1;
        transform.localScale = transformScale;
        facingDirection *= -1;
        isFacingRight = !isFacingRight;
    }
    public void ThrowRope()
    {
        if (inventory.numberOfRopes <= 0)
        {
            return;
        }
        //print(inventory.numberOfRopes);
        inventory.UseRope();
        Vector3 centerRope = new Vector3(0, 6, 0);
        Rope ropIntance = Instantiate(rope, transform.position+centerRope, Quaternion.identity);
        if(stateMachine.currentState==runState && directionalInput.y < 0)
        {
            ropIntance.placePos = transform.position + (facingDirection * Vector3.right*16);
          
        }
    }

    //Trang thai chet hoan toan khong co input thi ta can them Flag o class State va chuyen no ve True roi chan o PlayerInput la duoc.
    public void Splat()
    {
        //Loi khong reset duoc ve 0
        healthController.playerHealth = 0;
        healthController.UpdateHealth();
        stateMachine.ChangeState(dieState);
    }
    private void HealthChanged()
    {
        healthController.playerHealth = health.currentHealth; 
        healthController.UpdateHealth();
        if (health.currentHealth <= 0)
        {
            stateMachine.ChangeState(dieState);
        }
    }
    public void EnteredDoor(Exit door)
    {
        exitdoor = door;
    }
    public void ExitedDoor(Exit door)
    {
        exitdoor = null;
    }
    public void Use()
    {
        if (exitdoor == null)
        {
            return;
        }
        stateMachine.ChangeState(enterDoorState);
    }
}