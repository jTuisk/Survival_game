using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.SolarSystem
{
    public enum CelestialBodyType { Star, Planet, Moon, Other }
    [System.Serializable]
    public class CelestialBodyData
    {
        public CelestialBodyType type = CelestialBodyType.Planet;
        public string name = "N/A";
        public float mass = 1f;
        public float surfaceGravity = 9.81f;
        public float dayLength = 1f;
        public Vector3 rotationDirection;
        [Range(0f, 5f)]
        public float velocityMultiplier = 1f;
        public Vector3 initialVelocity = Vector3.zero;
        [ReadOnly]
        public Vector3 velocity = Vector3.zero;
    }
}
