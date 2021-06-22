using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieByEnergy : MonoBehaviour
{
    public int energyUnitsToDie = 3;
    public Color endColor;
    Color startColor;
    int current = 0, previous=0;
    int max;
    MeshRenderer renderer;

    private void Start()
    {
        max = energyUnitsToDie;
        renderer = GetComponent<MeshRenderer>();
        startColor = renderer.materials[0].GetColor("_BaseColor");
        ChangeColor();
    }

    void Update()
    {
        current = GetComponent<Energabler>().energy_units;
        if (previous>current)
        {
            energyUnitsToDie -= (previous - current);
            ChangeColor();
            if (energyUnitsToDie==0)
            {
                Destroy(gameObject);
            }
        }
        previous = current;
    }

    void ChangeColor()
    {
        renderer.materials[0].SetColor("_BaseColor", Color.Lerp(endColor, startColor, (float)energyUnitsToDie/max));
    }
}
