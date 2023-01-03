
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SimpleProceduralMesh : MonoBehaviour
{
    [SerializeField] string meshName = "Procedural Mesh";
    private MeshFilter _meshFilter = null;

    private void OnEnable()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.mesh = GenerateMesh();
    }


    private Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh
        {
            name = meshName
        };

        mesh.vertices = new Vector3[]
        {
            Vector3.zero, Vector3.right, Vector3.up,
            new Vector3(1f, 1f)
        };

        mesh.normals = new Vector3[]
        {
            Vector3.back, Vector3.back, Vector3.back,
            Vector3.back
        };

        mesh.tangents = new Vector4[]
        {
            new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f, 0f, 0f, -1f)
        };

        mesh.uv = new Vector2[]
        {
            Vector2.zero, Vector2.zero, Vector2.zero,
            Vector2.zero
        };

        mesh.triangles = new int[]
        {
            0,2,1,1,2,3
        };

        return mesh;
    }

}
