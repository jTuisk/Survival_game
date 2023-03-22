using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetShapeGenerator
{
    PlanetShapeSettings shapeSettings;
    INoiseFilter[] noiseFilters;
    public MinMax MinMax { get; private set; }

    public void UpdateSettings(PlanetShapeSettings shapeSettings, int seed)
    {
        this.shapeSettings = shapeSettings;
        noiseFilters = new INoiseFilter[shapeSettings.noiseLayers.Length];

        for(int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = PlanetNoiseFilterFactory.CreateNoiseFilter(shapeSettings.noiseLayers[i].noiseSettings, seed);
        }
        MinMax = new MinMax();
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float elevation = 0f;
        float firstLayerValue = 0f;

        if(noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (shapeSettings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for(int i = 1; i < noiseFilters.Length; i++)
        {
            if (shapeSettings.noiseLayers[i].enabled)
            {
                //Debug.Log(shapeSettings.noiseLayers[i].noiseSettings.filterType);
                float mask = (shapeSettings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1f;
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }
        elevation = shapeSettings.radius * (1 + elevation);
        MinMax.AddValue(elevation);
        return pointOnUnitSphere * elevation;
    }

    public void GenerateShape(ref List<Vector3> vertices)
    {
        for(int i = 0; i < vertices.Count; i++)
        {
            vertices[i] = CalculatePointOnPlanet(vertices[i]);
        }
        Debug.Log($"min {MinMax.Min}, max {MinMax.Max}");
    }

    public void SetSeed(int seed)
    {
        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i].SetSeed(seed);
        }
    }
}
