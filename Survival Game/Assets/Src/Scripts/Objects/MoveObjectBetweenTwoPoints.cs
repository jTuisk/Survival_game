using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectBetweenTwoPoints : MonoBehaviour
{
    Rigidbody _rb;

    [SerializeField] bool _canMove = true;
    [SerializeField] float _movementSpeed = 1f;

    [SerializeField] Vector3 _startPosition = Vector3.zero;
    [SerializeField] Vector3 _moveFromStartToDirection = Vector3.zero;
    [SerializeField] Vector3 _endPosition = Vector3.zero;

    Vector3 _platformPositionLastFrame = Vector3.zero;
    float _timeScale = 0f;

    List<Rigidbody> _objectsOnPlatform = new List<Rigidbody>();


    // Start is called before the first frame update
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _startPosition = _rb.position;
        
        if(_endPosition == Vector3.zero)
        {
            _endPosition = new Vector3(_startPosition.x + _moveFromStartToDirection.x,
                                       _startPosition.y + _moveFromStartToDirection.y,
                                       _startPosition.z + _moveFromStartToDirection.z);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_canMove)
        {
            _platformPositionLastFrame = _rb.position;
            _timeScale = _movementSpeed / Vector3.Distance(_startPosition, _endPosition);
            _rb.position = Vector3.Lerp(_endPosition, _startPosition, Mathf.Abs(Time.time * _timeScale % 2 - 1));

            foreach (Rigidbody rb in _objectsOnPlatform)
            {
                rb.position += (_rb.position - _platformPositionLastFrame);

            }
        }

    }



    private void OnTriggerEnter(Collider other)
    {
        if (!(other.attachedRigidbody == null) && !(other.attachedRigidbody.isKinematic))
        {
            if (!(_objectsOnPlatform.Contains(other.attachedRigidbody)))
            {
                _objectsOnPlatform.Add(other.attachedRigidbody);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!(other.attachedRigidbody == null))
        {
            if ((_objectsOnPlatform.Contains(other.attachedRigidbody)))
            {
                _objectsOnPlatform.Remove(other.attachedRigidbody);
            }
        }
    }
}
