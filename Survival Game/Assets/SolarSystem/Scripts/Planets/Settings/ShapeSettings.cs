using UnityEngine;

namespace Game.SolarSystem.Planet.Settings
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Planet/Shape settings")]
    public class ShapeSettings : ScriptableObject
    {
        [Range(0.01f, 10000f)]
        public float radius = 1f;
        [Range(0, 7)]
        public uint resolution = 1;
        public NoiseLayer[] noiseLayers;

        [System.Serializable]
        public class NoiseLayer
        {
            public bool enabled = true;
            public bool useFirstLayerAsMask;
            public NoiseSettings noiseSettings;
        }
    }
}