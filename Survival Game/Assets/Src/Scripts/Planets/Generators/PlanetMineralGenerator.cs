using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMineralGenerator 
{
    PlanetNatureSettings natureSettings;
    IMineralNoiseFilter[] mineralFilter;

    public void UpdateSettings(PlanetNatureSettings natureSettings, int seed, PlanetShapeGenerator shapeGenerator)
    {
        this.natureSettings = natureSettings;

        if (natureSettings.forestLayers == null || natureSettings.forestLayers.Length < 1)
            return;

        mineralFilter = new IMineralNoiseFilter[natureSettings.mineralLayers.Length];

        for (int i = 0; i < mineralFilter.Length; i++)
        {
            mineralFilter[i] = PlanetNoiseFilterFactory.CreateMineralNoiseFilter(natureSettings.mineralLayers[i].mineralSettings, seed);
        }
    }

    public void GenerateMinerals()
    {

    }
}
