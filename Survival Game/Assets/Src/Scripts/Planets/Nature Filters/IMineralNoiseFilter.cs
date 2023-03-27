using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMineralNoiseFilter
{
    public void GenerateMinerals(Vector3 position, Vector3 normal);
    public void SetSeed(int seed);
}
