using UnityEngine;

[RequireComponent(typeof(S11_DiamondMesh))]
public class S11_DiamondChain : MonoBehaviour
{
    [Tooltip("논리적인 부모부터 자식 순서로 넣으세요.")]
    [SerializeField] private Transform[] chain;

    private S11_DiamondMesh diamond;

    private void Awake()
    {
        diamond = GetComponent<S11_DiamondMesh>();
    }

    private void LateUpdate()
    {
        if (diamond == null || diamond.BaseVertices == null)
            return;

        Matrix4x4 combinedMatrix = Matrix4x4.identity;

        if (chain != null)
        {
            foreach (Transform node in chain)
            {
                if (node == null)
                    continue;

                Matrix4x4 localMatrix = Matrix4x4.TRS(
                    node.localPosition,
                    node.localRotation,
                    node.localScale
                );

                combinedMatrix = combinedMatrix * localMatrix;
            }
        }

        Vector3[] baseVertices = diamond.BaseVertices;
        Vector3[] transformedVertices = new Vector3[baseVertices.Length];

        for (int i = 0; i < baseVertices.Length; i++)
            transformedVertices[i] = combinedMatrix.MultiplyPoint3x4(baseVertices[i]);

        diamond.SetVertices(transformedVertices);
    }
}
