using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMovement : MonoBehaviour
{
    public MovementPrototype electronMove;

    public Transform targetTransform;
    Vector3 startPosition;
    Quaternion startRotation;

    Vector3 endPosition;
    Quaternion endRotation;

    float maxSpeed;

    void Start()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;

        endPosition = targetTransform.localPosition;
        endRotation = targetTransform.localRotation;

        maxSpeed = electronMove.lifeSpeed;
    }

    void Update()
    {
        float speedMagnitude = electronMove.GetMovementVector().magnitude;
        float lerpVal = speedMagnitude / maxSpeed;

        transform.localPosition = Vector3.Lerp(startPosition, endPosition, lerpVal);
        transform.localRotation = Quaternion.Lerp(startRotation, endRotation, lerpVal);
    }
}
