using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitation : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;
    public float offset = 0f;

    private float yOffset = 0;
    private float yStart;

    private void Start()
    {
        yStart = transform.localPosition.y;
    }

    void FixedUpdate()
    {
        yOffset = Mathf.Sin((Time.fixedTime + offset) * Mathf.PI * frequency) * amplitude;
        transform.localPosition = new Vector3(transform.localPosition.x, yStart + yOffset, transform.localPosition.z);
    }
}
