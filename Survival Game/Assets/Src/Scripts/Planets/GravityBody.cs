using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour
{
   [SerializeField]
   private float _gravity = -9.81f;
   [SerializeField]
   private float _gravityRadius = 0.1f;
   [SerializeField, Range(0.1f, 100000f)]
   private float _planetMass = 1000f;
   [SerializeField]
   private Vector3 _gravityPoint = Vector3.zero;

}
