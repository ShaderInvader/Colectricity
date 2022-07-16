using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;
        if (go.GetComponent<Electron>() != null)
        {
            go.GetComponent<Energabler>().energy = 0;
        }
    }
}
