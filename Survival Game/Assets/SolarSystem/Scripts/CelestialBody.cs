using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.SolarSystem.Planet;

namespace Game.SolarSystem
{
    public class CelestialBody : MonoBehaviour
    {
        public CelestialBodyData data;
        [SerializeField] SphereCollider collider;
        [SerializeField] IcosahedronPlanet planet;

        List<Rigidbody> objectsOnPlatform = new List<Rigidbody>();
        Rigidbody rb;

        Vector3 _platformPositionLastFrame = Vector3.zero;

        private void OnValidate()
        {
            gameObject.name = data.name;
            if(collider != null && planet != null && planet.GetShapeGenerator() != null)
            {
                collider.radius = planet.GetShapeGenerator().MinMax.Max + 0.1f;
            }
        }

        private void Awake()
        {
            if (TryGetComponent<Rigidbody>(out rb))
            {
                rb.mass = data.mass;
                data.velocity = data.initialVelocity;
            }
        }

        public void UpdateVelocity(Vector3 acceleration, float timeStep)
        {
            data.velocity += acceleration * timeStep * data.velocityMultiplier;
        }

        public void UpdatePosition(float timeStep)
        {
            _platformPositionLastFrame = rb.position;
            transform.position += data.velocity * timeStep;

            /*foreach (Rigidbody objectRb in objectsOnPlatform)
            {
                if (objectRb.gameObject.name == "Player")
                {
                    objectRb.position += (rb.position - _platformPositionLastFrame);
                }
            }*/
            //rb.MovePosition(rb.position + data.velocity * timeStep);
        }

        
        public void UpdateRotation(float timeStep)
        {
            transform.Rotate(data.rotationDirection * CalculateRotationSpeed(), Space.Self);
            

            /*rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles.x + (data.rotationDirection.x * rotationSpeed) * Time.fixedDeltaTime,
                                           rb.rotation.eulerAngles.y + (data.rotationDirection.y * rotationSpeed) * Time.fixedDeltaTime,
                                           rb.rotation.eulerAngles.z + (data.rotationDirection.z * rotationSpeed) * Time.fixedDeltaTime);*/


            //transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
        }

        private float CalculateRotationSpeed() //TODO Math for this.
        {
            return data.dayLength > 0 ? 1f / data.dayLength : 1f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!(other.attachedRigidbody == null) && !(other.attachedRigidbody.isKinematic))
            {
                if (!(objectsOnPlatform.Contains(other.attachedRigidbody)))
                {
                    objectsOnPlatform.Add(other.attachedRigidbody);

                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!(other.attachedRigidbody == null))
            {
                if ((objectsOnPlatform.Contains(other.attachedRigidbody)))
                {
                    objectsOnPlatform.Remove(other.attachedRigidbody);
                }
            }
        }

    }
}

