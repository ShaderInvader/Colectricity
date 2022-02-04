using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Camera cam;
    void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    void Update()
    {
        gameObject.transform.rotation = cam.transform.rotation;
    }
}
