using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferPlane : MonoBehaviour
{
    public float rotationSpeed;

    void Update()
    {
        this.transform.Rotate(new Vector3(0, 0, rotationSpeed));
    }
}
