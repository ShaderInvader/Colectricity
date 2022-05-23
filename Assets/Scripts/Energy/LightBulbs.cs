using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulbs : MonoBehaviour
{
    public GameObject[] lights;
    int currentHP;

    private void Start()
    {
        currentHP = lights.Length;
    }

    public void TurnOffNext()
    {
        int turnOffID = --currentHP;
        lights[turnOffID].SetActive(false);
    }
    public void TurnOnNext()
    {
        int turnOffID = currentHP++;
        lights[turnOffID].SetActive(true);
    }

    public void TurnOnAll()
    {
        foreach (GameObject l in lights)
        {
            l.SetActive(true);
        }
        currentHP = lights.Length;
    }
}
