using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.SolarSystem
{
    public static class Universe 
    {
        public const float GravitationalConstant = 1f; // 6.67 × 10-11 
        public const float PhysicsTimeStep = 0.01f;
        public const float VoidTemperature = -270f;
        public const float VoidTemperatureStrength = 0.00004337f;
    }
}