using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.SolarSystem.Planet.Settings;
using Game.SolarSystem.Planet.Generators;
using Game.SolarSystem.Planet.ShapeFilters;
using Game.SolarSystem.Planet.NatureFilters;


namespace Game.SolarSystem.Planet
{
    public static class NoiseFilterFactory
    {
        public static IShapeNoiseFilter CreateShapeNoiseFilter(NoiseSettings noiseSettings, int seed)
        {
            switch (noiseSettings.filterType)
            {
                case NoiseSettings.FilterType.Simple:
                    return new SimpleNoiseFilter(noiseSettings.simpleNoiseSettings, seed);

                case NoiseSettings.FilterType.Rigid:
                    return new RidgeNoiseFilter(noiseSettings.ridgeNoiseSettings, seed);

                default:
                    return null;
            }
        }

        public static INatureNoiseFilter CreateForestNoiseFilter(ForestSettings forestSettings, int seed, ShapeGenerator shapeGenerator)
        {
            switch (forestSettings.spawnType)
            {
                case ForestSettings.SpawnType.RandomTrees:
                    return new RandomTreesFilter(forestSettings.randomTrees, shapeGenerator);

                case ForestSettings.SpawnType.Forest:
                    return new ForestFilter(forestSettings.forest, shapeGenerator);

                default:
                    return null;
            }
        }

    }
}