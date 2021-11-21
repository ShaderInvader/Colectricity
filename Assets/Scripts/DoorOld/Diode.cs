using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diode : MonoBehaviour
{
    public bool switchesOnFull;
    public Energabler fan_energy;
    MeshRenderer diodeRenderer;
    bool status;
    void Start()
    {
        diodeRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (diodeRenderer != null)
        {
            if (switchesOnFull)
            {
                diodeRenderer.materials[0].SetColor("_BaseColor", Color.Lerp(Color.red, Color.green, ((float)fan_energy.energy) / fan_energy.max_energy));
            }
            else
            {
                diodeRenderer.materials[0].SetColor("_BaseColor", Color.Lerp(Color.green, Color.red, ((float)fan_energy.energy) / fan_energy.max_energy));
            }
            status = switchesOnFull ? fan_energy.IsFull() : fan_energy.IsEmpty();
        }
    }

    public bool IsSwitchedOn()
    {
        return status;
    }
}
