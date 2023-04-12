using UnityEngine;

namespace Game.SolarSystem.Planet.Settings
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Planet/Color settings")]
    public class ColorSettings : ScriptableObject
    {
        public bool enable = true;
        public Gradient gradient;
        public Material planetMaterial;
    }

}
