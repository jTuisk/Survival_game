using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    Rigidbody _rb;

    [SerializeField] bool _canRotate = true;
    [SerializeField] float _rotationSpeed = 5f;

    List<Rigidbody> _objectsOnPlatform = new List<Rigidbody>();

     
    // Start is called before the first frame update
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_canRotate)
        {
            _rb.rotation = Quaternion.Euler(_rb.rotation.eulerAngles.x,
                                            _rb.rotation.eulerAngles.y + _rotationSpeed * Time.fixedDeltaTime,
                                            _rb.rotation.eulerAngles.z);


            foreach (Rigidbody rb in _objectsOnPlatform)
            {
                float rotationAmount = _rotationSpeed * Time.fixedDeltaTime;

                Quaternion localAngleAxis = Quaternion.AngleAxis(rotationAmount, _rb.transform.up);
                rb.position = (localAngleAxis * (rb.position - _rb.position)) + _rb.position;

                Quaternion globalAngleAxis = Quaternion.AngleAxis(rotationAmount, rb.transform.InverseTransformDirection(_rb.transform.up));
                rb.rotation *= globalAngleAxis;
            }
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if(!(other.attachedRigidbody == null) && !(other.attachedRigidbody.isKinematic))
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
