using UnityEngine;

public class S12_CameraMove : MonoBehaviour
{
    [SerializeField] private float moveDistance = 1.2f;
    [SerializeField] private float moveSpeed = 1.0f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float amount = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = startPosition + transform.forward * amount;
    }
}