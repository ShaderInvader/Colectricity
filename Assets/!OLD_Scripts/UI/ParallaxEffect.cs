using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public ParallaxLevel[] parallaxLevels;
    private List<Vector3> startPosPL = new List<Vector3>();

    void Start()
    {
        for (int i = 0; i < parallaxLevels.Length; i++)
        {
            startPosPL.Add(parallaxLevels[i].spriteTransform.position);
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < parallaxLevels.Length; i++)
        {
            Vector3 displacement = (transform.position - startPosPL[i]) * parallaxLevels[i].parallaxPower;
            Vector3 position = startPosPL[i] + displacement;
            parallaxLevels[i].spriteTransform.position = position;
        }
    }
}
