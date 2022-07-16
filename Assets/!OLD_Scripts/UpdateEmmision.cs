using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateEmmision : MonoBehaviour
{
    public void UpdateValue(int energy, int prevEnergy, double intensityPower)
    {
        int nextEnergy = energy / GlobalVars.energy_amount_unit;
        double emmisionIntensity = Math.Pow((nextEnergy + 0.01f) / (prevEnergy + 0.01f), intensityPower);
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        Color currColor = mr.material.GetColor("_EmissiveColor");
        mr.material.SetColor("_EmissiveColor", currColor * (float)emmisionIntensity);
    }
}
