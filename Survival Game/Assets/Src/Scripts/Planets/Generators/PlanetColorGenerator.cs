using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetColorGenerator
{
    PlanetColorSettings colorSettings;
    Texture2D texture;
    const int textureResolution = 256;
    //INoiseFilter biomeNoiseFilter;
    int seed;

    public void UpdateSettings(PlanetColorSettings colorSettings, int seed)
    {
        this.colorSettings = colorSettings;
        this.seed = seed;
        if(texture == null)
        {
            texture = new Texture2D(textureResolution, 1);
        }
        /*if(texture == null || texture.height != colorSettings.biomeColorSettings.biomes.Length)
        {
            texture = new Texture2D(textureResolution, colorSettings.biomeColorSettings.biomes.Length);
        }
        biomeNoiseFilter = PlanetNoiseFilterFactory.CreateNoiseFilter(colorSettings.biomeColorSettings.noiseSettings, seed);*/
    }

    public void UpdateElevation(MinMax minMax)
    {
        colorSettings.planetMaterial.SetVector("_elevationMinMax", new Vector4(minMax.Min, minMax.Max));
    }

    /*public void UpdateColors()
    {
        if (colorSettings.biomeColorSettings.biomes.Length == 0)
            return;

        Color[] colors = new Color[texture.height * texture.width];

        var biomes = colorSettings.biomeColorSettings.biomes;

        for(int i = 0;  i < biomes.Length; i++)
        {
            for (int j = 0; j < colors.Length; j++)
            {
                Color mainColor = biomes[i].gradient.Evaluate(i / (textureResolution - 1f));
                Color biomeColor = biomes[i].biomeColor;

                colors[i] = mainColor * (1 - biomes[i].biomePercent) + biomeColor * biomes[i].biomePercent;
            }
        }

        texture.SetPixels(colors);
        texture.Apply();
        colorSettings.planetMaterial.SetTexture("_planetTexture", texture);
    }*/

    public void UpdateColors()
    {
        Color[] colours = new Color[textureResolution];
        for (int i = 0; i < colours.Length; i++)
        {
            colours[i] = colorSettings.gradient.Evaluate(i / (textureResolution - 1f));
        }

        texture.SetPixels(colours);
        texture.Apply();
        colorSettings.planetMaterial.SetTexture("_planetTexture", texture);
    }

    /*public float BiomePercentFromPoint(Vector3 point)
    {
        float heightPercent = (point.y + 1)/2f;
        heightPercent += (biomeNoiseFilter.Evaluate(point) - colorSettings.biomeColorSettings.noiseOffSet) * colorSettings.biomeColorSettings.noiseStrength;
        float biomeIndex = 0;
        int numBiomes = colorSettings.biomeColorSettings.biomes.Length;
        float blendRange = colorSettings.biomeColorSettings.blendAmount / 2f + 0.0001f;

        for(int i = 0; i < numBiomes; i++)
        {
            var biome = colorSettings.biomeColorSettings.biomes[i];
            float distance = biome.startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, distance);
            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
        }
        return biomeIndex / Mathf.Max(1, numBiomes - 1);
    }*/
}
