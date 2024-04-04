using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Class created to control the player movement, animation, and attack.
/// </summary>
public class PlayerController : MonoBehaviour
{
    // Fields for player components and settings
    [SerializeField] private Transform weapon;
    [SerializeField] private PlayerWeapon playerWeapon;
    [SerializeField] private Canvas canvas;

    private PlayerInputActions playerInputActions;
    private Animator animator;
    private float moveSpeed = 5f;
    private float turnSpeed = 10f;
    private int IsWalking = Animator.StringToHash("isWalking");
    public bool isWalking = false;
    private IAgentState<PlayerController> currentState;
    private Player player;

    /// <summary>
    /// Cache the main components in local variables during Awake.
    /// </summary>
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        animator = GetComponentInChildren<Animator>();
        currentState = new IdleState();
        canvas ??= gameObject.GetComponentInChildren<Canvas>();
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ICollectible>(out ICollectible collectible))
        {
            collectible.Accept(player);
        }
    }

    /// <summary>
    /// Update life cycle method. Handles state input and updates.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
        currentState.HandleInput(this);
        currentState.Update(this);
        //HandleWeapon();
        //HandleMovement();
        //HandleAnimation();
    }

    /// <summary>
    /// Transition to a new state.
    /// </summary>
    public void TransitionToState(IAgentState<PlayerController> newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    /// <summary>
    /// Handle the weapon in case of shooting a projectile
    /// </summary>
    public void HandleWeapon()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (!MouseUtil.Instance.IsMouseAvailable()) return;
            Vector3 targetDirection = (MouseUtil.Instance.GetMousePosition() - transform.position).normalized;
            playerWeapon?.TryShoot(targetDirection);
        }
    }


    /// <summary>
    /// Get the input and cast a capule to verify if the path is not blocked. 
    /// If the path is blocked, check if it is possible to move in one of the axis
    /// After moving, call for update the animation.
    /// </summary>
    public void HandleMovement()
    {
        Vector2 input = GetInputNormalized();
        Vector3 direction = new Vector3(input.x, 0f, input.y);
        float size = .15f;
        float height = 1.8f;
        float radius = 0.25f;
        RaycastHit hitInfo;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * height), radius, direction, out hitInfo, size);
        if (!canMove && hitInfo.transform.GetComponent<ICollectible>() != null)
        {
            canMove = true;
        }
        if (!canMove)
        {
            Vector3 directionX = new Vector3(input.x, 0f, 0f).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * height), radius, directionX, size);
            if (canMove)
            {
                direction = directionX;
            }
        }
        if (!canMove)
        {
            Vector3 directionZ = new Vector3(0f, 0f, input.y).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * height), radius, directionZ, size);
            if (canMove)
            {
                direction = directionZ;
            }
        }
        if (canMove)
        {
            transform.position += moveSpeed * Time.deltaTime * direction;
            transform.forward = Vector3.Slerp(transform.forward, direction, turnSpeed * Time.deltaTime);
        }
        isWalking = direction != Vector3.zero;
    }

    /// <summary>
    /// Handle the player animation
    /// </summary>
    public void HandleAnimation()
    {
        animator.SetBool(IsWalking, isWalking);
    }

    /// <summary>
    /// Returns the user input from the new Input Manager
    /// </summary>
    /// <returns>Vector2: the user input</returns>
    private Vector2 GetInput()
    {
        Vector2 input = playerInputActions.Player.Move.ReadValue<Vector2>();
        return input;
    }

    /// <summary>
    /// Return the user input, normalized
    /// </summary> 
    /// <returns>Vector2: the user input normalized</returns>
    private Vector2 GetInputNormalized()
    {
        return GetInput().normalized;
    }
}
