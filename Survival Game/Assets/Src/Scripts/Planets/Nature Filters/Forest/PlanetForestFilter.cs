using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetForestFilter : INatureNoiseFilter
{
    PlanetShapeGenerator shapeGenerator;
    PlanetForestSettings.Forest forestSettings;
    Noise noise;

    public PlanetForestFilter(PlanetForestSettings.Forest forestSettings, PlanetShapeGenerator shapeGenerator) 
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
