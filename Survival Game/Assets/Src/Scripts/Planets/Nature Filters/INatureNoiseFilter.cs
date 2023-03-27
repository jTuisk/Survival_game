using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INatureNoiseFilter
{
    public void Generate();
    public float Evaluate(Vector3 pointOnSphere);
    public void SetSeed(int seed);

}
