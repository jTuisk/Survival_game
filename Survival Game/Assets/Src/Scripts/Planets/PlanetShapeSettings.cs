using UnityEngine;

[CreateAssetMenu(menuName = "Planet/Shape settings")]
public class PlanetShapeSettings : ScriptableObject
{
    [Range(0.1f, 1000f)]
    public float radius = 1f;
    [Range(0, 7)]
    public uint resolution = 1;

    [Range(-360, 360)]
    public float multiplier = 1f;
}
