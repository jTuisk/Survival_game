using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSimpleNoiseFilter : IShapeNoiseFilter
{
    PlanetNoiseSettings.SimpleNoiseSettings noiseSettings;
    Noise noise;

    public PlanetSimpleNoiseFilter(PlanetNoiseSettings.SimpleNoiseSettings noiseSettings, int seed)
    {
        this.noiseSettings = noiseSettings;
        noise = new Noise(seed);
    }

    public float Evaluate(Vector3 pointOnSphere)
    {
        float noiseValue = 0f;
        float frequency = noiseSettings.baseRoughness;
        float amplitude = 1f;

        for (int i = 0; i < noiseSettings.numLayers; i++)
        {
             float v = noise.Evaluate(pointOnSphere * frequency + noiseSettings.center);
             noiseValue += (v + 1) * 0.5f * amplitude;
             frequency *= noiseSettings.roughness;
             amplitude *= noiseSettings.presistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - noiseSettings.minValue);

        return noiseValue * noiseSettings.strength;
    }
    public void SetSeed(int seed)
    {
        noise = new Noise(seed);
    }
}
