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
                return new PlanetRigidNoiseFilter(noiseSettings.rigidNoiseSettings, seed);

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

    public static IMineralNoiseFilter CreateMineralNoiseFilter(PlanetMineralSettings mineralSettings, int seed)
    {
        switch (mineralSettings.mineralType)
        {
            case PlanetMineralSettings.MineralSpawnType.BasicMine:
                return new PlanetBasicMineralFilter(mineralSettings.basicMine, seed);

            case PlanetMineralSettings.MineralSpawnType.SmallMine:
                return new PlanetSmallMineFilter(mineralSettings.smallMine, seed);

            case PlanetMineralSettings.MineralSpawnType.LargeMine:
                return new PlanetLargeMineFilter(mineralSettings.largeMine, seed);

            default:
                return null;
        }
    }
}
