using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlanetNoiseFilterFactory
{
    public static INoiseFilter CreateNoiseFilter(PlanetNoiseSettings noiseSettings, int seed)
    {
        switch (noiseSettings.filterType)
        {
            case PlanetNoiseSettings.FilterType.Simple:
                return new PlanetSimpleNoiseFilter(noiseSettings.simpleNoiseSettings, seed);

            case PlanetNoiseSettings.FilterType.Rigid:
                return new PlanetRigidNoiseFilter(noiseSettings.rigidNoiseSettings, seed);

            default:
                return null;
        }
    }
}
