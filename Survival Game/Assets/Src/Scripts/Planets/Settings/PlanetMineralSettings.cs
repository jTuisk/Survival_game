using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetMineralSettings
{
    public enum MineralSpawnType { BasicMine, SmallMine, LargeMine }
    public MineralSpawnType mineralType;
    public List<GameObject> minerals;

    public SmallMine basicMine;
    public SmallMine smallMine;
    public LargeMine largeMine;

    [System.Serializable]
    public class BasicMine
    {
        public List<GameObject> minerals;

        public float minElevationHeight = float.MinValue;
        public float maxElevationHeight = float.MaxValue;
        [Range(0f, 180f)]
        public float maxSlope = 180f;
        [Range(0f, 1f)]
        public float frequency = 1f;
    }

    [System.Serializable]
    public class SmallMine : BasicMine
    {
        public uint mineSize = 100;
    }

    [System.Serializable]
    public class LargeMine : SmallMine
    {

    }
}
