using UnityEngine;

[ExecuteAlways]
[RequireComponent(
    typeof(MeshFilter),
    typeof(MeshRenderer)
)]
public class S12_StillLifeMesh : MonoBehaviour
{
    public enum Shape
    {
        Triangle,
        Diamond,
        Pyramid
    }

    [SerializeField]
    private Shape shape = Shape.Triangle;

    private void Reset()
    {
        BuildMesh();
    }

    private void OnValidate()
    {
        BuildMesh();
    }

    private void Awake()
    {
        BuildMesh();
    }

    private void BuildMesh()
    {
        MeshFilter meshFilter =
            GetComponent<MeshFilter>();

        if (meshFilter == null)
            return;

        Mesh mesh = meshFilter.sharedMesh;

        if (mesh == null ||
            !mesh.name.StartsWith("S12_"))
        {
            mesh = new Mesh();
            meshFilter.sharedMesh = mesh;
        }

        mesh.Clear();

        switch (shape)
        {
            case Shape.Triangle:
                BuildTriangle(mesh);
                break;

            case Shape.Diamond:
                BuildDiamond(mesh);
                break;

            case Shape.Pyramid:
                BuildPyramid(mesh);
                break;
        }

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private void BuildTriangle(Mesh mesh)
    {
        mesh.name = "S12_TriangleMesh";

        mesh.vertices = new Vector3[]
        {
            new Vector3(-0.6f, -0.5f, -0.35f),
            new Vector3( 0.6f, -0.5f, -0.35f),
            new Vector3( 0.0f,  0.6f, -0.35f),

            new Vector3(-0.6f, -0.5f,  0.35f),
            new Vector3( 0.6f, -0.5f,  0.35f),
            new Vector3( 0.0f,  0.6f,  0.35f)
        };

        mesh.triangles = new int[]
        {
            0, 2, 1,
            3, 4, 5,

            0, 1, 4,
            0, 4, 3,

            1, 2, 5,
            1, 5, 4,

            2, 0, 3,
            2, 3, 5
        };
    }

    private void BuildDiamond(Mesh mesh)
    {
        mesh.name = "S12_DiamondMesh";

        mesh.vertices = new Vector3[]
        {
            new Vector3(0f,  0.7f,  0f),
            new Vector3(0f, -0.7f,  0f),
            new Vector3(0f,  0f, -0.55f),
            new Vector3(0.55f, 0f, 0f),
            new Vector3(0f, 0f, 0.55f),
            new Vector3(-0.55f, 0f, 0f)
        };

        mesh.triangles = new int[]
        {
            0, 3, 2,
            0, 4, 3,
            0, 5, 4,
            0, 2, 5,

            1, 2, 3,
            1, 3, 4,
            1, 4, 5,
            1, 5, 2
        };
    }

    private void BuildPyramid(Mesh mesh)
    {
        mesh.name = "S12_PyramidMesh";

        mesh.vertices = new Vector3[]
        {
            new Vector3(0f, 0.7f, 0f),
            new Vector3(-0.55f, -0.5f, -0.55f),
            new Vector3(0.55f, -0.5f, -0.55f),
            new Vector3(0.55f, -0.5f, 0.55f),
            new Vector3(-0.55f, -0.5f, 0.55f)
        };

        mesh.triangles = new int[]
        {
            0, 2, 1,
            0, 3, 2,
            0, 4, 3,
            0, 1, 4,

            1, 2, 3,
            1, 3, 4
        };
    }
}