using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class IcosahedronPlanet : MonoBehaviour
{

    [HideInInspector]
    public bool ShapeSettingsfoldout;
    [HideInInspector]
    public bool ColorSettingsfoldout;
    [HideInInspector]
    public bool NatureSettingsfoldout;

    [SerializeField]
    private bool _DebugMode = false;
    [Header("Generation seed")]
    [SerializeField]
    private bool _generateSeedFromString = false;
    [SerializeField]
    private string _seedString = "";
    [SerializeField, ReadOnly]
    private int _seed;

    [Header("Mesh")]
    [SerializeField]
    private bool _updateMessEveryFrame = true;
    [SerializeField]
    private string _meshName;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;
    private Mesh _mesh;
    [SerializeField, ReadOnly]
    private int _verticeCount = 0;

    PlanetShapeGenerator _shapeGenerator;
    PlanetColorGenerator _colorGenerator;

    public PlanetShapeSettings shapeSettings;
    public PlanetColorSettings colorSettings;
    public PlanetNatureSettings natureSettings;

    private void OnValidate()
    {
        _seed = GetNewSeed(_generateSeedFromString);
        _shapeGenerator = new PlanetShapeGenerator();
        _shapeGenerator.UpdateSettings(shapeSettings, _seed);
        _colorGenerator = new PlanetColorGenerator();
        _colorGenerator.UpdateSettings(colorSettings, _seed);
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();
        _mesh = InitializeNewMesh(_meshName);
        _meshFilter.mesh = _mesh;
        GenerateMesh();
        InitializeMeshCollider();
    }

    private void FixedUpdate()
    {
        if(_updateMessEveryFrame)
            GenerateMesh();
        ShowNormals(_DebugMode);
    }

    private void InitializeMeshCollider()
    {
        _meshCollider.sharedMesh = _mesh;
    }

    private void ShowNormals(bool show)
    {
        if (show)
        {
            for (int i = 0; i < _mesh.vertices.Length; i++)
            {
                Vector3 norm = transform.TransformDirection(_mesh.normals[i]);
                Vector3 vert = transform.TransformPoint(_mesh.vertices[i]);
                Debug.DrawRay(vert, norm * 0.1f * shapeSettings.radius, Color.red);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(_meshFilter != null)
        {
            if (!_DebugMode)
                return;

            List<Vector3> verts = GetDefaultVertices();
            for (int i = 0; i < verts.Count; i++)
            {
                if(i == 0)
                    Gizmos.color = Color.red;
                if (i == 4)
                    Gizmos.color = Color.blue;
                if (i == 8)
                    Gizmos.color = Color.green;
                Gizmos.DrawCube(transform.TransformPoint(verts[i]), new Vector3(0.03f, 0.03f, 0.03f) * shapeSettings.radius);
            }

            Gizmos.color = Color.black;
            foreach (var vert in _meshFilter.sharedMesh.vertices)
            {
                Gizmos.DrawCube(transform.TransformPoint(vert), new Vector3(0.015f, 0.015f, 0.015f)* shapeSettings.radius);
            }
        }
    }

    private void GenerateMesh()
    {
        List<Vector3> vertices = GetDefaultVertices();
        List<Triangle> triangles = GetDefaultTriangles();
        List<Vector3> normals = GetDefaultNormals();

        triangles = Subdivide(vertices, triangles, normals, shapeSettings.resolution);

        _shapeGenerator.GenerateShape(ref vertices);

        _mesh.vertices = vertices.ToArray();
        _mesh.triangles = ConvertTrianglesToArray(triangles);
        _mesh.normals = normals.ToArray();

        GenerateColors();

        _verticeCount = _mesh.vertices.Length;
        _colorGenerator.UpdateElevation(_shapeGenerator.MinMax);
        //UpdateUVs(_colorGenerator);
    }

    private List<Triangle> Subdivide(List<Vector3> vertices, List<Triangle> triangles, List<Vector3> normals, uint divides)
    {
        /**
         * Total count for vertices.
         * [divides, verticeCount]
         * [0, 12], [1, 42], [2, 162], [3, 642], [4, 2562], [5, 10242] 
         */
        for (int i = 0; i < divides; i++)
        {
            List<Triangle> newTriangles = new List<Triangle>();

            for (int j = 0; j < triangles.Count; j++)
            {
                Triangle currentFace = triangles[j];


                Vector3 a = vertices[currentFace.a];
                Vector3 b = vertices[currentFace.b];
                Vector3 c = vertices[currentFace.c];

                
                Vector3 ab = _shapeGenerator.CalculatePointOnPlanet(Vector3.Lerp(a, b, 0.5f)).normalized;
                Vector3 bc = _shapeGenerator.CalculatePointOnPlanet(Vector3.Lerp(b, c, 0.5f)).normalized;
                Vector3 ca = _shapeGenerator.CalculatePointOnPlanet(Vector3.Lerp(c, a, 0.5f)).normalized;

                int ab_index = AddAndGetVerticeIndex(vertices, normals, ab);
                int bc_index = AddAndGetVerticeIndex(vertices, normals, bc);
                int ca_index = AddAndGetVerticeIndex(vertices, normals, ca);

                newTriangles.Add(new Triangle(vertices.IndexOf(a), ab_index, ca_index));
                newTriangles.Add(new Triangle(vertices.IndexOf(b), bc_index, ab_index));
                newTriangles.Add(new Triangle(vertices.IndexOf(c), ca_index, bc_index));
                newTriangles.Add(new Triangle(ab_index, bc_index, ca_index));
            }
            triangles = newTriangles;
        }
        return triangles;
    }

    private int AddAndGetVerticeIndex(List<Vector3> vertices, List<Vector3> normals, Vector3 newVertice)
    {
        int index = vertices.IndexOf(newVertice);

        if (index == -1)
        {
            vertices.Add(newVertice);
            normals.Add(newVertice);
            index = vertices.Count - 1;
        }

        return index;
    }

    private List<Vector3> GetDefaultVertices()
    {
        float short_side = 1f / 2f;
        float long_side = GetGoldenRectangleSideLength() / 2;
        
        return new List<Vector3>
                {
                    //plane y-x
                    //red
                    new Vector3(-long_side, -short_side, 0f).normalized,   // 0
                    new Vector3(long_side, -short_side, 0f).normalized,    // 1
                    new Vector3(-long_side, short_side, 0f).normalized,    // 2
                    new Vector3(long_side, short_side, 0f).normalized,     // 3
                    
                    //plane y-z
                    //blue
                    new Vector3(0f, -long_side, -short_side).normalized,   // 4
                    new Vector3(0f, -long_side, short_side).normalized,    // 5
                    new Vector3(0f, long_side, -short_side).normalized,    // 6
                    new Vector3(0f, long_side, short_side).normalized,     // 7
                    
                    //plane z-x
                    //green
                    new Vector3(-short_side, 0f, -long_side).normalized,   // 8
                    new Vector3(short_side, 0f, -long_side).normalized,    // 9
                    new Vector3(-short_side, 0f, long_side).normalized,    // 10
                    new Vector3(short_side, 0f, long_side).normalized     // 11
                };
            }

    private List<Triangle> GetDefaultTriangles()
    {
        return new List<Triangle>
        {
            new Triangle(2, 7, 6),
            new Triangle(8, 2, 6),
            new Triangle(8, 6, 9),
            new Triangle(0, 8, 4),
            new Triangle(4, 8, 9),
            new Triangle(4, 9, 1),
            new Triangle(9, 6, 3),
            new Triangle(3, 6, 7),
            new Triangle(3, 7, 11),
            new Triangle(11, 7, 10),
            new Triangle(10, 7, 2),
            new Triangle(0, 10, 2),
            new Triangle(0, 2, 8),
            new Triangle(1, 9, 3),
            new Triangle(1, 3, 11),
            new Triangle(1, 5, 4),
            new Triangle(0, 4, 5),
            new Triangle(5, 1, 11),
            new Triangle(5, 11, 10),
            new Triangle(5, 10, 0)
        };
    }

    private List<Vector3> GetDefaultNormals()
    {
        return GetDefaultVertices();
    }

    private int[] ConvertTrianglesToArray(List<Triangle> triangles)
    {
        int[] int_Triangles = new int[triangles.Count*3];


        for(int i = 0; i < triangles.Count; i++)
        {
            int[] points = triangles[i].ToArray();
            int_Triangles[i*3 + 0] = points[0];
            int_Triangles[i*3 + 1] = points[1];
            int_Triangles[i*3 + 2] = points[2];
        }

        return int_Triangles;
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

        if(_shapeGenerator != null)
        {
            _shapeGenerator.SetSeed(seed);
        }

        return seed;
    }

    public Vector3 GetRandomSurfacePoint()
    {
        //REDO

        Vector3 surfacePoint = new Vector3();
        Vector3 direction = Random.onUnitSphere * shapeSettings.radius;
        surfacePoint = (direction - transform.position).normalized;
        return surfacePoint;
    }


    private void GenerateColors()
    {
        //_meshRenderer.sharedMaterial = colorSettings.planetMaterial;
        _colorGenerator.UpdateColors();
    }

    /*public void UpdateUVs(PlanetColorGenerator colorGenerator)
    {
        Vector2[] uv = new Vector2[_verticeCount];
        int resolution = _verticeCount / 2;
        
        for (int i = 0; i < _mesh.vertices.Length; i++)
        {
            Vector2 percent = new Vector2(_mesh.vertices[i].x, _mesh.vertices[i].y) / (resolution -1);
            uv[i] = new Vector2(colorGenerator.BiomePercentFromPoint(_mesh.vertices[i].normalized), 0);

        }
        _mesh.uv = uv;
    }*/


    public void GeneratePlanet()
    {
        Debug.Log("GeneratePlanet");
        _mesh.Clear();

        GenerateMesh();
    }

    public void OnShapeSettingsUpdated()
    {
        Debug.Log("OnShapeSettingsUpdated");
        _shapeGenerator.ResetMinMax();
        _mesh.Clear();
        GenerateMesh();
    }

    public void OnColorSettingsUpdated()
    {
        GenerateColors();
        //UpdateUVs(_colorGenerator);
    }
    public void OnNatureSettingsUpdated()
    {
        Debug.Log("OnNatureSettingsUpdated");
    }
}
