using UnityEngine;

public class Mid_OrbitingRelic : MonoBehaviour
{
    public Transform floatingTarget;
    public float orbitSpeed = 30f;
    public float bobHeight = 0.2f;
    public float bobSpeed = 2f;

    private Vector3 startPosition;

    private void Start()
    {
        if (floatingTarget != null)
            startPosition = floatingTarget.localPosition;
    }

    private void Update()
    {
        transform.Rotate(0f, orbitSpeed * Time.deltaTime, 0f);

        if (floatingTarget == null)
            return;

        Vector3 position = startPosition;
        position.y += Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        floatingTarget.localPosition = position;
    }
}
