using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlanetGravityBody : MonoBehaviour
{
    [SerializeField]
    private PlanetGravityAttractor _attractor;

    private Rigidbody _rigidbody;



    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        _attractor.Attract(transform, _rigidbody);
    }
}
