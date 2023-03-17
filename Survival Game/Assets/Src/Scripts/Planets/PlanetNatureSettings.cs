using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Planet/Nature settings")]
public class PlanetNatureSettings : ScriptableObject
{
    public bool spawnTrees = true;
    public List<GameObject> trees;
    /*
     * List<Forests> (List of trees that grow in same place)
     */

    public bool spawnMinerals = true;
    public List<GameObject> minerals;
}