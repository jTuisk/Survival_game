using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.SolarSystem.Planet.ShapeFilters
{
    public interface IShapeNoiseFilter
    {
        public float Evaluate(Vector3 pointOnSphere);
        public void SetSeed(int seed);
    }
}

