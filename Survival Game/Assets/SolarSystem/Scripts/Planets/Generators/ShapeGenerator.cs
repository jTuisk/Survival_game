using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Misc;
using Game.SolarSystem.Planet.Settings;
using Game.SolarSystem.Planet.ShapeFilters;


namespace Game.SolarSystem.Planet.Generators
{
    public class ShapeGenerator
    {
        ShapeSettings shapeSettings;
        IShapeNoiseFilter[] noiseFilters;
        public MinMax MinMax { get; private set; }

        public void UpdateSettings(ShapeSettings shapeSettings, int seed)
        {
            this.shapeSettings = shapeSettings;
            noiseFilters = new IShapeNoiseFilter[shapeSettings.noiseLayers.Length];

            for (int i = 0; i < noiseFilters.Length; i++)
            {
                noiseFilters[i] = NoiseFilterFactory.CreateShapeNoiseFilter(shapeSettings.noiseLayers[i].noiseSettings, seed);
            }
            MinMax = new MinMax();
        }

        public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere, bool addToMinMax = true)
        {
            float elevation = 0f;
            float firstLayerValue = 0f;

            if (noiseFilters.Length > 0)
            {
                firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
                if (shapeSettings.noiseLayers[0].enabled)
                {
                    elevation = firstLayerValue;
                }
            }

            for (int i = 1; i < noiseFilters.Length; i++)
            {
                if (shapeSettings.noiseLayers[i] == null)
                    continue;

                if (shapeSettings.noiseLayers[i].enabled)
                {
                    float mask = (shapeSettings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1f;
                    elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
                }
            }
            elevation = shapeSettings.radius * (1 + elevation);
            if (addToMinMax)
            {
                MinMax.AddValue(elevation);
            }
            return pointOnUnitSphere * elevation;
        }

        public void GenerateShape(ref List<Vector3> vertices)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = CalculatePointOnPlanet(vertices[i]);
            }
        }

        public void SetSeed(int seed)
        {
            for (int i = 0; i < noiseFilters.Length; i++)
            {
                noiseFilters[i].SetSeed(seed);
            }
        }

        public void ResetMinMax()
        {
            MinMax = new MinMax();
        }
    }
}