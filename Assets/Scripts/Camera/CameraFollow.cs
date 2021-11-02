using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform parent;
    public Vector3 offset;
    public float followSpeed = 0.1f;

    void Start()
    {
        if (this.transform.parent != null)
        {
            parent = transform.parent;
            transform.SetParent(null, true);
            offset = this.transform.position - parent.position;
        }
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, parent.position + offset, followSpeed);
    }
}
