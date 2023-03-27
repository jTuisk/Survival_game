using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSmallMineFilter : IMineralNoiseFilter
{
    PlanetMineralSettings.SmallMine mineSettings;
    int seed;

    public PlanetSmallMineFilter(PlanetMineralSettings.SmallMine mineSettings, int seed)
    {
        this.mineSettings = mineSettings;
        this.seed = seed;
    }

    public void GenerateMinerals(Vector3 position, Vector3 normal)
    {
        throw new System.NotImplementedException();
    }

    public void SetSeed(int seed)
    {
        this.seed = seed;
    }
}
