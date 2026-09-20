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

        // 삼각형 8개: 위쪽 4개(apex=0) + 아래쪽 4개(apex=1)
        // 위/아래는 바깥쪽 방향이 반대라서 winding order도 서로 반대로 감아야 함
        int[] triangles = new int[]
        {
            // 위쪽 피라미드 (apex: 0번)
            0, 3, 2,
            0, 4, 3,
            0, 5, 4,
            0, 2, 5,

            // 아래쪽 피라미드 (apex: 1번, 위쪽과 반대 순서)
            1, 2, 3,
            1, 3, 4,
            1, 4, 5,
            1, 5, 2,
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.name = "CustomDiamond";

        GetComponent<MeshFilter>().mesh = mesh;

        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.color = new Color(0.6f, 0.85f, 1f); // 연한 하늘색으로 마무리
        GetComponent<MeshRenderer>().sharedMaterial = mat;
    }
}
