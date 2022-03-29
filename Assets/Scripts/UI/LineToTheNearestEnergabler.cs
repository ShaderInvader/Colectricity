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
    public Material materialForContainers;
    public Material materialForPlayers;
    public float oddawaczMinDistanceToGiveEnergyToOtherPlayer = Mathf.Infinity;

    private GameObject energablerLine;
    private GameObject playerLine;

    void Start()
    {
        type = GetComponent<Electron>().player;

        energablerLine = new GameObject();
        playerLine = new GameObject();

        energablerLine.name = "Energabler line from " + this.gameObject.name;
        playerLine.name = "Player line from " + this.gameObject.name;

        energablerLine.AddComponent<LineRenderer>();
        playerLine.AddComponent<LineRenderer>();

        LineRenderer lr = energablerLine.GetComponent<LineRenderer>();
        if (materialForContainers != null)
        {
            lr.material = materialForContainers;
        }
        lr = playerLine.GetComponent<LineRenderer>();
        if (materialForPlayers != null)
        {
            lr.material = materialForPlayers;
        }
    }

    void Update()
    {
        List<GameObject> possibleEnergablers = new List<GameObject>();
        GameObject possiblePlayers = null;
        Energabler[] energablers = GameObject.FindObjectsOfType<Energabler>();
        foreach(Energabler e in energablers)
        {
            float distance = Vector3.Distance(transform.position, e.transform.position);
            if (e.GetComponent<Electron>() != null)
            {
                float d = this.type == Electron.Type.receiver ? this.GetComponent<Electron>().transfer_distance_limit : oddawaczMinDistanceToGiveEnergyToOtherPlayer;
                if ((distance <= d)
                    && (e.energy_units < e.max_energy_units && GetComponent<Energabler>().energy_units > 0)
                    && (this.gameObject != e.gameObject))
                {
                    possiblePlayers = e.gameObject;
                    if (this.name == "Oddawacz")
                    {
                        Debug.Log(possiblePlayers.name);
                    }
                }
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
            energablerLine.SetActive(true);
            DrawLineToEnergablers(this.transform.position, nearest.transform.position);
        } 
        else
        {
            energablerLine.SetActive(false);
        }

        if(possiblePlayers != null)
        {
            playerLine.SetActive(true);
            DrawLineToPlayers(this.transform.position, possiblePlayers.transform.position);
        }
        else
        {
            playerLine.SetActive(false);
        }
    }

    void DrawLineToEnergablers(Vector3 start, Vector3 end)
    {
        LineRenderer lr = energablerLine.GetComponent<LineRenderer>();
        energablerLine.transform.position = start;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    void DrawLineToPlayers(Vector3 start, Vector3 end)
    {
        LineRenderer lr = playerLine.GetComponent<LineRenderer>();
        playerLine.transform.position = start;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
