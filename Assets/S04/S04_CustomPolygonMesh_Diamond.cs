using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S04_CustomPolygonMesh_Diamond : MonoBehaviour
{
    void Start()
    {
        // 0: (0,0,0)  1: (1,0,0)  2: (1,1,0)  3: (0,1,0)
        // 4: (0,0,1)  5: (1,0,1)  6: (1,1,1)  7: (0,1,1)

        // 다이아몬드 정점 6개: 위 꼭짓점 1 + 허리띠(belt) 4 + 아래 꼭짓점 1
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0.5f, 1f, 0.5f),   // 0: 위 꼭짓점
            new Vector3(0.5f, 0f, 0.5f),   // 1: 아래 꼭짓점
            new Vector3(0f,   0.5f, 0f),   // 2: 허리띠 - 뒤 왼쪽
            new Vector3(1f,   0.5f, 0f),   // 3: 허리띠 - 뒤 오른쪽
            new Vector3(1f,   0.5f, 1f),   // 4: 허리띠 - 앞 오른쪽
            new Vector3(0f,   0.5f, 1f),   // 5: 허리띠 - 앞 왼쪽
        };

        // TODO: 정점 3개씩 묶어 삼각형 8개를 구성하세요 (winding order 주의)
        int[] triangles = new int[]
        {
            // 예: 0, 3, 2,
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
