using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLandInput : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public bool MoveIsPressed { get; private set; }
    public Vector2 LookInput { get; private set; } = Vector2.zero;

    public bool SprintIsPressed { get; private set; }

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
    }

    private void OnDisable()
    {
        _input.PlayerLand.Move.performed -= SetMove;
        _input.PlayerLand.Move.canceled -= SetMove;

        _input.PlayerLand.Look.performed -= SetLook;
        _input.PlayerLand.Look.canceled -= SetLook;

        _input.PlayerLand.Sprint.performed -= SetSprint;
        _input.PlayerLand.Sprint.canceled -= SetSprint;

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
}
