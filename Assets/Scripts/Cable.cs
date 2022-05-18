using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    public Cable connected;
    public float range_of_connection = 1;
    private Electron player;
    private bool ready = false;
    private Renderer obj_renderer;

    void Start()
    {
        if (connected == null)
        {
            Debug.Log("Connect your Cable");
            gameObject.SetActive(false);
        }
        gameObject.GetComponent<Energabler>().enabled = false;
        obj_renderer = gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        TryConnectingElectron();
        ready = (player != null);
        gameObject.GetComponent<Energabler>().enabled = ready;
        if (IsGoodToTransfer())
        { 
            int energy_acc = gameObject.GetComponent<Energabler>().energy;
            if (energy_acc > 0)
            {
                connected.SendEnergy(energy_acc);
                gameObject.GetComponent<Energabler>().RemEnergy(energy_acc);
            }
        }
    }

    public Electron GetElectron()
    {
        return player;
    }

    public float getRangeOfConnection()
    {
        return range_of_connection;
    }

    bool IsCableReady()
    {
        return ready;
    }

    public bool IsGoodToTransfer()
    {
        return ready && connected.IsCableReady();
    }

    public void SendEnergy(int ammount)
    {
        player.gameObject.GetComponent<Energabler>().AddEnergy(ammount);
        player.UpdateEmission();
    }

    void TryConnectingElectron()
    {
        Electron[] electrons = FindObjectsOfType<Electron>();
        Electron eMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Electron e in electrons)
        {
            float dist = Vector3.Distance(e.transform.position, currentPos);
            if (dist < minDist && dist <= range_of_connection)
            {
                eMin = e;
                minDist = dist;
            }
        }
        player = eMin;
    }
}
