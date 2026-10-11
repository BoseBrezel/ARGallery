using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float speed = 100f;
    public float direction = 1f;

    private void Update()
    {
        transform.Rotate(0f,0f, speed * direction * Time.deltaTime);
    }
}