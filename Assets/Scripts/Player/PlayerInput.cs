using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    private Player player;
    private Button leftButton, rightButton, upButton, downButton, jumpButton;
    private Button attackButton, bombButton, ropeButton, exitButton;
    private Vector2 directionalInput = Vector2.zero;

    private bool checkExited = true;
    private void Awake()
    {
        player = GetComponent<Player>();
        leftButton = GameObject.Find("LeftButton").GetComponent<Button>();
        rightButton = GameObject.Find("RightButton").GetComponent<Button>();
        upButton = GameObject.Find("TopButton").GetComponent<Button>();
        downButton = GameObject.Find("DownButton").GetComponent<Button>();
        attackButton = GameObject.Find("AttackButton").GetComponent<Button>();
        ropeButton = GameObject.Find("RopeButton").GetComponent<Button>();
        bombButton = GameObject.Find("BombButton").GetComponent<Button>();
        jumpButton = GameObject.Find("JumpButton").GetComponent<Button>();


    }
    private void Start()
    {
        EventTrigger leftButtonEventTrigger = leftButton.GetComponent<EventTrigger>();
        EventTrigger rightButtonEventTrigger = rightButton.GetComponent<EventTrigger>();
        EventTrigger upButtonEventTrigger = upButton.GetComponent<EventTrigger>();
        EventTrigger downButtonEventTrigger = downButton.GetComponent<EventTrigger>();

        AddEventTrigger(leftButtonEventTrigger, EventTriggerType.PointerDown, (data) => { OnLeftButtonClicked(); });
        AddEventTrigger(leftButtonEventTrigger, EventTriggerType.PointerUp, (data) => { OnButtonUp(); });
        AddEventTrigger(rightButtonEventTrigger, EventTriggerType.PointerDown, (data) => { OnRightButtonClicked(); });
        AddEventTrigger(rightButtonEventTrigger, EventTriggerType.PointerUp, (data) => { OnButtonUp(); });
        AddEventTrigger(upButtonEventTrigger, EventTriggerType.PointerDown, (data) => { OnUpButtonClicked(); });
        AddEventTrigger(upButtonEventTrigger, EventTriggerType.PointerUp, (data) => { OnButtonUp(); });
        AddEventTrigger(downButtonEventTrigger, EventTriggerType.PointerDown, (data) => { OnDownButtonClicked(); });
        AddEventTrigger(downButtonEventTrigger, EventTriggerType.PointerUp, (data) => { OnButtonUp(); });

        jumpButton.onClick.AddListener(OnJumpButtonClick);
        ropeButton.onClick.AddListener(OnRopeButtonClick);
        attackButton.onClick.AddListener(OnAttackButtonClick);
        bombButton.onClick.AddListener(OnThrowButtonClick);

    }
    private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, UnityAction<BaseEventData> callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();

        //Dat loai su kien la eventType: PointerDown, pointerUp
        entry.eventID = eventType;
        entry.callback.AddListener(callback);

        //Them vao trigger
        trigger.triggers.Add(entry);
    }
    private void Update()
    {
        player.stateMachine.currentState.OnDirectionInput(directionalInput);
        if (checkExited && player.exitdoor != null)
        {
            checkExited = false;
            exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
        }
        if (player.exitdoor != null)
        {
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(OnUseButtonClick);
        }
    }
    public void OnUseButtonClick()
    {
        player.stateMachine.currentState.OnUseInputDown();
    }
    public void OnButtonUp()
    {
        directionalInput = Vector2.zero;
    }

    public void OnLeftButtonClicked()
    {
        directionalInput = Vector2.left;
    }

    public void OnRightButtonClicked()
    {
        directionalInput = Vector2.right;
    }

    public void OnUpButtonClicked()
    {
        directionalInput = Vector2.up;
    }

    public void OnDownButtonClicked()
    {
        directionalInput = Vector2.down;
    }

    public void OnJumpButtonClick()
    {
        player.stateMachine.currentState.OnJumpInputDown();
    }

    public void OnRopeButtonClick()
    {
        player.stateMachine.currentState.OnRopeInput();
    }

    public void OnAttackButtonClick()
    {
        player.stateMachine.currentState.OnAttackInputDown();
    }

    public void OnThrowButtonClick()
    {
        player.stateMachine.currentState.OnThrowInput();
    }

}


//-----------------Day la choi tren may tinh----------------------


//public class PlayerInput : MonoBehaviour
//{
//    private Player player;
//    public void Start()
//    {
//        player = GetComponent<Player>();
//    }

//    void Update()
//    {

//        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
//        player.stateMachine.currentState.OnDirectionInput(directionalInput);
//        if (Input.GetButtonDown("Jump"))
//        {
//            player.stateMachine.currentState.OnJumpInputDown();
//        }
//        if (Input.GetButtonUp("Jump"))
//        {
//            player.stateMachine.currentState.OnJumpInputUp();
//        }
//        if (Input.GetKeyDown(KeyCode.H))
//        {
//            player.stateMachine.currentState.OnRopeInput();
//        }
//        if (Input.GetKeyDown(KeyCode.J))
//        {
//            player.stateMachine.currentState.OnAttackInputDown();
//        }
//        if (Input.GetKeyDown(KeyCode.K))
//        {
//            player.stateMachine.currentState.OnThrowInput();
//        }
//        if (Input.GetKeyDown(KeyCode.L))
//        {
//            player.stateMachine.currentState.OnUseInputDown();
//        }
//    }
//}
