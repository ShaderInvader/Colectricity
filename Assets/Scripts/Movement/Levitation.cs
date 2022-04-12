using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitation : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private float yOffset = 0;
    private float yStart;

    private void Start()
    {
        yStart = transform.position.y;
    }

    void Update()
    {
        yOffset += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, yStart + yOffset, transform.position.z);
    }
}
