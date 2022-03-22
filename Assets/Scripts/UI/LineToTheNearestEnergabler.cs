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

    private GameObject myLine;

    void Start()
    {
        type = GetComponent<Electron>().player;

        myLine = new GameObject();
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        if (material != null)
        {
            lr.material = material;
        }
    }

    void Update()
    {
        List<GameObject> possibleEnergablers = new List<GameObject>();
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
                    possibleEnergablers.Add(e.gameObject);
                   // DrawLine(this.transform.position, e.transform.position);
                }
            }
        }
        if(possibleEnergablers.Count > 0)
        {
            GameObject nearest = possibleEnergablers[0];
            float minDistance = Vector3.Distance(this.transform.position, possibleEnergablers[0].transform.position);
            for(int i=0; i<possibleEnergablers.Count; i++)
            {
                if(Vector3.Distance(this.transform.position, possibleEnergablers[i].transform.position) < minDistance)
                {
                    minDistance = Vector3.Distance(this.transform.position, possibleEnergablers[i].transform.position);
                    nearest = possibleEnergablers[i];
                }
            }
            myLine.SetActive(true);
            DrawLine(this.transform.position, nearest.transform.position);
        } 
        else
        {
            myLine.SetActive(false);
        }
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        myLine.transform.position = start;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
