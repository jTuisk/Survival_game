using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Misc;
using Game.SolarSystem.Planet.Generators;
using Game.SolarSystem.Planet.Settings;

namespace Game.SolarSystem.Planet.NatureFilters
{
    public class ForestFilter : INatureNoiseFilter
    {
        ShapeGenerator shapeGenerator;
        ForestSettings.Forest forestSettings;
        Noise noise;

        public ForestFilter(ForestSettings.Forest forestSettings, ShapeGenerator shapeGenerator)
        {
            this.forestSettings = forestSettings;
            this.noise = new Noise(forestSettings.seed);
            this.shapeGenerator = shapeGenerator;
        }

        public float Evaluate(Vector3 pointOnSphere)
        {
            throw new System.NotImplementedException();
        }

        public void Generate()
        {
            Debug.Log("Generation forests!");
        }

        public void SetSeed(int seed)
        {
            noise = new Noise(seed);
        }
    }
}