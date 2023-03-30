using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetNoiseSettings
{
    public enum FilterType { Simple, Rigid };
    public FilterType filterType;

    public SimpleNoiseSettings simpleNoiseSettings;
    public RidgeNoiseSettings ridgeNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        public float strength = 1f;
        public float baseRoughness = 1f;
        public float roughness = 1f;
        [Range(0, 10)]
        public int numLayers = 1;
        public float presistence = 0.5f;
        public Vector3 center = Vector3.zero;
        public float minValue = 0f;
    }

    [System.Serializable]
    public class RidgeNoiseSettings : SimpleNoiseSettings
    {
        public float weightMultiplier = 1f;
    }
}
