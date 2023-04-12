using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.SolarSystem.Temperature
{
    public class SunTemperatureSensor : TemperatureSensor
    {
        private void Awake()
        {
            base.shootRays = false;
        }
    }
}

