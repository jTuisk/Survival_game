using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class AdvancedMultiStreamProceduralMesh : MonoBehaviour
{
    [SerializeField] string meshName;

    private MeshFilter _meshFilter;
    private Mesh _generatedMesh;
    private Mesh.MeshDataArray _meshDataArray;

    private void OnEnable()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _generatedMesh = GenerateMesh();
        Mesh.ApplyAndDisposeWritableMeshData(_meshDataArray, _generatedMesh);
        _meshFilter.mesh = _generatedMesh;
    }
    //https://catlikecoding.com/unity/tutorials/procedural-meshes/creating-a-mesh/
    private Mesh GenerateMesh()
    {
        int vertexAtrributeCount = 4;
        int vertexCount = 4;

        _meshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData meshData = _meshDataArray[0];

        NativeArray<VertexAttributeDescriptor> vertexAttributes = new NativeArray<VertexAttributeDescriptor>
            (
                vertexAtrributeCount, Allocator.Temp
            );
        vertexAttributes[0] = new VertexAttributeDescriptor(dimension: 3);


        meshData.SetVertexBufferParams(vertexCount, vertexAttributes);
        vertexAttributes.Dispose();

        Mesh mesh = new Mesh
        {
            name = meshName
        };

        return mesh;
    }
}
