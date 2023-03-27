using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetForestSettings
{
    public enum SpawnType { RandomTrees, Forest }
    public SpawnType spawnType;

    public RandomTrees randomTrees;
    public Forest forest;

    [System.Serializable]
    public class RandomTrees
    {
        public int seed;
        public List<GameObject> trees;
        public List<GameObject> generatedTrees;
        [Range(0f, 1f)]
        public float frequency = 1f;
        public float minElevationHeight = float.MinValue;
        public float maxElevationHeight = float.MaxValue;
        public uint forestSize = 100;
    }

    [System.Serializable]
    public class Forest : RandomTrees
    {
        public float minDistance = 0.1f;
    }
}