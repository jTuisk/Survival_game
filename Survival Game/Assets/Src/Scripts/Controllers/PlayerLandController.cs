using UnityEngine;

public class PlayerLandController : MonoBehaviour
{

    private Rigidbody _rigidbody = null;

    public Transform CameraFollow;
    [SerializeField] PlayerLandInput _input;
    [SerializeField] PlayerMovementAnimator _playerAnimator;

    Vector3 _playerMoveInput = Vector3.zero;
    Vector3 _playerLookInput = Vector3.zero;
    Vector3 _previousPlayerLookInput = Vector3.zero;

    float _cameraPitch = 0.0f;
    [SerializeField] float _playerLookInputLerpTime = 0.35f;

    [Header("Movement")]
    [SerializeField] float _movementMultiplier = 30.0f;
    [SerializeField] float _sprintingMultiplier = 1.6f;
    [SerializeField] float _rotationSpeedMultiplier = 180.0f;
    [SerializeField] float _pitchSpeedMultiplier = 180.0f;


    [SerializeField] bool _isRunning = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();    
    }

    private void FixedUpdate()
    {
        _playerLookInput = GetLookInput();
        PlayerLook();
        PitchCamera();

        _playerMoveInput = GetMoveInput();
        PlayerIsRunning();
        PlayerMove();
        HandleAnimation();

        _rigidbody.AddRelativeForce(_playerMoveInput, ForceMode.Force);
    }


    private Vector3 GetMoveInput()
    {
        return new Vector3(_input.MoveInput.x, 0.0f, _input.MoveInput.y);
    }
    private void PlayerIsRunning()
    {
        _isRunning = _input.SprintIsPressed;
    }
    private void PlayerMove()
    {

        if (!_isRunning)
        {
            _playerMoveInput = new Vector3(_playerMoveInput.x * _movementMultiplier,
                                           _playerMoveInput.y,
                                           _playerMoveInput.z * _movementMultiplier);
        }
        else
        {
            _playerMoveInput = new Vector3(_playerMoveInput.x * _movementMultiplier * _sprintingMultiplier,
                                           _playerMoveInput.y,
                                           _playerMoveInput.z * _movementMultiplier * _sprintingMultiplier);
        }
    }

    private Vector3 GetLookInput()
    {
        _previousPlayerLookInput = _playerLookInput;
        _playerLookInput = new Vector3(_input.LookInput.x, _input.LookInput.y, 0.0f);
        return Vector3.Lerp(_playerLookInput, _playerLookInput * Time.deltaTime, _playerLookInputLerpTime);
    }

    private void PlayerLook()
    {
        _rigidbody.rotation = Quaternion.Euler(0.0f, _rigidbody.rotation.eulerAngles.y + (_playerLookInput.x * _rotationSpeedMultiplier), 0.0f);
    }

    private void PitchCamera()
    {
        Vector3 rotationValues = CameraFollow.rotation.eulerAngles;
        _cameraPitch += _playerLookInput.y * _pitchSpeedMultiplier;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -89.9f, 89.9f);

        CameraFollow.rotation = Quaternion.Euler(_cameraPitch, rotationValues.y, rotationValues.z);
    }

    private void HandleAnimation()
    {
        if (_input.MoveIsPressed)
        {
            if (!_isRunning)
            {
                _playerAnimator.ChangeAnimatinState(PlayerAnimationStates.Walk);
            }
            else
            {
                _playerAnimator.ChangeAnimatinState(PlayerAnimationStates.Run);
            }
            _playerAnimator.ChangeAnimationSpeed(_rigidbody.velocity.magnitude / 1.9f);
        }
        else
        {
            _playerAnimator.ChangeAnimatinState(PlayerAnimationStates.Idle);
        }
    }
}
