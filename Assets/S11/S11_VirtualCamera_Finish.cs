using UnityEngine;
using UnityEngine.UI;

public class S11_VirtualCamera_Finish : MonoBehaviour
{
    [Header("Objects to draw")]
    [SerializeField] private MeshFilter[] targets;

    [Header("Virtual camera")]
    [SerializeField] private float size = 2f;
    [SerializeField] private float near = 0.3f;
    [SerializeField] private float far = 20f;

    [Header("Canvas output")]
    [SerializeField] private RawImage canvasImage;
    [SerializeField] private Color lineColor = Color.green;

    private Texture2D canvas;
    private Color32[] pixels;
    private int width;
    private int height;

    private void LateUpdate()
    {
        EnsureCanvas();
        DrawSceneOnCanvas();
    }

    private void EnsureCanvas()
    {
        int newWidth = Mathf.Max(2, Screen.width);
        int newHeight = Mathf.Max(2, Screen.height);

        if (canvas != null && width == newWidth && height == newHeight)
            return;

        width = newWidth;
        height = newHeight;

        if (canvas != null)
            Destroy(canvas);

        canvas = new Texture2D(width, height, TextureFormat.RGBA32, false);
        canvas.name = "S11_VirtualCameraCanvas";
        canvas.filterMode = FilterMode.Point;
        canvas.wrapMode = TextureWrapMode.Clamp;
        pixels = new Color32[width * height];

        if (canvasImage != null)
            canvasImage.texture = canvas;
    }

    private void DrawSceneOnCanvas()
    {
        ClearCanvas();

        Matrix4x4 view = BuildViewMatrix(transform);

        if (targets != null)
        {
            foreach (MeshFilter meshFilter in targets)
            {
                if (meshFilter == null || meshFilter.sharedMesh == null)
                    continue;

                Matrix4x4 viewModel = MultiplyMatrixMatrix(
                    view,
                    meshFilter.transform.localToWorldMatrix
                );

                Vector3[] vertices = meshFilter.sharedMesh.vertices;
                int[] triangles = meshFilter.sharedMesh.triangles;

                for (int i = 0; i < triangles.Length; i += 3)
                {
                    Vector3 a = viewModel.MultiplyPoint(vertices[triangles[i]]);
                    Vector3 b = viewModel.MultiplyPoint(vertices[triangles[i + 1]]);
                    Vector3 c = viewModel.MultiplyPoint(vertices[triangles[i + 2]]);

                    DrawEdge(a, b);
                    DrawEdge(b, c);
                    DrawEdge(c, a);
                }
            }
        }

        canvas.SetPixels32(pixels);
        canvas.Apply(false);
    }

    private Matrix4x4 BuildViewMatrix(Transform cam)
    {
        Matrix4x4 translationInverse = Matrix4x4.Translate(-cam.position);
        Matrix4x4 rotationInverse = Matrix4x4.Rotate(Quaternion.Inverse(cam.rotation));

        // 행렬 곱셈 연산자(*) 대신 직접 만든 함수를 사용한다.
        return MultiplyMatrixMatrix(rotationInverse, translationInverse);
    }

    private Matrix4x4 MultiplyMatrixMatrix(Matrix4x4 a, Matrix4x4 b)
    {
        Matrix4x4 result = Matrix4x4.zero;

        // B의 각 열을 차례대로 계산한다.
        for (int column = 0; column < 4; column++)
        {
            for (int row = 0; row < 4; row++)
            {
                float value = 0f;

                for (int k = 0; k < 4; k++)
                    value += a[row, k] * b[k, column];

                result[row, column] = value;
            }
        }

        return result;
    }

    private void ClearCanvas()
    {
        Color32 transparent = new Color32(0, 0, 0, 0);

        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = transparent;
    }

    private void DrawEdge(Vector3 a, Vector3 b)
    {
        if (!ToPixel(a, out Vector2 pixelA) || !ToPixel(b, out Vector2 pixelB))
            return;

        DrawLine(
            Mathf.RoundToInt(pixelA.x),
            Mathf.RoundToInt(pixelA.y),
            Mathf.RoundToInt(pixelB.x),
            Mathf.RoundToInt(pixelB.y)
        );
    }

    private bool ToPixel(Vector3 point, out Vector2 pixel)
    {
        pixel = Vector2.zero;

        if (point.z < near || point.z > far)
            return false;

        float aspect = (float)width / height;
        pixel.x = width * 0.5f + point.x / (size * aspect) * (width * 0.5f);
        pixel.y = height * 0.5f + point.y / size * (height * 0.5f);
        return true;
    }

    private void DrawLine(int x0, int y0, int x1, int y1)
    {
        int dx = Mathf.Abs(x1 - x0);
        int sx = x0 < x1 ? 1 : -1;
        int dy = -Mathf.Abs(y1 - y0);
        int sy = y0 < y1 ? 1 : -1;
        int error = dx + dy;

        while (true)
        {
            SetPixelSafe(x0, y0, lineColor);

            if (x0 == x1 && y0 == y1)
                break;

            int twiceError = 2 * error;

            if (twiceError >= dy)
            {
                error += dy;
                x0 += sx;
            }

            if (twiceError <= dx)
            {
                error += dx;
                y0 += sy;
            }
        }
    }

    private void SetPixelSafe(int x, int y, Color color)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return;

        pixels[y * width + x] = color;
    }

    private void OnDestroy()
    {
        if (canvas != null)
            Destroy(canvas);
    }
}
