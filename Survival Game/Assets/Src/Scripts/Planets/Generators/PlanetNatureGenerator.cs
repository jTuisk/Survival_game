using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetNatureGenerator
{
    PlanetForestGenerator planetForestGenerator;
    PlanetMineralGenerator planetMineralGenerator;

    public PlanetNatureGenerator()
    {
        planetForestGenerator = new PlanetForestGenerator();
        planetMineralGenerator = new PlanetMineralGenerator();
    }

    public void UpdateSettings(PlanetNatureSettings natureSettings = null, int seed = 0, PlanetShapeGenerator shapeGenerator = null)
    {
        planetForestGenerator.UpdateSettings(natureSettings, seed, shapeGenerator);
        planetMineralGenerator.UpdateSettings(natureSettings, seed, shapeGenerator);
    }

    public void GenerateNature()
    {
        planetForestGenerator.GenerateForest();
        planetMineralGenerator.GenerateMinerals();
    }
}
