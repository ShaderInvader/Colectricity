using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial_1 : MonoBehaviour
{

    public GameObject t1;
    public GameObject t2;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        t2.SetActive(true);
        t1.SetActive(false);
    }
}
