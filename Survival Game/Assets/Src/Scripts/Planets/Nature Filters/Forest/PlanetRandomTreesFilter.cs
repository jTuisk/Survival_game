using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRandomTreesFilter : INatureNoiseFilter
{
    PlanetShapeGenerator shapeGenerator;
    PlanetForestSettings.RandomTrees forestSettings;
    Noise noise;

    GameObject _parent;

    public PlanetRandomTreesFilter(PlanetForestSettings.RandomTrees forestSettings, PlanetShapeGenerator shapeGenerator)
    {
        this.forestSettings = forestSettings;
        this.noise = new Noise(forestSettings.seed);
        this.shapeGenerator = shapeGenerator;
    }


    public float Evaluate(Vector3 pointOnSphere)
    {
        throw new System.NotImplementedException();
    }

    public void Generate()
    {
        if(_parent == null)
        {
            _parent = GameObject.Find("Trees");
            if(_parent == null)
            {
                _parent = new GameObject("Trees");
            }
        }

        Debug.Log("Generating random trees!");
        for (int i = 0; _parent.transform.childCount < forestSettings.forestSize; i++)
        {
            GameObject tree = GetRandomTree();
            Vector3 pos = shapeGenerator.CalculatePointOnPlanet(Random.onUnitSphere, false);
            tree.transform.position = pos;
            tree.transform.localScale = new Vector3(100f, 100f, 100f) * Random.Range(0.5f, 1.3f);

            GameObject planet = GameObject.Find("Earth");

            if(planet != null)
            {
                Vector3 spawnRotation = pos.normalized;
                Quaternion rotation = Quaternion.FromToRotation(tree.transform.up, spawnRotation) * tree.transform.rotation;
                tree.transform.rotation = rotation;
            }
            
            UnityEngine.MonoBehaviour.Instantiate(tree, _parent.transform);
        }
    }

    private GameObject GetRandomTree()
    {
        return forestSettings.trees[Random.Range(0, forestSettings.trees.Count - 1)];
    }

    public void SetSeed(int seed)
    {
        noise = new Noise(seed);
    }
}
