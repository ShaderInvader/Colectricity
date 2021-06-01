using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealer : MonoBehaviour
{
    public float stealingRadius;
    public int stealingAmmount;
    public float timeBetweenStealing;
    GameObject[] all;

    private float sinceLast;

    private void Start()
    {
        all = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        if(sinceLast <= 0)
        {
            sinceLast = timeBetweenStealing;
            foreach (GameObject g in all) // very slow method, needed refactoring and optimalization
            {
                if (Vector3.Distance(g.transform.position, transform.position) < stealingRadius)
                {
                    int before = g.GetComponent<Energabler>().energy;
                    g.GetComponent<Energabler>().energy = Mathf.Max(0, g.GetComponent<Energabler>().energy - stealingAmmount);
                    GetComponent<Energabler>().energy += before - g.GetComponent<Energabler>().energy;
                }
            }
        }
        sinceLast = sinceLast - Time.deltaTime;
    }
}
