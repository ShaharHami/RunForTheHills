using UnityEngine;
using Random = UnityEngine.Random;

public class Spinner : MonoBehaviour
{
    public float rotationSpeed;

    private void Start()
    {
        rotationSpeed = Random.Range(0f, 1f) < 0.5f ? rotationSpeed : -rotationSpeed;
    }

    void Update()
    {
        var rot = transform.rotation;
        transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
