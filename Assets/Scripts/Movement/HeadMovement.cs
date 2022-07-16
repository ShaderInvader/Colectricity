using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMovement : MonoBehaviour
{
    public Movement movement;

    public Transform targetTransform;
    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private Vector3 _endPosition;
    private Quaternion _endRotation;

    private float _maxSpeed;

    private void Start()
    {
        _startPosition = transform.localPosition;
        _startRotation = transform.localRotation;

        _endPosition = targetTransform.localPosition;
        _endRotation = targetTransform.localRotation;

        _maxSpeed = movement.NormalSpeed;
    }

    private void Update()
    {
        float speedMagnitude = movement.MovementVector.magnitude;
        if (speedMagnitude <= 0.01)
        {
            return;
        }
        float lerpVal = speedMagnitude / _maxSpeed;

        transform.localPosition = Vector3.Lerp(_startPosition, _endPosition, lerpVal);
        transform.localRotation = Quaternion.Lerp(_startRotation, _endRotation, lerpVal);
    }
}
