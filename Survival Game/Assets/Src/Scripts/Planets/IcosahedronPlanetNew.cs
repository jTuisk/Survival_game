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
    private int _meshResolution = 1;
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


    //remove later

    private Vector3[] tempVertices;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _seed = GetNewSeed(_generateSeedFromString);
        _mesh = InitializeNewMesh(_meshName);
        _meshFilter.mesh = _mesh;
        GenerateMesh();
        //_mesh.colors32 = ApplyRandomColorToEachTriangle();
        tempVertices = GetDefaultVertices();
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
            Gizmos.color = Color.cyan;
            foreach (var vert in tempVertices)
            {
                Gizmos.DrawCube(transform.TransformPoint(vert), new Vector3(0.03f, 0.03f, 0.02f) * _radius);
            }

            Gizmos.color = Color.blue;
            foreach (var vert in _meshFilter.sharedMesh.vertices)
            {
                if (vert == Vector3.zero)
                    continue;

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
        _mesh.vertices = GenerateVertices(_meshResolution);
        //_mesh.vertices = GenerateDefaultVertices();
        //_mesh.triangles = GenerateDefaultTriangles();
        //_mesh.normals = GenerateDefaultNormals();
        //_mesh.uv = GenerateDefaultUV();
        //_mesh.colors32 = ApplyRandomColorToEachTriangle();
    }
    private float GetGoldenRectangleSideLength()
    {
        return  (1 + Mathf.Sqrt(5)) / 2;
    }

    private Vector3[] GenerateVertices(int divideTo)
    {

        int verticesCount = 12 + (12 * 4 * divideTo);

        Vector3[] defaultVertices = GetDefaultVertices(); // Ei haluta vertices listaan näitä.
        Vector3[] vertices = new Vector3[1000]; // pitää laskea verticeCount

        int[,] defaultTriangles = new int[20,3]
        {
            {2, 3, 6},
            {8, 6, 4},
            {0, 8, 4},
            {6, 2, 3},
            {6, 3, 9},
            {4, 6, 9},
            {1, 4, 9},
            {0, 4, 1},
            {10, 2, 8},
            {0, 10, 8},
            {9, 3, 11},
            {1, 9, 11},
            {10, 7, 2},
            {7, 3, 2},
            {11, 3, 7},
            {5, 7, 10},
            {0, 5, 10},
            {5, 11, 7},
            {1, 11, 5},
            {1, 5, 0}
        };

        //TODO FIX THIS
        //for (int i = 0; i < defaultTriangles.GetLength(0); i++) 
        for (int i = 0; i < 2; i++)
        {
            Debug.Log($"{i} - a: {defaultTriangles[i, 0]}, b: {defaultTriangles[i, 1]}, c: {defaultTriangles[i, 2]} ");
            int startIndex = (200*i); //recalculate
            DevideVerticleGroup(vertices, startIndex, vertices[defaultTriangles[i ,0]], vertices[defaultTriangles[i, 1]], vertices[defaultTriangles[i, 2]], divideTo);
        }
        //DevideVerticleGroup(vertices, 12, defaultVertices[6], defaultVertices[2], defaultVertices[3], divideTo);
        //DevideVerticleGroup(vertices, 500, defaultVertices[8], defaultVertices[6], defaultVertices[4], divideTo);

        //DefaultVertices

        //Debug.Log($"Vertices:  {vertices.Length}");

        return vertices;
    }

    private int GetVerticeCount(int diviedTo)
    {
        //https://colab.research.google.com/drive/1IFV_kIQH17ZFDrOnFy6r_9rlkYWvjylJ#scrollTo=KXpzIWHx3kUO
        int count = 0;

        return count;
    }

    private Vector3[] GetDefaultVertices()
    {
        float short_side = 1f / 2f * _radius;
        float long_side = GetGoldenRectangleSideLength() / 2 * _radius;

        Vector3[] vertices = new Vector3[12];

        //plane y-x
        vertices[0] = new Vector3(-short_side, -long_side, 0f);
        vertices[1] = new Vector3(short_side, -long_side, 0f);
        vertices[2] = new Vector3(-short_side, long_side, 0f);
        vertices[3] = new Vector3(short_side, long_side, 0f);

        //plane y-z
        vertices[4] = new Vector3(0f, -short_side, -long_side);
        vertices[5] = new Vector3(0f, -short_side, long_side);
        vertices[6] = new Vector3(0f, short_side, -long_side);
        vertices[7] = new Vector3(0f, short_side, long_side);

           //plane z-x
        vertices[8] = new Vector3(-short_side, 0f, -long_side);
        vertices[9] = new Vector3(short_side, 0f, -long_side);
        vertices[10] = new Vector3(-short_side, 0f, long_side);
        vertices[11] = new Vector3(short_side, 0f, long_side);

        return vertices;
    }

    private void DevideVerticleGroup(Vector3[] vertices, int startIndex, Vector3 a, Vector3 b, Vector3 c, int divideTo)
    {
        divideTo = divideTo < 1 ? 1 : divideTo;
        /*
        * https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html
        * Lerp(Start value, End value, Interpolate)
        * We get interpolate value dividing 1 by divideTo * i
        * Where 1 divided by divideTo gives us % value of and then we multiplie it by i.
        */

        for (int i = 0; i < divideTo; i++)
        {
            vertices[startIndex + (i * 3) + 0] = Vector3.Lerp(a, b, (1f / divideTo * i));
            vertices[startIndex + (i * 3) + 1] = Vector3.Lerp(b, c, (1f / divideTo * i));
            vertices[startIndex + (i * 3) + 2] = Vector3.Lerp(c, a, (1f / divideTo * i));
        }

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

        for (int i = 0; i < _meshResolution; i++)
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
