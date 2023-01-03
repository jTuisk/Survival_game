using ProceduralMeshes;
using ProceduralMeshes.Generators;
using ProceduralMeshes.Streams;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralMesh : MonoBehaviour
{
    [SerializeField] string meshName;
    private Mesh _mesh;
    private MeshFilter _meshFilter;

    [SerializeField, Range(1, 10)]
    int resolution = 1;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        InitializeMesh();
        _meshFilter.mesh = _mesh;
    }

    void OnValidate() => enabled = true;

    void Update()
    {
        GenerateMesh();
        enabled = false;
    }

    private void InitializeMesh()
    {
        _mesh = new Mesh
        {
            name = meshName
        };
    }
    private void GenerateMesh()
    {
        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData meshData = meshDataArray[0];

        MeshJob<SquareGrid, MultiStream>.ScheduleParallel(
            _mesh, meshData, resolution, default    
        ).Complete();

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, _mesh);
    }
    void AGenerateMesh()
    {
        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData meshData = meshDataArray[0];

        MeshJob<SquareGrid, SingleStream>.ScheduleParallel(
            _mesh, meshData, resolution, default
        ).Complete();
        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, _mesh);
    }
}