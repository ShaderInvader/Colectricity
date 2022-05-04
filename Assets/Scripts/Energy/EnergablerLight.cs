using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergablerLight : MonoBehaviour
{
    private Light eLight;
    private Energabler energabler;

    public int minLightIntensity = 0;
    public int maxLightIntensity = 20000;
    [Space]
    public MeshRenderer cableRenderer;
    [Space]
    [ColorUsage(false, true)] public Color disabledEmissiveColor;
    [ColorUsage(false, true)] public Color enabledEmissiveColor;
    [Space]
    [ColorUsage(false, true)] public Color disabledDetailColor;
    [ColorUsage(false, true)] public Color enabledDetailColor;

    private static readonly int EmissiveColor = Shader.PropertyToID("Emissive_Color");
    private static readonly int DetailColor = Shader.PropertyToID("Detail_Color");

    void Start()
    {
        eLight = GetComponentInChildren<Light>();
        energabler = GetComponent<Energabler>();
        if (energabler == null) Debug.LogError("Energabler Light needs Energabler in the same Object!");
        if (eLight == null) Debug.LogError("Energabler Light needs Light in the children Object!");
    }

    private void Update()
    {
        float energyPercent = ((float)energabler.energy_units) / ((float)energabler.max_energy_units);

        eLight.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, energyPercent);

        if (cableRenderer)
        {
            cableRenderer.material.SetColor(EmissiveColor, Color.Lerp(disabledEmissiveColor, enabledEmissiveColor, energyPercent));
            cableRenderer.material.SetColor(DetailColor, Color.Lerp(disabledDetailColor, enabledDetailColor, energyPercent));
        }
    }
}
