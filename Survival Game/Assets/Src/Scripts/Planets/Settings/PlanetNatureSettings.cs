using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Planet/Nature settings")]
public class PlanetNatureSettings : ScriptableObject
{
    public ForestLayers[] forestLayers;

    [System.Serializable]
    public class ForestLayers
    {
        public bool enabled = true;
        public PlanetForestSettings forestSettings;
    }
}