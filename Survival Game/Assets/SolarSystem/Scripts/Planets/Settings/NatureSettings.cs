using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.SolarSystem.Planet.Settings
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Planet/Nature settings")]
    public class NatureSettings : ScriptableObject
    {
        public ForestLayers[] forestLayers;

        [System.Serializable]
        public class ForestLayers
        {
            public bool enabled = true;
            public ForestSettings forestSettings;
        }
    }
}