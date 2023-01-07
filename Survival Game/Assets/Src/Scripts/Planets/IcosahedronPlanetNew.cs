using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class IcosahedronPlanetNew : MonoBehaviour
{
    [Header("Generation seed")]
    [SerializeField]
    private bool _generateSeedFromString = true;
    [SerializeField]
    private string _seedString = "";
    [SerializeField, ReadOnly]
    private int _seed;

    [Header("Generation size and resolution")]
    [SerializeField, Range(0.1f, 1000f)]
    private float _radius = 1;
    [SerializeField, Range(1, 10)]
    private float _meshDivides;
    [SerializeField, ReadOnly]
    private int _divideToXTriangles = 4;
    [SerializeField, ReadOnly]
    private int _triangleCount;

    [Header("Mesh")]
    [SerializeField]
    private string _meshName;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Mesh _mesh;

    [Header("Planet Physics")]
    private float _gravity = -9.81f;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _seed = GetNewSeed(_generateSeedFromString);
        _mesh = InitializeNewMesh(_meshName);
        _meshFilter.mesh = _mesh;
        GenerateMesh();
        //_mesh.colors32 = ApplyRandomColorToEachTriangle();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //DivideTriangles();
        GenerateMesh();
        for (int i = 0; i < _mesh.vertices.Length; i++)
        {
            //Vector3 norm = transform.TransformDirection(_mesh.normals[i]);
            //Vector3 vert = transform.TransformPoint(_mesh.vertices[i]);
            //Debug.DrawRay(vert, norm * 2f, Color.red);
        }
    }

    private void OnDrawGizmos()
    {
        if (_meshFilter != null)
        {
            Gizmos.color = Color.blue;
            foreach (var vert in _meshFilter.sharedMesh.vertices)
            {
                Gizmos.DrawCube(transform.TransformPoint(vert), new Vector3(0.03f, 0.03f, 0.03f) * _radius);
            }
        }
    }

    private Mesh InitializeNewMesh(string name)
    {
        Mesh mesh = new Mesh
        {
            name = name
        };

        return mesh;
    }

    private void GenerateMesh()
    {
        _mesh.vertices = GenerateDefaultVertices();
        //_mesh.triangles = GenerateDefaultTriangles();
        //_mesh.normals = GenerateDefaultNormals();
        //_mesh.uv = GenerateDefaultUV();
        //_mesh.colors32 = ApplyRandomColorToEachTriangle();
    }
    private float GetGoldenRectangleSideLength()
    {
        return  (1 + Mathf.Sqrt(5)) / 2;
    }

    private Vector3[] GenerateVertices(int divided)
    {

        float short_side = 1f / 2f * _radius;
        float long_side = GetGoldenRectangleSideLength() / 2 * _radius;

        int triangleVerticesCount = 3;

        int verticesCount = 12 + (12 * 4 * divided);

        Vector3[] vertices = new Vector3[verticesCount];

        //DefaultVertices


        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(0f, 0f, 0f);
            
        }

        return vertices;
    }

    private Vector3[] GenerateDefaultVertices()
    {
        float default_side = 1f / 2f * _radius; // one side is 1f times radius long. We need to divide it by two to center it.
        float goldenRectangle = (1 + Mathf.Sqrt(5)) / 4 * _radius; // one side is (1 + Mathf.Sqrt(5)) / 2 times radius, but we need to divide it by two to center it.

        Vector3[] vertices = new Vector3[12]
        { //2, 3, 6
            //plane y-x 
            new Vector3(-default_side, -goldenRectangle, 0f),
            new Vector3(default_side, -goldenRectangle, 0f),
            new Vector3(-default_side, goldenRectangle, 0f),
            new Vector3(default_side, goldenRectangle, 0f),

            //plane y-z
            new Vector3(0f, -default_side, -goldenRectangle),
            new Vector3(0f, -default_side, goldenRectangle),
            new Vector3(0f, default_side, -goldenRectangle),
            new Vector3(0f, default_side, goldenRectangle),
            
            //plane z-x
            new Vector3(-goldenRectangle, 0f, -default_side),
            new Vector3(goldenRectangle, 0f, -default_side),
            new Vector3(-goldenRectangle, 0f, default_side),
            new Vector3(goldenRectangle, 0f , default_side)

        };
        return vertices;
    }
    /*
    private Vector3[] GenerateDefaultVertices()
    {

        float default_side = 1f / 2f * _radius; // one side is 1f times radius long. We need to divide it by two to center it.
        float goldenRectangle = (1 + Mathf.Sqrt(5)) / 4 * _radius; // one side is (1 + Mathf.Sqrt(5)) / 2 times radius, but we need to divide it by two to center it.
        Vector3[] vertices = new Vector3[12]
        {
            //plane y-x 
            new Vector3(-default_side, -goldenRectangle, 0f),
            new Vector3(default_side, -goldenRectangle, 0f),
            new Vector3(-default_side, goldenRectangle, 0f),
            new Vector3(default_side, goldenRectangle, 0f),

            //plane y-z
            new Vector3(0f, -default_side, -goldenRectangle),
            new Vector3(0f, -default_side, goldenRectangle),
            new Vector3(0f, default_side, -goldenRectangle),
            new Vector3(0f, default_side, goldenRectangle),
            
            //plane z-x
            new Vector3(-goldenRectangle, 0f, -default_side),
            new Vector3(goldenRectangle, 0f, -default_side),
            new Vector3(-goldenRectangle, 0f, default_side),
            new Vector3(goldenRectangle, 0f , default_side)

        };
        return vertices;
    }*/

    private int[] GenerateDefaultTriangles()
    {
        int[] triangles = new int[]
        {
            // Golen rectangle planes to get each vertec index.
            /*
            //plane y-x
            0, 2, 1,
            1, 2, 0,
            2, 3, 1,
            1, 3, 2,

            //plane y-z
            4, 6, 5,
            5, 6, 4,
            6, 7, 5,
            5, 7, 6,

            //plane z-x
            8, 10, 9,
            9, 10, 8,
            10, 11, 9,
            9, 11, 10,
            */
            
            // IcosahedronPlanet, 20 outside faces
            8, 2, 6,
            8, 6, 4,
            0, 8, 4,
            6, 2, 3,
            6, 3, 9,
            4, 6, 9,
            1, 4, 9,
            0, 4, 1,
            10, 2, 8,
            0, 10, 8,
            9, 3, 11,
            1, 9, 11,
            10, 7, 2,
            7, 3, 2,
            11, 3, 7,
            5, 7, 10,
            0, 5, 10,
            5, 11, 7,
            1, 11, 5,
            1, 5, 0,
            
            // IcosahedronPlanet, 20 inside faces
           /*6, 2, 8,
            4, 6, 8,
            4, 8, 0,
            3, 2, 6,
            9, 3, 6,
            9, 6, 4,
            9, 4, 1,
            1, 4, 0,
            8, 2, 10,
            8, 10, 0,
            11, 3, 9,
            11, 9, 1,
            2, 7, 10,
            2, 3, 7,
            7, 3, 11,
            10, 7, 5,
            10, 5, 0,
            7, 11, 5,
            5, 11, 1,
            0, 5, 1*/
        };

        return triangles;
    }

    private Vector3[] GenerateDefaultNormals()
    {
        Vector3[] normals = new Vector3[12]
        { //fix normals!
            //plane y-x
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            
            //plane y-z
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,

            //plane y-z
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward

        };

        return normals;
    }

    private Vector2[] GenerateDefaultUV()
    {
        Vector2[] uv = new Vector2[12]
        {
            //plane y-x
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(1,1),
            
            //plane y-z
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(1,1),

            //plane y-z
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(1,1),

        };

        return uv;
    }

    private Color32[] ApplyRandomColorToEachTriangle()
    {
        Color32[] colors = new Color32[_mesh.vertices.Length];

        for (int i = 0; i < colors.Length; i += 3)
        {
            Debug.Log($"i0: {i}/{_mesh.triangles[i + 0]}, i1: {i + 1}/{_mesh.triangles[i + 1]}, i2: {i + 2}/{_mesh.triangles[i + 2]}");
            Color32 randomColor = GetRandomColor();
            colors[_mesh.triangles[i + 0]] = randomColor;
            colors[_mesh.triangles[i + 1]] = randomColor;
            colors[_mesh.triangles[i + 2]] = randomColor;
        }

        return colors;
    }

    private void DivideTriangles()
    {
        Vector3 a = _mesh.vertices[_mesh.triangles[0]];
        Vector3 b = _mesh.vertices[_mesh.triangles[1]];
        Vector3 c = _mesh.vertices[_mesh.triangles[2]];

        Vector3 ab = b - a;
        Vector3 bc = c - b;
        Vector3 ca = a - c;


        Vector3[] vertices = new Vector3[]
        {
            a, b, c, ab, bc, ca
        };
        _mesh.vertices = vertices;

        int[] triangles = new int[]
        {
            3,2,4,
            5,3,4,
            0,3,5,
            5,4,1,
        };
        _mesh.triangles = triangles;
    }

    private int GetTriangleCount()
    {
        int count = 1;

        for (int i = 0; i < _meshDivides; i++)
        {
            count *= _divideToXTriangles;
        }

        return count;
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

    private Color32 GetRandomColor()
    {
        return new Color(Random.Range(0, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
    }
}
