using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputManager _inputManager;
    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;

    [SerializeField]
    private bool _randomLocationAtStart = true;
    [SerializeField]
    private IcosahedronPlanet _planet;  

    [Header("Gravity settings")]
    [SerializeField]
    private bool _useGravity = true;
    [SerializeField]
    private bool _autoOrient = true;
    [SerializeField, Range(0.0f, 1000f)]
    private float _oirientSlerpSpeed = 10f;
    [SerializeField] //[SerializeField, ReadOnly]
    private CelestialBody _celestialBody;
    [SerializeField, Range(0.0f, -1000f)] //[SerializeField, ReadOnly]
    private float _gravityForce = -9.81f;
    [SerializeField, Range(0.0f, -10f)]
    private float _stickToGroundGravity = -5f;


    [Header("Movement settings")]
    [SerializeField, ReadOnly]
    private Vector3 _playerMoveInput = Vector3.zero;
    private Vector3 _targetVelocity;
    private Vector3 _smoothVelocity;
    private Vector3 _smoothVectorRef;

    [SerializeField, ReadOnly]
    private float _currentSpeed = 0.0f;
    [SerializeField, Range(0.1f, 100f)]
    private float _walkSpeed = 5.4f;
    [SerializeField, Range(0.1f, 100f)]
    private float _runSpeed = 9.8f;

    [SerializeField, Range(0.1f, 100f)]
    private float _jumpForce = 10.0f;
    private float _vSmoothTime = 0.1f;
    private float _airSmoothTime = 0.5f;


    [Header("Mouse settings")]
    [SerializeField, Range(0.1f, 1000f)]
    private float _mouseSensitivity = 100f;

    [Header("Other player settings")]
    [SerializeField]
    private float _mass = 80.0f;

    [Header("Camera settings")]
    [SerializeField]
    private Transform _firstPersonCamera;
    [SerializeField]
    private Transform _firstPersonCameraPosition;

    [SerializeField]
    private float _xRotationUpperLimit = -40f;
    [SerializeField]
    private float _xRotationLowerLimit = 80f;
    private float _xRotation;

    [Header("Ground check")]
    [SerializeField]
    private bool _playerIsGrounded = false;
    [SerializeField, Range(0.0f, 2f)]
    private float _groundCheckRadiusMultiplier = 1f;
    [SerializeField, Range(-1f, 1f)]
    private float _groundCheckDistance = 0.05f;
    RaycastHit _groundCheckHit = new RaycastHit();

    [Header("Animation settings")]
    [SerializeField]
    private Animator _animator;
    private bool _hasAnimator;
    private int _speedHash;


    private void Awake()
    {
        HideCursor.ShowCurors(false, true);
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _inputManager = GetComponent<InputManager>();
        InitializeRigidbody();
        InitializeAnimator();
    }

    private void Start()
    {
        if(_randomLocationAtStart && _planet != null)
        {
            RelocatePlayer(_planet.GetRandomSurfacePoint());
        }
    }

    private void InitializeRigidbody()
    {
        if(!TryGetComponent<Rigidbody>(out _rb))
        {
            _rb = gameObject.AddComponent<Rigidbody>() as Rigidbody;
        }
        _rb.useGravity = false;
        _rb.isKinematic = false;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.mass = _mass;
    }

    private void InitializeAnimator()
    {
        _hasAnimator = _animator != null ? true : TryGetComponent<Animator>(out _animator);

        _speedHash = Animator.StringToHash("Speed");
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }


    private void HandleMovement()
    {
        if (Time.timeScale == 0)
            return;

        HandleGravity();

        HandleMovementInput();
        HandleCameraMovements();
        HandleAnimations();
    }

    private void HandleGravity()
    {
        if (!_useGravity)
            return;


        if (_autoOrient)
        {
            if (_celestialBody != null)
            {
                Vector3 planetOrigin = _celestialBody.transform.position;
                Vector3 gravityUp = (transform.position - planetOrigin).normalized; 
                _rb.rotation = Quaternion.FromToRotation(transform.up, gravityUp) * _rb.rotation;

                /*
                    Vector3 spawnRotation = toPos.normalized;
                    Quaternion rotation = Quaternion.FromToRotation(transform.up, spawnRotation) * transform.rotation;
                    transform.rotation = rotation; 
                 */
            }
        }
        //Get highest gravity Force or get nearest celestian body and get gravity force from that.

        _rb.AddForce(transform.up * _gravityForce, ForceMode.VelocityChange);
    }

    private void HandleMovementInput()
    {
        _playerIsGrounded = IsGrounded();
        _playerMoveInput = GetMoveInput();

        _targetVelocity = transform.TransformDirection(_playerMoveInput.normalized) * GetMovementSpeed();
        _smoothVelocity = Vector3.SmoothDamp(_smoothVelocity, _targetVelocity, ref _smoothVectorRef, GetSmoothVelocityTime());

        _currentSpeed = _smoothVelocity.magnitude;

        if (_inputManager.Jump)
        {
            HandleJump();
        }
        else
        {
            StickPlayerToGround();
        }

        _rb.MovePosition(_rb.position + _smoothVelocity * Time.fixedDeltaTime);
    }

    private void HandleCameraMovements()
    {
        float mouse_x = _inputManager.Look.x;
        float mouse_y = _inputManager.Look.y;
        _firstPersonCamera.position = _firstPersonCameraPosition.position;

        _xRotation -= mouse_y * _mouseSensitivity * Time.deltaTime;
        _xRotation = Mathf.Clamp(_xRotation, _xRotationUpperLimit, _xRotationLowerLimit);

        _firstPersonCamera.localRotation = Quaternion.Euler(_xRotation, 0.0f, 0.0f);

        transform.Rotate(Vector3.up, mouse_x * _mouseSensitivity * Time.fixedDeltaTime);
    }

    private void HandleAnimations()
    {
        if (_hasAnimator)
        {
            _animator.SetFloat(_speedHash, _currentSpeed);
        }
    }

    private void HandleJump()
    {
        if (_playerIsGrounded)
        {
            _rb.AddForce(transform.up * _jumpForce, ForceMode.VelocityChange);
            _playerIsGrounded = false;
        }
    }

    private void StickPlayerToGround()
    {
        if(!_useGravity)
            return;

        if (_playerIsGrounded)
        {
            _rb.AddForce(transform.up * _stickToGroundGravity, ForceMode.VelocityChange);
        }
    }
    private bool IsGrounded()
    {
        if(_celestialBody != null)
        {
            Vector3 relativeVelocity = _rb.velocity - _celestialBody.Velocity;
            if(relativeVelocity.y <= _jumpForce * 0.5f)
            {
                float sphereCastRadius = _capsuleCollider.radius * _groundCheckRadiusMultiplier;
                float sphereCastTravelDistance = _capsuleCollider.bounds.extents.y + sphereCastRadius + _groundCheckDistance;

                //TODO: Add LayerMask?
                return Physics.SphereCast(_rb.position, sphereCastRadius, -transform.up, out _groundCheckHit, sphereCastTravelDistance);
            }
        }
        return false;
    }
    private float GetSmoothVelocityTime()
    {
        return _playerIsGrounded ? _vSmoothTime : _airSmoothTime;
    }

    private float GetMovementSpeed()
    {
        if (_inputManager.Run)
            return _runSpeed;

        return _walkSpeed;
    }

    private Vector2 GetLookInput()
    {
        return new Vector3(_inputManager.Look.x, _inputManager.Look.y);
    }

    private Vector3 GetMoveInput()
    {
        return new Vector3(_inputManager.Move.x, 0.0f, _inputManager.Move.y);
    }

    private void RelocatePlayer(Vector3 toPos)
    {
        transform.position = toPos;
        Vector3 spawnRotation = toPos.normalized;
        Quaternion rotation = Quaternion.FromToRotation(transform.up, spawnRotation) * transform.rotation;
        transform.rotation = rotation;
    }
}
