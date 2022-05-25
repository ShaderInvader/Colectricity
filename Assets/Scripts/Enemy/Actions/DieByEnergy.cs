using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieByEnergy : MonoBehaviour
{
    public int energyUnitsToDie = 3;
    int current = 0, previous=0;
    int max;

    private void Start()
    {
        max = energyUnitsToDie;
    }

    void Update()
    {
        current = GetComponent<Energabler>().energy_units;
        if (previous>current)
        {
            energyUnitsToDie -= (previous - current);
            if (energyUnitsToDie==0)
            {
                Destroy(gameObject);
            }
        }
        previous = current;
    }
}
