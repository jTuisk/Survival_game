using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    public bool Sprint { get; private set; }
    /*
    public bool MoveIsPressed { get; private set; }
    public bool JumpIsPressed { get; private set; }
    public bool CrouchIsPressed { get; private set; }
    public bool AttackIsPressed { get; private set; }
    public bool BlockIsPressed { get; private set; }
    public bool PauseIsPressed { get; private set; }*/

    private InputActionMap _currentMap;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _sprintAction;

    private void Awake()
    {
        _currentMap = _playerInput.currentActionMap;
        _moveAction = _currentMap.FindAction("Move");
        _lookAction = _currentMap.FindAction("Look");
        _sprintAction = _currentMap.FindAction("Sprint");

        _moveAction.performed += SetMove;
        _moveAction.canceled += SetMove;

        _lookAction.performed += SetLook;
        _lookAction.canceled += SetLook;

        _sprintAction.performed += SetSprint;
        _sprintAction.canceled += SetSprint;
    }
    private void SetMove(InputAction.CallbackContext context)
    {
        Move = context.ReadValue<Vector2>();
    }

    private void SetLook(InputAction.CallbackContext context)
    {
        Look = context.ReadValue<Vector2>();
    }

    private void SetSprint(InputAction.CallbackContext context)
    {
        Sprint = context.ReadValueAsButton();
    }

    private void OnEnable()
    {
        _currentMap.Enable();
    }
    private void OnDisable()
    {
        _currentMap.Disable();
    }
}
