using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S08_DirectTransform : MonoBehaviour
{
    public enum DemoMode
    {
        TranslateThenScale,
        ScaleThenTranslate
    }

    [Header("Demo Mode")]
    [SerializeField]
    private DemoMode demoMode = DemoMode.TranslateThenScale;

    [Header("Values used by both operations")]
    [SerializeField]
    private Vector3 translation = new Vector3(5f, 0f, 0f);

    [SerializeField]
    private Vector3 scale = new Vector3(2f, 1f, 1f);

    [Header("Display")]
    [SerializeField]
    private Color objectColor =
        new Color(1f, 0.45f, 0.1f, 1f);

    private Mesh generatedMesh;
    private Material generatedMaterial;
    private Vector3[] baseVertices;

    private static readonly int[] DiamondTriangles =
    {
        // 위쪽 면 4개
        0, 5, 3,
        0, 3, 4,
        0, 4, 2,
        0, 2, 5,

        // 아래쪽 면 4개
        1, 3, 5,
        1, 4, 3,
        1, 2, 4,
        1, 5, 2
    };

    private void Awake()
    {
        CreateDiamondMesh();
        CreateMaterial();
        ApplySelectedMode();
    }

    private void CreateDiamondMesh()
    {
        // 6개의 정점으로 다이아몬드 모양을 만든다.
        baseVertices = new Vector3[]
        {
            new Vector3( 0f,  1f,  0f), // 0: 위
            new Vector3( 0f, -1f,  0f), // 1: 아래
            new Vector3(-1f,  0f,  0f), // 2: 왼쪽
            new Vector3( 1f,  0f,  0f), // 3: 오른쪽
            new Vector3( 0f,  0f, -1f), // 4: 앞
            new Vector3( 0f,  0f,  1f)  // 5: 뒤
        };

        generatedMesh = new Mesh
        {
            name = $"S08_Diamond_{demoMode}"
        };

        GetComponent<MeshFilter>().sharedMesh = generatedMesh;
    }

    private void CreateMaterial()
    {
        Shader shader =
            Shader.Find("Universal Render Pipeline/Lit");

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
            Debug.LogWarning(
                "사용 가능한 Shader를 찾지 못했습니다.",
                this
            );

            return;
        }

        generatedMaterial = new Material(shader)
        {
            name = $"S08_Material_{demoMode}"
        };

        SetMaterialColor();

        GetComponent<MeshRenderer>().sharedMaterial =
            generatedMaterial;
    }

    private void SetMaterialColor()
    {
        if (generatedMaterial == null)
        {
            return;
        }

        if (generatedMaterial.HasProperty("_BaseColor"))
        {
            generatedMaterial.SetColor(
                "_BaseColor",
                objectColor
            );
        }

        if (generatedMaterial.HasProperty("_Color"))
        {
            generatedMaterial.SetColor(
                "_Color",
                objectColor
            );
        }
    }

    private void ApplySelectedMode()
    {
        if (generatedMesh == null || baseVertices == null)
        {
            return;
        }

        Vector3[] transformedVertices;

        switch (demoMode)
        {
            case DemoMode.TranslateThenScale:
                // 1. 먼저 이동: v + t
                // 2. 그 결과를 스케일: (v + t) * s
                transformedVertices = ApplyScale(
                    ApplyTranslation(
                        baseVertices,
                        translation
                    ),
                    scale
                );
                break;

            case DemoMode.ScaleThenTranslate:
                // 1. 먼저 스케일: v * s
                // 2. 그 결과를 이동: (v * s) + t
                transformedVertices = ApplyTranslation(
                    ApplyScale(
                        baseVertices,
                        scale
                    ),
                    translation
                );
                break;

            default:
                transformedVertices =
                    (Vector3[])baseVertices.Clone();
                break;
        }

        generatedMesh.Clear();
        generatedMesh.vertices = transformedVertices;
        generatedMesh.triangles = DiamondTriangles;

        generatedMesh.RecalculateNormals();
        generatedMesh.RecalculateBounds();
    }

    public Vector3[] ApplyTranslation(
        Vector3[] inputVertices,
        Vector3 amount
    )
    {
        Vector3[] result =
            new Vector3[inputVertices.Length];

        for (int i = 0; i < inputVertices.Length; i++)
        {
            result[i] = inputVertices[i] + amount;
        }

        return result;
    }

    public Vector3[] ApplyScale(
        Vector3[] inputVertices,
        Vector3 amount
    )
    {
        Vector3[] result =
            new Vector3[inputVertices.Length];

        for (int i = 0; i < inputVertices.Length; i++)
        {
            result[i] = Vector3.Scale(
                inputVertices[i],
                amount
            );
        }

        return result;
    }

    private void OnValidate()
    {
        // 게임 실행 중 Inspector 값을 바꾸면
        // 결과를 즉시 다시 계산한다.
        if (!Application.isPlaying)
        {
            return;
        }

        SetMaterialColor();
        ApplySelectedMode();
    }

    private void OnDestroy()
    {
        if (generatedMesh != null)
        {
            Destroy(generatedMesh);
        }

        if (generatedMaterial != null)
        {
            Destroy(generatedMaterial);
        }
    }
}