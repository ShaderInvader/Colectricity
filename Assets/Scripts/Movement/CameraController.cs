using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform p;
    public Vector3 offset;

    void Start()
    {
        if (this.transform.parent != null)
        {
            p = transform.parent;
            transform.SetParent(null, true);
            offset = this.transform.position - p.position;
        }
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, p.position + offset, 0.1f);
    }
}
