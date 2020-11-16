using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed;
    public float distanceFromTarget;
    public Vector3 lookOffset;
    private void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, target.position.x, followSpeed * Time.deltaTime),
            transform.position.y,
            target.position.z - distanceFromTarget
            );
        transform.LookAt(new Vector3(
            transform.position.x + lookOffset.x,
            target.position.y + lookOffset.y,
            target.position.z + lookOffset.z
            ));
    }
}
