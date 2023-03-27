using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetForestGenerator 
{
    PlanetNatureSettings natureSettings;
    INatureNoiseFilter[] forestFilter;

    public void UpdateSettings(PlanetNatureSettings natureSettings, int seed, PlanetShapeGenerator shapeGenerator)
    {
        this.natureSettings = natureSettings;

        if (natureSettings.forestLayers == null || natureSettings.forestLayers.Length < 1)
            return;

        forestFilter = new INatureNoiseFilter[natureSettings.forestLayers.Length];

        for (int i = 0; i < forestFilter.Length; i++)
        {
            forestFilter[i] = PlanetNoiseFilterFactory.CreateForestNoiseFilter(natureSettings.forestLayers[i].forestSettings, seed, shapeGenerator);
        }
    }

    public Vector3 GetSpawnPoint()
    {
        return Random.insideUnitCircle.normalized;
    }

    public void GenerateForest()
    {
        for (int i = 0; i < natureSettings.forestLayers.Length; i++)
        {
            if (natureSettings.forestLayers[i].enabled)
            {
                forestFilter[i].Generate();
            }
        }
    }
}
