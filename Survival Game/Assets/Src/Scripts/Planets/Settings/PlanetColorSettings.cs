using UnityEngine;

[CreateAssetMenu(menuName = "Planet/Color settings")]
public class PlanetColorSettings : ScriptableObject
{
    public Gradient gradient;
    public Material planetMaterial;
}
