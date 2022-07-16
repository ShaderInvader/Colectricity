using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothRotation : MonoBehaviour
{
    public MovementPrototype mp;
    public float speed = 0.1f;

    void Update()
    {
        Vector3 targetDirection = new Vector3(-mp.movement_vector.z, 0, mp.movement_vector.x);
        float singleStep = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
