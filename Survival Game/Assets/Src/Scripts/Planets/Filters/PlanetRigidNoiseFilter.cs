using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRigidNoiseFilter : INoiseFilter
{
    PlanetNoiseSettings.RigidNoiseSettings noiseSettings;
    Noise noise;

    public PlanetRigidNoiseFilter(PlanetNoiseSettings.RigidNoiseSettings noiseSettings, int seed)
    {
        this.noiseSettings = noiseSettings;
        noise = new Noise(seed);
    }

    public float Evaluate(Vector3 pointOnSphere)
    {
        float noiseValue = 0f;
        float frequency = noiseSettings.baseRoughness;
        float amplitude = 1f;
        float weight = 1f;
        
        for (int i = 0; i < noiseSettings.numLayers; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(pointOnSphere * frequency + noiseSettings.center));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * noiseSettings.weightMultiplier);

            noiseValue += v * amplitude;
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
