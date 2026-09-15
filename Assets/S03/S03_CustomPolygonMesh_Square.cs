using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S03_CustomPolygonMesh_Square : MonoBehaviour
{
    void Start()
    {
        // 정점 5개 좌표 (정오각형, 반지름 1)
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0f, 1f, 0f),                     // 0: 90도
            new Vector3(0.9511f, 0.309f, 0f),             // 1: 18도
            new Vector3(0.5878f, -0.809f, 0f),            // 2: -54도
            new Vector3(-0.5878f, -0.809f, 0f),           // 3: -126도
            new Vector3(-0.9511f, 0.309f, 0f),            // 4: -198도(=162도)
        };

        // TODO 2: 정점 3개씩 묶어 삼각형들을 구성하세요
        int[] triangles = new int[]
        {
            // 예: 0, 1, 2,
            // …
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().sharedMaterial =
            new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }
}
