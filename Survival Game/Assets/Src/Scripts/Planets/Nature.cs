using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nature : MonoBehaviour
{
    [SerializeField]
    private IcosahedronPlanet _planet;


    [SerializeField]
    private GameObject[] _treePrefabs;
    [SerializeField]
    private GameObject[] _stonesPrefabs;

    [SerializeField, Range(0, 10000)]
    private int _maxAmountOfTrees = 0;
    [SerializeField, Range (0, 10000)]
    private int _maxAmountOfStones = 0;

    [SerializeField]
    private List<GameObject> _ingameTrees = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _ingameStones = new List<GameObject>();

    private GameObject _treeParent;
    private GameObject _stoneParent;

    private void Awake()
    {
        _planet = _planet == null? GetComponent<IcosahedronPlanet>() : _planet;
        
        InitializeChildrens();
        PlaceObjects();
    }
    private void FixedUpdate()
    {
        PlaceObjects();
        RemoveObjects();
    }

    private void InitializeChildrens()
    {
        _treeParent = new GameObject("Trees");
        _treeParent.transform.parent = transform;
        _stoneParent = new GameObject("Stones");
        _stoneParent.transform.parent = transform;

    }



    private void PlaceObjects()
    {
        if(_maxAmountOfTrees > _ingameTrees.Count && _treePrefabs.Length > 0)
        {
            for (int i = _ingameTrees.Count; i < _maxAmountOfTrees; i++)
            {
                GameObject go = GetRandomPrefabFromArray(_treePrefabs);
                go.transform.position = _planet.GetRandomSurfacePoint();
                go.transform.localScale = new Vector3(1f, 1f, 1f) * Random.Range(0.5f, 1.1f);

                Vector3 spawnRotation = (go.transform.position - _planet.transform.position).normalized;
                Quaternion rotation = Quaternion.FromToRotation(go.transform.up, spawnRotation) * go.transform.rotation;
                go.transform.rotation = rotation;
                _ingameTrees.Add(Instantiate(go, _treeParent.transform));
            }
        }

        if (_maxAmountOfStones > _ingameStones.Count && _stonesPrefabs.Length > 0)
        {
            for (int i = _ingameStones.Count; i < _maxAmountOfStones; i++)
            {
                GameObject go = GetRandomPrefabFromArray(_stonesPrefabs);
                go.transform.position = _planet.GetRandomSurfacePoint();
                go.transform.localScale = new Vector3(1f, 1f, 1f) * Random.Range(0.5f, 1.1f);

                Vector3 spawnRotation = (go.transform.position - _planet.transform.position).normalized;
                Quaternion rotation = Quaternion.FromToRotation(go.transform.up, spawnRotation) * go.transform.rotation;
                go.transform.rotation = rotation;
                _ingameStones.Add(Instantiate(go, _stoneParent.transform));
            }
        }
    }

    private void RemoveObjects()
    {
        while(_ingameTrees.Count > _maxAmountOfTrees)
        {
            Destroy(_ingameTrees[0]);
            _ingameTrees.RemoveAt(0);
        }
        while (_ingameStones.Count > _maxAmountOfStones)
        {
            Destroy(_ingameStones[0]);
            _ingameStones.RemoveAt(0);
        }
    }

    private GameObject GetRandomPrefabFromArray(GameObject[] array)
    {
        return array[Random.Range(0, array.Length - 1)];
    }
    
}
