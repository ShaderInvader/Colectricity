using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float speed = 1;
    public float offset = 2;

    public List<Energabler> energablers;

    public List<MeshRenderer> doorPanels;
    public List<Light> doorLights;

    private List<float> panelsMaxValues = new List<float>();
    private List<float> lightsMaxIntensities = new List<float>();

    private float offsetCounter = 0;
    private float time = 1;
    private bool isOpen = false;
    private string propertyName = "DissapearFromAboveOrigin";

    public bool IsOpen
    {
        get => isOpen;
        set
        {
            time = 0;
            offsetCounter = 0;
            isOpen = value;
        }

    }

    void Start()
    {
        foreach (MeshRenderer panel in doorPanels)
        {
            panelsMaxValues.Add(panel.material.GetFloat(propertyName));
        }
        foreach (Light light in doorLights)
        {
            lightsMaxIntensities.Add(light.intensity);
        }
    }

    void Update()
    {
        if (IsFull() != isOpen)
        {
            IsOpen = !isOpen;
            UpdateColliders();
        }


        if (time == 1)
        {
            return;
        }

        if (offsetCounter < offset)
        {
            offsetCounter += Time.deltaTime;
            return;
        }

        time += Time.deltaTime * speed;
        if (time > 1)
        {
            time = 1;
        }

        if (isOpen)
        {
            foreach (Light li in doorLights)
            {
                li.intensity = Mathf.Lerp(li.intensity, 0, time);
            }
            foreach (MeshRenderer panel in doorPanels)
            {
                float val = Mathf.Lerp(panel.material.GetFloat(propertyName), 0, time);
                panel.material.SetFloat(propertyName, val);
            }
        }
        else
        {
            for (int i = 0; i < doorLights.Count; i++)
            {
                doorLights[i].intensity = Mathf.Lerp(doorLights[i].intensity, lightsMaxIntensities[i], time);
            }
            for (int i = 0; i < doorPanels.Count; i++)
            {
                float val = Mathf.Lerp(doorPanels[i].material.GetFloat(propertyName), panelsMaxValues[i], time);
                doorPanels[i].material.SetFloat(propertyName, val);
            }
        }
    }

    void UpdateColliders()
    {
        foreach (MeshRenderer panel in doorPanels)
        {
            Collider col = panel.gameObject.GetComponentInChildren<Collider>();
            if (col != null)
            {
                col.enabled = !isOpen;
            }
        }
    }

    bool IsFull()
    {
        return energablers.All(e => e.IsFull());
    }
}
