using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetShapeGenerator
{
    PlanetShapeSettings shapeSettings;

    public PlanetShapeGenerator(PlanetShapeSettings shapeSettings)
    {
        this.shapeSettings = shapeSettings;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        pointOnUnitSphere.z += Mathf.Sin(pointOnUnitSphere.y * shapeSettings.multiplier) * shapeSettings.radius;
        pointOnUnitSphere.x += Mathf.Sin(pointOnUnitSphere.y * shapeSettings.multiplier) * shapeSettings.radius;
        return pointOnUnitSphere;// * shapeSettings.radius;
    }

    public void GenerateShape(ref List<Vector3> vertices)
    {
        for(int i = 0; i < vertices.Count; i++)
        {
            vertices[i] = CalculatePointOnPlanet(vertices[i]);
        }
    }
}
