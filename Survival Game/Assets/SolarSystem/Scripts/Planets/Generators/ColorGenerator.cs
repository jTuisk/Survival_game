using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Misc;
using Game.SolarSystem.Planet.Settings;

namespace Game.SolarSystem.Planet.Generators
{
    public class ColorGenerator
    {
        ColorSettings colorSettings;
        Texture2D texture;
        const int textureResolution = 256;

        public void UpdateSettings(ColorSettings colorSettings)
        {
            this.colorSettings = colorSettings;
            if(texture == null)
            {
                texture = new Texture2D(textureResolution, 1);
            }
        }

        public void UpdateElevation(MinMax minMax)
        {
            if (colorSettings.enable)
            {
                colorSettings.planetMaterial.SetVector("_elevationMinMax", new Vector4(minMax.Min, minMax.Max));
            }
        }

        public void UpdateColors()
        {
            if (colorSettings.enable)
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
    }
}
