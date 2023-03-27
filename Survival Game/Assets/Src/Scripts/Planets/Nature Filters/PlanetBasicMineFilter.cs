using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBasicMineralFilter : IMineralNoiseFilter
{
    PlanetMineralSettings.BasicMine mineSetting;
    int seed;

    public PlanetBasicMineralFilter(PlanetMineralSettings.BasicMine mineSetting, int seed)
    {
        this.mineSetting = mineSetting;
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
