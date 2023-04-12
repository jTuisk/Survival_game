using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.SolarSystem.Temperature
{
    [System.Serializable]
    public class SensorTemperatureData
    {
        [ReadOnly] public float currentTemperature = 0f;
        public float startTemperature = 15f;
        public float minTemperature = -30f;
        public float maxTempature = 70f;
        [Range(0f, 10f)] public float temperatureStrength = 1f;
    }
}

