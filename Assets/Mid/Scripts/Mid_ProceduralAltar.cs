using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Mid_ProceduralAltar : MonoBehaviour
{
    public int sides = 12;
    public float radius = 2f;
    public float height = 1.5f;

    private void Start()
    {
        MakeMesh();
    }

    private void OnValidate()
    {
        MakeMesh();
    }

    private void MakeMesh()
    {
        sides = Mathf.Max(3, sides);

        Vector3[] vertices = new Vector3[sides * 2 + 2];
        int[] triangles = new int[sides * 12];

        int bottomCenter = sides * 2;
        int topCenter = bottomCenter + 1;

        vertices[bottomCenter] = Vector3.zero;
        vertices[topCenter] = new Vector3(0f, height, 0f);

        for (int i = 0; i < sides; i++)
        {
            float angle = Mathf.PI * 2f * i / sides;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            vertices[i] = new Vector3(x, 0f, z);
            vertices[i + sides] = new Vector3(x, height, z);
        }

        for (int i = 0; i < sides; i++)
        {
            int next = (i + 1) % sides;
            int index = i * 12;

            triangles[index] = i;
            triangles[index + 1] = i + sides;
            triangles[index + 2] = next + sides;

            triangles[index + 3] = i;
            triangles[index + 4] = next + sides;
            triangles[index + 5] = next;

            triangles[index + 6] = bottomCenter;
            triangles[index + 7] = i;
            triangles[index + 8] = next;

            triangles[index + 9] = topCenter;
            triangles[index + 10] = next + sides;
            triangles[index + 11] = i + sides;
        }

        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

        if (mesh == null || mesh.name != "Mid_Altar")
        {
            mesh = new Mesh();
            mesh.name = "Mid_Altar";
            GetComponent<MeshFilter>().sharedMesh = mesh;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
