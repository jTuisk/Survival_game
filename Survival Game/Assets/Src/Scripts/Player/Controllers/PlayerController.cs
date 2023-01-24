using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool use;

    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;
    private InputManager _inputManager;

    [SerializeField]
    private PlanetGravityAttractor _attractor;

    [SerializeField]
    private Animator _animator;
    private bool _hasAnimator;
    [SerializeField]
    private float _animatorBlendSpeed = 1f;
    private int _xVelocityHash;
    private int _zVelocityHash;

    [Header("Movement")]
    [SerializeField, ReadOnly]
    private Vector3 _playerMoveInput = Vector3.zero;

    [SerializeField, ReadOnly]
    private float _currentSpeed = 0f;
    [SerializeField, Range(0.1f, 100f)]
    private float _walkSpeed = 5.4f;
    [SerializeField, Range(0.1f, 100f)]
    private float _sprintSpeed = 9.8f;

    private Vector3 _moveDirection;

    [SerializeField, Range(0.1f, 100f)]
    private float _movementMultiplier = 10.0f;


    [Header("Ground check")]
    [SerializeField]
    private bool _playerIsGrounded = false;
    [SerializeField, Range(0.0f, 2f)]
    private float _groundCheckRadiusMultiplier = 1f;
    [SerializeField, Range(-1f, 1f)]
    private float _groundCheckDistance = 0.05f;
    RaycastHit _groundCheckHit = new RaycastHit();

    [Header("Gravity")]
    [SerializeField]
    private bool _useGravity = true;
    [SerializeField]
    private bool _autoOrient = true;
    [SerializeField, Range(0.0f, 1000f)]
    private float _oirientSlerpSpeed = 10f;

    //outterGravityForce
    //innerGravityForce


    [Header("Camera")]
    [SerializeField]
    private Transform _firstPersonCamera;
    [SerializeField]
    private Transform _firstPersonCameraPosition;

    [SerializeField]
    private float _xRotationUpperLimit = -40f;
    [SerializeField]
    private float _xRotationLowerLimit = 80f;
    private float _xRotation;

    [SerializeField]
    private float _mouseSensitivity = 100f;

    private void Awake()
    {
        HideCursor.ShowCurors(false, true);

        _hasAnimator = _animator != null ? true : TryGetComponent<Animator>(out _animator);
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _inputManager = GetComponent<InputManager>();

        _xVelocityHash = Animator.StringToHash("X_Velocity");
        _zVelocityHash = Animator.StringToHash("Z_Velocity");
    }

    private void FixedUpdate()
    {
        _moveDirection = GetMoveDirection();
        _playerMoveInput = GetMoveInput();
        _playerIsGrounded = PlayerIsGrounded();
        //_playerMoveInput.y = PlayerGravity();
        _playerMoveInput = PlayerGravity();

        //PlayerGravity();
        PlayerMove();
        Animate();

        _rigidbody.AddRelativeForce(_playerMoveInput, ForceMode.Force);
    }

    private void LateUpdate()
    {
        CameraMovements();
    }

    private Vector3 GetMoveDirection()
    {
        return new Vector3(_inputManager.Move.x, 0.0f, _inputManager.Move.y).normalized;
    }

    private Vector3 GetMoveInput()
    {
        return new Vector3(_inputManager.Move.x, 0.0f, _inputManager.Move.y);
    }

    /*
    private void PlayerMove()
    {
        Debug.Log($"input: {_playerMoveInput}, multi: {_movementMultiplier}, dir: {transform.TransformDirection(_moveDirection)}, input+dir: {_playerMoveInput+transform.TransformDirection(_moveDirection)}");
        //_playerMoveInput += _moveDirection;
        _playerMoveInput += transform.TransformDirection(_moveDirection);
        _playerMoveInput *= _movementMultiplier;

    }
    */
    private void PlayerMove()
    {
        _playerMoveInput = new Vector3(_playerMoveInput.x * _movementMultiplier, _playerMoveInput.y, _playerMoveInput.z * _movementMultiplier);
        _playerMoveInput += transform.TransformDirection(_moveDirection) * _movementMultiplier;
    }


    private Vector3 PlayerGravity()
    {
        if (!_useGravity || _playerIsGrounded)
            return _playerMoveInput;


        float maxGravityFall = _attractor.Gravity;
        Vector3 planetOrigin = _attractor.transform.position;
        Vector3 gravityUp = (transform.position - planetOrigin).normalized;
        int layerMask = 1 << 3;


        Debug.DrawRay(transform.position, -gravityUp * Vector3.Distance(transform.position, planetOrigin), Color.red);

        RaycastHit distanceHit;

        if (Physics.Raycast(transform.position, -gravityUp, out distanceHit, Vector3.Distance(transform.position, planetOrigin), layerMask))
        {
            Debug.DrawRay(transform.position, -gravityUp * distanceHit.distance, Color.yellow);
        }

        float gravityForce = _attractor.GetCurrentGravityForce(distanceHit.distance);

        Debug.Log($"gravityForce: {gravityForce} at distance: {distanceHit.distance}, -dir = {-gravityUp * gravityForce}, dir: {gravityUp * gravityForce} ");
        //_rigidbody.AddRelativeForce(transform.TransformDirection(gravityUp * gravityForce), ForceMode.Force);
        //_playerMoveInput += transform.TransformDirection(gravityUp * gravityForce);

        if (_autoOrient)
            RotatePlayerToGravityOrigin(gravityUp);

        return _playerMoveInput;
    }

    /*private void PlayerGravity()
    {
        if (!_useGravity || _playerIsGrounded)
            return;

        float maxGravityFall = _attractor.Gravity;
        Vector3 planetOrigin = _attractor.transform.position;
        Vector3 gravityUp = (transform.position - planetOrigin).normalized;
        int layerMask = 1 << 3;


        Debug.DrawRay(transform.position, -gravityUp * Vector3.Distance(transform.position, planetOrigin), Color.red);

        RaycastHit distanceHit;

        if (Physics.Raycast(transform.position, -gravityUp, out distanceHit, Vector3.Distance(transform.position, planetOrigin), layerMask))
        {
            Debug.DrawRay(transform.position, -gravityUp * distanceHit.distance, Color.yellow);
        }

        float gravityForce = _attractor.GetCurrentGravityForce(distanceHit.distance);

        Debug.Log($"gravityForce: {gravityForce} at distance: {distanceHit.distance}, -dir = {-gravityUp * gravityForce}, dir: {gravityUp * gravityForce} ");
        _rigidbody.AddRelativeForce(transform.TransformDirection(gravityUp * gravityForce) , ForceMode.Force);

        RotatePlayerToGravityOrigin(-gravityUp);
    }*/

    private void RotatePlayerToGravityOrigin(Vector3 gravityUp)
    {
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, gravityUp) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _oirientSlerpSpeed * Time.fixedDeltaTime);
    }

    private bool PlayerIsGrounded()
    {
        float sphereCastRadius = _capsuleCollider.radius * _groundCheckRadiusMultiplier;
        float sphereCastTravelDistance = _capsuleCollider.bounds.extents.y - sphereCastRadius + _groundCheckDistance;
        return Physics.SphereCast(_rigidbody.position, sphereCastRadius, Vector3.down, out _groundCheckHit, sphereCastTravelDistance);
    }

    private void Animate()
    {
        if (_hasAnimator)
        {
            _animator.SetFloat(_xVelocityHash, _rigidbody.velocity.x);
            _animator.SetFloat(_zVelocityHash, -_rigidbody.velocity.y);
        }
    }

    private float GetMaxSpeed()
    {
        if (_inputManager.Run)
            return _sprintSpeed;

        return _walkSpeed;
    }

    private void CameraMovements()
    {
        float mouse_x = _inputManager.Look.x;
        float mouse_y = _inputManager.Look.y;
        _firstPersonCamera.position = _firstPersonCameraPosition.position;

        _xRotation -= mouse_y * _mouseSensitivity * Time.deltaTime;
        _xRotation = Mathf.Clamp(_xRotation, _xRotationUpperLimit, _xRotationLowerLimit);


        _firstPersonCamera.localRotation = Quaternion.Euler(_xRotation, 0.0f, 0.0f);

        if(use)
            transform.Rotate(Vector3.up, mouse_x * _mouseSensitivity * Time.deltaTime);
        else
            transform.Rotate(Vector3.up * mouse_x * _mouseSensitivity * Time.smoothDeltaTime);
    }
}
