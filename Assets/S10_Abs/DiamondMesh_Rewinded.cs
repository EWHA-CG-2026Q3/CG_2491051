using UnityEngine;

[DefaultExecutionOrder(-100)]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DiamondMesh_Rewinded : MonoBehaviour
{
    [SerializeField] private Color color = new Color(0.2f, 0.75f, 1f, 1f);

    public Vector3[] BaseVertices { get; private set; }

    private Mesh diamondMesh;
    private Material generatedMaterial;

    private static readonly int[] DiamondTriangles =
    {
        0, 5, 3,
        0, 3, 4,
        0, 4, 2,
        0, 2, 5,
        1, 3, 5,
        1, 4, 3,
        1, 2, 4,
        1, 5, 2
    };

    private void Awake()
    {
        BaseVertices = new Vector3[]
        {
            new Vector3( 0f,  1f,  0f),
            new Vector3( 0f, -1f,  0f),
            new Vector3(-1f,  0f,  0f),
            new Vector3( 1f,  0f,  0f),
            new Vector3( 0f,  0f, -1f),
            new Vector3( 0f,  0f,  1f)
        };

        diamondMesh = new Mesh
        {
            name = "DiamondMesh_Rewinded_Runtime"
        };

        diamondMesh.vertices = BaseVertices;
        diamondMesh.triangles = DiamondTriangles;
        diamondMesh.RecalculateNormals();
        diamondMesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = diamondMesh;
        CreateMaterial();
    }

    public void SetVertices(Vector3[] vertices)
    {
        if (diamondMesh == null || vertices == null)
        {
            return;
        }

        if (vertices.Length != BaseVertices.Length)
        {
            Debug.LogError("정점 개수가 기본 다이아몬드와 다릅니다.", this);
            return;
        }

        diamondMesh.vertices = vertices;
        diamondMesh.RecalculateNormals();
        diamondMesh.RecalculateBounds();
    }

    private void CreateMaterial()
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");

        if (shader == null)
        {
            shader = Shader.Find("Standard");
        }

        if (shader == null)
        {
            shader = Shader.Find("Diffuse");
        }

        if (shader == null)
        {
            Debug.LogWarning("사용 가능한 Shader를 찾지 못했습니다.", this);
            return;
        }

        generatedMaterial = new Material(shader)
        {
            name = "S10_Diamond_Runtime_Material"
        };

        if (generatedMaterial.HasProperty("_BaseColor"))
        {
            generatedMaterial.SetColor("_BaseColor", color);
        }

        if (generatedMaterial.HasProperty("_Color"))
        {
            generatedMaterial.SetColor("_Color", color);
        }

        GetComponent<MeshRenderer>().sharedMaterial = generatedMaterial;
    }

    private void OnDestroy()
    {
        if (diamondMesh != null)
        {
            Destroy(diamondMesh);
        }

        if (generatedMaterial != null)
        {
            Destroy(generatedMaterial);
        }
    }
}
