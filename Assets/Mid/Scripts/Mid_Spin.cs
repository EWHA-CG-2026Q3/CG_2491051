using UnityEngine;

public class Mid_Spin : MonoBehaviour
{
    public float speed = 60f;

    private void Update()
    {
        transform.Rotate(0f, speed * Time.deltaTime, 0f);
    }
}
