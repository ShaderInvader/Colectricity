using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineToTheNearestEnergabler : MonoBehaviour
{
    Electron.Type type;

    public float minDistanceWithMinOpacity;
    public float minDistanceWithMaxOpacity;
    public float minOpacity;
    public float maxOpacity;
    public Transform line;

    void Start()
    {
        type = GetComponent<Electron>().player;
    }

    void Update()
    {
        Energabler[] energablers = FindObjectsOfType<Energabler>();
        foreach (Energabler energabler in energablers)
        {
            float distance = Vector3.Distance(transform.position, energabler.transform.position);
            if (energabler.GetComponent<Electron>() != null)
            {
                break;
            }
            else if (distance < minDistanceWithMinOpacity)
            {
                float opacity;
                if (distance < minDistanceWithMaxOpacity) opacity = maxOpacity;
                else opacity = Mathf.Lerp(maxOpacity, minOpacity, (distance - minDistanceWithMaxOpacity) / (minDistanceWithMinOpacity - minDistanceWithMaxOpacity));

                if ((type == Electron.Type.giver && energabler.energy_units < energabler.max_energy_units)
                    || (type == Electron.Type.receiver && energabler.energy_units > 0))
                {
                    Debug.DrawLine(transform.position, energabler.transform.position, Color.green);
                }
            }
        }
    }
}
