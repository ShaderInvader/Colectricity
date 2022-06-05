using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParallaxLevel : MonoBehaviour
{
    [HideInInspector] public Transform referencePoint;
    public float effectPower;

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        if (referencePoint)
        {
            Vector3 displacement = (_startPosition - referencePoint.position) * effectPower;
            Vector3 position = referencePoint.position + displacement;
            transform.position = position;
        }
    }
}
