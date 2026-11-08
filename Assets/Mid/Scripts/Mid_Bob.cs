using UnityEngine;

public class Mid_Bob : MonoBehaviour
{
    public float height = 0.3f;
    public float speed = 2f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        Vector3 position = startPosition;
        position.y += Mathf.Abs(Mathf.Sin(Time.time * speed)) * height;
        transform.position = position;
    }
}
