using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlanetNoiseFilterFactory
{
    public static IShapeNoiseFilter CreateShapeNoiseFilter(PlanetNoiseSettings noiseSettings, int seed)
    {
        switch (noiseSettings.filterType)
        {
            case PlanetNoiseSettings.FilterType.Simple:
                return new PlanetSimpleNoiseFilter(noiseSettings.simpleNoiseSettings, seed);

            case PlanetNoiseSettings.FilterType.Rigid:
                return new PlanetRidgeNoiseFilter(noiseSettings.ridgeNoiseSettings, seed);

            default:
                return null;
        }
    }

    public static INatureNoiseFilter CreateForestNoiseFilter(PlanetForestSettings forestSettings, int seed, PlanetShapeGenerator shapeGenerator)
    {
        switch (forestSettings.spawnType)
        {
            case PlanetForestSettings.SpawnType.RandomTrees:
                return new PlanetRandomTreesFilter(forestSettings.randomTrees, shapeGenerator);

            case PlanetForestSettings.SpawnType.Forest:
                return new PlanetForestFilter(forestSettings.forest, shapeGenerator);

            default:
                return null;
        }
    }

}
