using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLandInput : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public bool MoveIsPressed { get; private set; }
    public Vector2 LookInput { get; private set; } = Vector2.zero;
    public bool SprintIsPressed { get; private set; }
    public bool JumpIsPressed { get; private set; }
    public bool CrouchIsPressed { get; private set; }
    public bool AttackIsPressed { get; private set; }
    public bool BlockIsPressed { get; private set; }
    public bool PauseIsPressed { get; private set; }

    InputActions _input;

    private void OnEnable()
    {
        _input = new InputActions();
        _input.PlayerLand.Enable();

        _input.PlayerLand.Move.performed += SetMove;
        _input.PlayerLand.Move.canceled += SetMove;

        _input.PlayerLand.Look.performed += SetLook;
        _input.PlayerLand.Look.canceled += SetLook;

        _input.PlayerLand.Sprint.performed += SetSprint;
        _input.PlayerLand.Sprint.canceled += SetSprint;

        _input.PlayerLand.Jump.started += SetJump;
        _input.PlayerLand.Jump.canceled += SetJump;

        _input.PlayerLand.Crouch.performed += SetCrouch;
        _input.PlayerLand.Crouch.canceled += SetCrouch;

        _input.PlayerLand.Attack.started += SetAttack;
        _input.PlayerLand.Attack.performed += SetAttack;
        _input.PlayerLand.Attack.canceled += SetAttack;

        _input.PlayerLand.Block.started += SetBlock;
        _input.PlayerLand.Block.performed += SetBlock;
        _input.PlayerLand.Block.canceled += SetBlock;

        _input.PlayerLand.Pause.started += SetPause;
        _input.PlayerLand.Pause.canceled += SetPause;

    }

    private void OnDisable()
    {
        _input.PlayerLand.Move.performed -= SetMove;
        _input.PlayerLand.Move.canceled -= SetMove;

        _input.PlayerLand.Look.performed -= SetLook;
        _input.PlayerLand.Look.canceled -= SetLook;

        _input.PlayerLand.Sprint.performed -= SetSprint;
        _input.PlayerLand.Sprint.canceled -= SetSprint;

        _input.PlayerLand.Jump.started -= SetJump;
        _input.PlayerLand.Jump.canceled -= SetJump;

        _input.PlayerLand.Crouch.performed -= SetCrouch;
        _input.PlayerLand.Crouch.canceled -= SetCrouch;

        _input.PlayerLand.Attack.started -= SetAttack;
        _input.PlayerLand.Attack.performed -= SetAttack;
        _input.PlayerLand.Attack.canceled -= SetAttack;

        _input.PlayerLand.Block.started -= SetBlock;
        _input.PlayerLand.Block.performed -= SetBlock;
        _input.PlayerLand.Block.canceled -= SetBlock;

        _input.PlayerLand.Pause.started -= SetPause;
        _input.PlayerLand.Pause.canceled -= SetPause;

        _input.PlayerLand.Disable();
    }

    private void SetMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
        MoveIsPressed = !(MoveInput == Vector2.zero);
    }

    private void SetLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    private void SetSprint(InputAction.CallbackContext context)
    {
        SprintIsPressed = context.performed;
    }

    private void SetJump(InputAction.CallbackContext context)
    {
        JumpIsPressed = context.performed;
    }

    private void SetCrouch(InputAction.CallbackContext context)
    {
        CrouchIsPressed = context.performed;
    }

    private void SetAttack(InputAction.CallbackContext context)
    {
        AttackIsPressed = context.performed;
    }

    private void SetBlock(InputAction.CallbackContext context)
    {
        BlockIsPressed = context.performed;
    }

    private void SetPause(InputAction.CallbackContext context)
    {
        PauseIsPressed = context.performed;
    }
}
