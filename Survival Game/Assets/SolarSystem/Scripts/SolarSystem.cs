using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player.Controller;

namespace Game.SolarSystem
{
    public class SolarSystem : MonoBehaviour
    {
        public bool Update = true;

        CelestialBody[] celestials;
        static SolarSystem instance;
        static SolarSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<SolarSystem>();
                }
                return instance;
            }
        }


        void Awake()
        {
            instance = this;
            celestials = FindObjectsOfType<CelestialBody>();
            Time.fixedDeltaTime = Universe.PhysicsTimeStep;

        }

        private void FixedUpdate()
        {
            if (!Update)
                return;

            UpdateVelocity();
            UpdatePosition();
            UpdateRotation();
        }

        public void UpdateVelocity()
        {
            for(int i = 0; i < instance.celestials.Length; i++)
            {
                Vector3 acceleration = CalculateAcceleration(instance.celestials[i]);
                instance.celestials[i].UpdateVelocity(acceleration, Universe.PhysicsTimeStep);
            }
        }
        public void UpdatePosition()
        {
            for (int i = 0; i < instance.celestials.Length; i++)
            {
                instance.celestials[i].UpdatePosition(Universe.PhysicsTimeStep);
            }
        }

        public void UpdateRotation()
        {
            for(int i = 0; i < instance.celestials.Length; i++)
            {
                instance.celestials[i].UpdateRotation(Universe.PhysicsTimeStep);
            }
        }

        public static Vector3 CalculateAcceleration(CelestialBody body)
        {
            Vector3 acceleration = Vector3.zero;

            /**
            * Newtons law
            * F = G * m_1 * m_2 / r^2
            * F = force between two objects
            * G = gravitational constant
            * m_1 = object 1 mass (body)
            * m_2 = object 2 mass (otherBody)
            * r^2 = distance between object 1 and 2 squared.
            */

            foreach (CelestialBody otherBody in Instance.celestials)
            {
                if(!otherBody.Equals(body))
                {
                    float m1 = body.data.mass;
                    float m2 = otherBody.data.mass;
                    float sqrtR = (otherBody.transform.position - body.transform.position).sqrMagnitude; // https://docs.unity3d.com/ScriptReference/Vector3-sqrMagnitude.html
                    //float F = Universe.GravitationalConstant * ((m1 * m2) / sqrtR);
                    float F = Universe.GravitationalConstant * m2 / sqrtR;
                    Vector3 direction = (otherBody.transform.position - body.transform.position).normalized;
                    acceleration += direction * F;
                }
            }
            return acceleration;
        }

        private void Gravity()
        {
            /**
             * Newtons law
             * F = G * m_1 * m_2 / r^2
             * F = force between two objects
             * G = gravitational constant
             * m_1 = object 1 mass (a)
             * m_2 = object 2 mass (b)
             * r^2 = distance between object 1 and 2 squared.
             */
            /*
            foreach (GameObject a in celestials)
            {
                Rigidbody a_rb = a.GetComponent<Rigidbody>();
                foreach (GameObject b in celestials)
                {
                    if (!a.Equals(b))
                    {
                        Rigidbody b_rb = b.GetComponent<Rigidbody>();

                        float m1 = a_rb.mass;
                        float m2 = b_rb.mass;
                        float r = Vector3.Distance(a.transform.position, b.transform.position);
                        float F = Universe.GravitationalConstant * ((m1 * m2) / (r * r));
                        Vector3 direction = (b.transform.position - a.transform.position).normalized;

                        a_rb.AddForce(direction * F);
                    }
                }
            }*/
        }

        void InitialVelocity()
        {
            /**
             * Circular orbit instant velocity
             * v0 = sqrt((G * m2) / r)
             * G = gravitational constant
             * m_2 = object 2 mass (b)
             * r^2 = distance between object 1 and 2 squared.
             */
            /*
            foreach (GameObject a in celestials)
            {
                Rigidbody a_rb = a.GetComponent<Rigidbody>();
                foreach (GameObject b in celestials)
                {
                    if (!a.Equals(b))
                    {
                        Rigidbody b_rb = b.GetComponent<Rigidbody>();

                        float m2 = b_rb.mass;
                        float r = Vector3.Distance(a.transform.position, b.transform.position);

                        a_rb.velocity += a.transform.right * Mathf.Sqrt((Universe.GravitationalConstant * m2) / r);
                    }
                }
            }*/
        }
    }
}


