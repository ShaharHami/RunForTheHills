using System;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed;
    public float distanceFromTarget;
    public bool updateLookAt;
    public bool followX;
    public Vector3 lookOffset;

    private void Start()
    {
        SetLookDirection();
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(
            followX ? Mathf.Lerp(transform.position.x, target.position.x, followSpeed * Time.deltaTime) : transform.position.x,
            transform.position.y,
            target.position.z - distanceFromTarget
            );
        if (updateLookAt)
        {
            SetLookDirection();
        }
    }

    private void SetLookDirection()
    {
        transform.LookAt(new Vector3(
            transform.position.x + lookOffset.x,
            target.position.y + lookOffset.y,
            target.position.z + lookOffset.z
        ));
    }
}
