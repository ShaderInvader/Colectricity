using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform parent;
    Vector3 offset;

    void Start()
    {
        parent = transform.parent;
        transform.SetParent(null, true);
        offset = transform.position - parent.position;
    }

    void Update()
    {
        transform.position = parent.position + offset;
    }
}
