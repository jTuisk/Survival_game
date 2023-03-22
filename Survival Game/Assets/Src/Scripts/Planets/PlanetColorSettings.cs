using UnityEngine;

[CreateAssetMenu(menuName = "Planet/Color settings")]
public class PlanetColorSettings : ScriptableObject
{
    //public Color color = Color.white;
    public Gradient gradient;
    public Material planetMaterial;
    public PlanetBiomeColorSettings biomeColorSettings;

    [System.Serializable]
    public class PlanetBiomeColorSettings
    {
        public Biome[] biomes;
        public PlanetNoiseSettings noiseSettings;
        public float noiseOffSet;
        public float noiseStrength;
        [Range(0f, 1f)]
        public float blendAmount;

        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;
            public Color biomeColor;
            [Range(0f, 1f)]
            public float startHeight;
            [Range(0f, 1f)]
            public float biomePercent;
        }
    }
}
