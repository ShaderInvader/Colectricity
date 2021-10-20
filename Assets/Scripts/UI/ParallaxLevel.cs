using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]  
public class ParallaxLevel
{
    public Transform spriteTransform;
    public float parallaxPower;

    public ParallaxLevel(Transform s, float pp)
    {
        spriteTransform = s;
        parallaxPower = pp; 
    }
}
