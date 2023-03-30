using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetColorGenerator
{
    PlanetColorSettings colorSettings;
    Texture2D texture;
    const int textureResolution = 256;

    public void UpdateSettings(PlanetColorSettings colorSettings)
    {
        this.colorSettings = colorSettings;
        if(texture == null)
        {
            texture = new Texture2D(textureResolution, 1);
        }
    }

    public void UpdateElevation(MinMax minMax)
    {
        colorSettings.planetMaterial.SetVector("_elevationMinMax", new Vector4(minMax.Min, minMax.Max));
    }

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
}
