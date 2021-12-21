using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergablerLight : MonoBehaviour
{
    private Light light;
    private Energabler energabler;

    public int minLightIntensity = 0;
    public int maxLightIntensity = 20000;

    void Start()
    {
        light = GetComponentInChildren<Light>();
        energabler = GetComponent<Energabler>();
        if (energabler == null) Debug.LogError("Energabler Light needs Energabler in the same Object!");
        if (light == null) Debug.LogError("Energabler Light needs Light in the children Object!");
    }

    private void Update()
    {
        light.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, ((float)energabler.energy_units) / ((float)energabler.max_energy_units));
    }
}
