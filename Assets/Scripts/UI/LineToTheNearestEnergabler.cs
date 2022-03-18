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
    public Material material;

   private List<GameObject> lines = new List<GameObject>();

    void Start()
    {
        type = GetComponent<Electron>().player;
    }

    void Update()
    {
        foreach(GameObject line in lines)
        {
            Destroy(line);
        }
        Energabler[] energablers = GameObject.FindObjectsOfType<Energabler>();
        foreach(Energabler e in energablers) /* usun to */
        {
            float distance = Vector3.Distance(transform.position, e.transform.position);
            if (e.GetComponent<Electron>() != null)
            {
                continue;
            }
            else if (distance < minDistanceWithMinOpacity)
            {
                float opacity;
                if (distance < minDistanceWithMaxOpacity) opacity = maxOpacity;
                else opacity = Mathf.Lerp(maxOpacity, minOpacity, (distance - minDistanceWithMaxOpacity) / (minDistanceWithMinOpacity - minDistanceWithMaxOpacity)) / maxOpacity;

                if ((type == Electron.Type.giver && e.energy_units < e.max_energy_units && GetComponent<Energabler>().energy_units > 0)
                    || (type == Electron.Type.receiver && e.energy_units > 0))
                {
                    DrawLine(this.transform.position, e.transform.position, material);
                }
            }
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Material material)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        if(material != null)
        {
            lr.material = material;
        }
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lines.Add(myLine);
    }
}
