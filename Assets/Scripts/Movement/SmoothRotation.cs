using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothRotation : MonoBehaviour
{
    public Movement movement;
    public float speed = 0.1f;

    void Update()
    {
        Vector3 targetDirection = new Vector3(-movement.MovementVector.z, 0, movement.MovementVector.x);
        float singleStep = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
