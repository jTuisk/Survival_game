using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class IcosahedronPlanet : MonoBehaviour
{

    [Header("Generation seed")]
    [SerializeField]
    private bool _generateSeedFromString = false;
    [SerializeField]
    private string _seedString = "";
    [SerializeField, ReadOnly]
    private int _seed;

    [Header("Generation size and resolution")]
    [SerializeField, Range(0.1f, 1000f)]
    private float _radius = 1;
    [SerializeField, Range(0, 11)]
    private int _meshDivides = 0;

    [Header("Mesh")]
    [SerializeField]
    private string _meshName;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Mesh _mesh;
    [SerializeField, ReadOnly]
    private int _verticeCount = 0;


    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _seed = GetNewSeed(_generateSeedFromString);
        _mesh = InitializeNewMesh(_meshName);
        _meshFilter.mesh = _mesh;
        GenerateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        //DivideTriangles();
        GenerateMesh();
    }

    private void OnDrawGizmos()
    {
        if(_meshFilter != null)
        {
            Gizmos.color = Color.cyan;
            foreach (var vert in GetDefaultVertices())
            {
                Gizmos.DrawCube(transform.TransformPoint(vert), new Vector3(0.03f, 0.03f, 0.03f) * _radius);
            }

            Gizmos.color = Color.blue;
            foreach (var vert in _meshFilter.sharedMesh.vertices)
            {
                Gizmos.DrawCube(transform.TransformPoint(vert), new Vector3(0.015f, 0.015f, 0.015f)*_radius);
            }
        }
    }

    private void GenerateMesh()
    {
        List<Vector3> vertices = GetDefaultVertices();
        List<Triangle> triangles = GetDefaultTriangles();

        vertices = Subdivide(vertices, triangles, _meshDivides);


        _mesh.vertices = vertices.ToArray();
        _verticeCount = _mesh.vertices.Length;
    }

    private List<Vector3> Subdivide(List<Vector3> vertices, List<Triangle> triangles, int divides)
    {        
        /**
         * Total count for vertices.
         * [divideTo, verticeCount]
         * [1, 12], [2, 42], [3, 162], [4, 642], [5, 2562], [6, 10242]
         */
        for (int i = 0; i < divides; i++)
        {
            List<Triangle> newTriangles = new List<Triangle>();
            for(int j = 0; j < triangles.Count; j++)
            {
                Triangle currentFace = triangles[j];

                Vector3 a = vertices[currentFace.a];
                Vector3 b = vertices[currentFace.b];
                Vector3 c = vertices[currentFace.c];

                Vector3 ab = Vector3.Lerp(a, b, 0.5f);
                Vector3 bc = Vector3.Lerp(b, c, 0.5f);
                Vector3 ca = Vector3.Lerp(c, a, 0.5f);

                int ab_index = AddAndGetVerticeIndex(vertices, ab);
                int bc_index = AddAndGetVerticeIndex(vertices, bc);
                int ca_index = AddAndGetVerticeIndex(vertices, ca);

                newTriangles.Add(new Triangle(vertices.IndexOf(a), ab_index, ca_index));
                newTriangles.Add(new Triangle(vertices.IndexOf(b), bc_index, ab_index));
                newTriangles.Add(new Triangle(vertices.IndexOf(c), ca_index, bc_index));
                newTriangles.Add(new Triangle(ab_index, bc_index, ca_index));
            }
            triangles = newTriangles;
        }

        return vertices;
    }

    private int AddAndGetVerticeIndex(List<Vector3> vertices, Vector3 newVertice)
    {
        int index = vertices.IndexOf(newVertice);

        if (index == -1)
        {
            vertices.Add(newVertice);
            index = vertices.Count - 1;
        }

        return index;
    } 

    private List<Vector3> GetDefaultVertices()
    {
        float short_side = 1f / 2f * _radius;
        float long_side = GetGoldenRectangleSideLength() / 2 * _radius;

        return new List<Vector3>
                {
                    new Vector3(-short_side, -long_side, 0f).normalized,
                    new Vector3(short_side, -long_side, 0f).normalized,
                    new Vector3(-short_side, long_side, 0f).normalized,
                    new Vector3(short_side, long_side, 0f).normalized,

                    new Vector3(0f, -short_side, -long_side).normalized,
                    new Vector3(0f, -short_side, long_side).normalized,
                    new Vector3(0f, short_side, -long_side).normalized,
                    new Vector3(0f, short_side, long_side).normalized,

                    new Vector3(-short_side, 0f, -long_side).normalized,
                    new Vector3(short_side, 0f, -long_side).normalized,
                    new Vector3(-short_side, 0f, long_side).normalized,
                    new Vector3(short_side, 0f, long_side).normalized
                };

    }

    private List<Triangle> GetDefaultTriangles()
    {
        return new List<Triangle>
        {
            new Triangle(8, 2, 6),
            new Triangle(8, 6, 4),
            new Triangle(0, 8, 4),
            new Triangle(6, 2, 3),
            new Triangle(6, 3, 9),
            new Triangle(4, 6, 9),
            new Triangle(1, 4, 9),
            new Triangle(0, 4, 1),
            new Triangle(10, 2, 8),
            new Triangle(0, 10, 8),
            new Triangle(9, 3, 11),
            new Triangle(1, 9, 11),
            new Triangle(10, 7, 2),
            new Triangle(7, 3, 2),
            new Triangle(11, 3, 7),
            new Triangle(5, 7, 10),
            new Triangle(0, 5, 10),
            new Triangle(5, 11, 7),
            new Triangle(1, 11, 5),
            new Triangle(1, 5, 0)
        };
    }

    private int GetVerticeCount(int divideTo)
    {
        return ((divideTo + 1) * (divideTo + 2)) / 2;
    }
    private float GetGoldenRectangleSideLength()
    {
        return (1 + Mathf.Sqrt(5)) / 2;
    }
    private Mesh InitializeNewMesh(string name)
    {
        Mesh mesh = new Mesh
        {
            name = name
        };

        return mesh;
    }
    private int GenerateNewSeed()
    {
        int seed = _seed;

        if (!string.IsNullOrEmpty(_seedString))
        {
            seed = _seedString.GetHashCode();
        }
        else
        {
            seed = Random.Range(0, 2147483647);
        }

        return seed;
    }

    private int GetNewSeed(bool overwrite)
    {
        int seed = _seed;

        if (!overwrite && seed != 0)
            return seed;

        seed = GenerateNewSeed();
        return seed;
    }

}
