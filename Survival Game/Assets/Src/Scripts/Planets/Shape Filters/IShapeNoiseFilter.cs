using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShapeNoiseFilter
{
    public float Evaluate(Vector3 pointOnSphere);
    public void SetSeed(int seed);
}
