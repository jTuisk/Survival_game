using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

using Game.Player.Input;

namespace Game.Player.Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput _playerInput;

        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool Run { get; private set; }
        public bool Jump { get; private set; }

        public bool Interact { get; private set; }
        public bool Tab { get; private set; }
        public bool Shift { get; private set; }

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
        private InputAction _runAction;
        private InputAction _jumpAction;
        private InputAction _InteractAction;
        private InputAction _Tab;
        private InputAction _Shift;

        private void Awake()
        {
            _currentMap = _playerInput.currentActionMap;
            _moveAction = _currentMap.FindAction("Move");
            _lookAction = _currentMap.FindAction("Look");
            _runAction = _currentMap.FindAction("Run");
            _jumpAction = _currentMap.FindAction("Jump");
            _InteractAction = _currentMap.FindAction("Interact");
            _Tab = _currentMap.FindAction("Tab");
            _Shift = _currentMap.FindAction("Shift");

            _moveAction.performed += SetMove;
            _moveAction.canceled += SetMove;

            _lookAction.performed += SetLook;
            _lookAction.canceled += SetLook;

            _runAction.performed += SetRun;
            _runAction.canceled += SetRun;

            _jumpAction.performed += SetJump;
            _jumpAction.canceled += SetJump;

            _InteractAction.performed += SetInteract;
            _InteractAction.canceled += SetInteract;

            _Tab.performed += SetTab;
            _Tab.canceled += SetTab;

            _Shift.performed += SetShift;
            _Shift.canceled += SetShift;
        }
        private void SetMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }

        private void SetLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }

        private void SetRun(InputAction.CallbackContext context)
        {
            Run = context.ReadValueAsButton();
        }

        private void SetJump(InputAction.CallbackContext context)
        {
            Jump = context.ReadValueAsButton();
        }

        private void SetInteract(InputAction.CallbackContext context)
        {
            Interact = context.ReadValueAsButton();
        }

        private void SetTab(InputAction.CallbackContext context)
        {
            Tab = context.ReadValueAsButton();
        }
        private void SetShift(InputAction.CallbackContext context)
        {
            Shift = context.ReadValueAsButton();
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
}