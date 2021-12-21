using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnergyLiquid : MonoBehaviour
{
    public float speed = 1;
    public List<float> yScale = new List<float>();
    public Energabler fanEnergy = null;
    
    private int status = 0;
    private float time = 1.0f;

    public int Status
    {
        get => status;
        set
        {
            time = 0;
            status = value;
        }

    }

    void Start()
    {
        if (fanEnergy == null)
        {
            fanEnergy = transform.GetComponentInParent<Energabler>();
        }
    }

    void Update()
    {
        if (status != fanEnergy.energy)
        {
            Status = fanEnergy.energy_units;
        }

        if (time == 1)
        {
            return;
        }

        time += Time.deltaTime * speed;
        time = time > 1 ? 1 : time;

        float yVal = Mathf.Lerp(transform.localScale[1], yScale[status], time);
        transform.localScale = new Vector3(transform.localScale.x, yVal, transform.localScale.z);
    }
}
