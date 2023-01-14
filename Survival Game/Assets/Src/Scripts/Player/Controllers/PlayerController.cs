using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private InputManager _inputManager;

    [SerializeField]
    private Animator _animator;
    private bool _hasAnimator;
    [SerializeField]
    private float _animatorBlendSpeed = 1f;
    private int _xVelocityHash;
    private int _yVelocityHash;

    private const float _walkSpeed = 2f;
    private const float _sprintSpeed = 6f;

    private Vector2 _currentVelocity;

    [Header("Camera")]
    [SerializeField]
    private Transform _thirdPersonCamera;
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
    }

    private void Start()
    {
        _hasAnimator = _animator != null ? true : TryGetComponent<Animator>(out _animator);
        _rigidbody = GetComponent<Rigidbody>();
        _inputManager = GetComponent<InputManager>();


        _xVelocityHash = Animator.StringToHash("X_Velocity");
        _yVelocityHash = Animator.StringToHash("Y_Velocity");
    }

    private void FixedUpdate()
    {
        Move();        
    }

    private void LateUpdate()
    {
        CameraMovements();
    }

    private void Move()
    {
        //if (!_hasAnimator)
        //    return;

        float targetSpeed = GetTargetSpeed();

        Debug.Log($"Move: {_inputManager.Move}, targetSpeed: {targetSpeed}");

        _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, _animatorBlendSpeed * Time.fixedDeltaTime);
        _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, _animatorBlendSpeed * Time.fixedDeltaTime);

        float xVelocityDifference = _currentVelocity.x - _rigidbody.velocity.x;
        float yVelocityDifference = _currentVelocity.y - _rigidbody.velocity.y;



        Debug.Log($"Move: {_inputManager.Move}, targetSpeed: {targetSpeed}, xVelDif: {xVelocityDifference}, yVelDif: {yVelocityDifference}");

        //_rigidbody.AddForce(transform.TransformVector(new Vector3(xVelocityDifference, 0.0f, yVelocityDifference)), ForceMode.VelocityChange);
         _rigidbody.AddRelativeForce(new Vector3(_currentVelocity.x, 0.0f, _currentVelocity.y), ForceMode.Force);

        Debug.Log($"_hasAnimator: {_hasAnimator}");
        if (_hasAnimator)
        {
            _animator.SetFloat(_xVelocityHash, _currentVelocity.x);
            _animator.SetFloat(_yVelocityHash, _currentVelocity.y);
        }
    }

    /*private void Move()
    {
        //if (!_hasAnimator)
        //    return;

        float targetSpeed = GetTargetSpeed();

        Vector3 playerMoveInput = new Vector3(_inputManager.Move.x, 0.0f, _inputManager.Move.y) * targetSpeed;

        _rigidbody.AddRelativeForce(playerMoveInput, ForceMode.Force);

        if (_hasAnimator)
        {
            _animator.SetFloat(_xVelocityHash, _currentVelocity.x);
            _animator.SetFloat(_yVelocityHash, _currentVelocity.y);
        }
    }*/

    private float GetTargetSpeed()
    {
        if (_inputManager.Move == Vector2.zero)
            return 0.0f;

        if (_inputManager.Sprint)
            return _sprintSpeed;

        return _walkSpeed;
    }

    private void CameraMovements()
    {
        //if (!_hasAnimator)
        //    return;

        float mouse_x = _inputManager.Look.x;
        float mouse_y = _inputManager.Look.y;
        _firstPersonCamera.position = _firstPersonCameraPosition.position;

        _xRotation -= mouse_y * _mouseSensitivity * Time.deltaTime;
        _xRotation = Mathf.Clamp(_xRotation, _xRotationUpperLimit, _xRotationLowerLimit);


        _firstPersonCamera.localRotation = Quaternion.Euler(_xRotation, 0.0f, 0.0f);
        transform.Rotate(Vector3.up, mouse_x * _mouseSensitivity * Time.deltaTime);
        //transform.Rotate(Vector3.up * Mouse_X * mouseSpeed * Time.smoothDeltaTime);
    }


}
