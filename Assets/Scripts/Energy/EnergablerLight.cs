using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergablerLight : MonoBehaviour
{
    private Light light;

    void Start()
    {
        light = GetComponentInChildren<Light>();
    }
}
