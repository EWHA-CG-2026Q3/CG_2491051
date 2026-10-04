using UnityEngine;

[DefaultExecutionOrder(100)]
[RequireComponent(typeof(MeshFilter))]
public class S09_RotationX : MonoBehaviour
{
    [Header("Raw X-axis Rotation")]
    [SerializeField]
    private float angleDegrees = 30f;

    private Mesh rotatedMesh;

    private void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshFilter.sharedMesh == null)
        {
            Debug.LogError(
                "MeshFilter에 Mesh가 없습니다. " +
                "DiamondMesh가 먼저 Mesh를 만들도록 확인하세요.",
                this
            );

            return;
        }

        // 다른 비교 오브젝트의 Mesh가 함께 수정되지 않도록
        // 현재 오브젝트만 사용할 Mesh를 복제한다.
        rotatedMesh = Instantiate(meshFilter.sharedMesh);
        rotatedMesh.name = "S09_RotationX_Raw_Mesh";

        meshFilter.sharedMesh = rotatedMesh;

        ApplyRotationXRaw();
    }

    public float[,] RotationXMatrixRaw(float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float c = Mathf.Cos(rad);
        float s = Mathf.Sin(rad);

        // x축은 그대로 두고 y축과 z축만 회전한다.
        return new float[,]
        {
            { 1f, 0f,  0f, 0f },
            { 0f,  c,  -s, 0f },
            { 0f,  s,   c, 0f },
            { 0f, 0f,  0f, 1f }
        };
    }

    private void ApplyRotationXRaw()
    {
        if (rotatedMesh == null)
        {
            return;
        }

        Vector3[] baseVertices = rotatedMesh.vertices;
        Vector3[] result =
            new Vector3[baseVertices.Length];

        float[,] rotationMatrix =
            RotationXMatrixRaw(angleDegrees);

        for (int i = 0; i < baseVertices.Length; i++)
        {
            result[i] = MultiplyPointRaw(
                rotationMatrix,
                baseVertices[i]
            );
        }

        rotatedMesh.vertices = result;
        rotatedMesh.RecalculateNormals();
        rotatedMesh.RecalculateBounds();
    }

    private Vector3 MultiplyPointRaw(
        float[,] matrix,
        Vector3 point
    )
    {
        float x =
            matrix[0, 0] * point.x +
            matrix[0, 1] * point.y +
            matrix[0, 2] * point.z +
            matrix[0, 3];

        float y =
            matrix[1, 0] * point.x +
            matrix[1, 1] * point.y +
            matrix[1, 2] * point.z +
            matrix[1, 3];

        float z =
            matrix[2, 0] * point.x +
            matrix[2, 1] * point.y +
            matrix[2, 2] * point.z +
            matrix[2, 3];

        return new Vector3(x, y, z);
    }

    private void OnDestroy()
    {
        if (rotatedMesh != null)
        {
            Destroy(rotatedMesh);
        }
    }
}