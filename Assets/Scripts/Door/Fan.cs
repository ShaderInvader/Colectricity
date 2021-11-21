using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fan : MonoBehaviour
{
    public float speed = 10;
    public float acceleration = 10;
    public List<Energabler> energablers = new List<Energabler>();

    private float currentSpeed = 0;

    void Start()
    {
        if (energablers.Count == 0)
        {
            energablers.Add(GetComponentInParent<Energabler>());
        }
    }
    
    void Update()
    {
        float vel = Time.deltaTime * currentSpeed;

        float changeOfSpeed = Time.deltaTime * acceleration;
        currentSpeed += IsFull() ? changeOfSpeed : -changeOfSpeed;

        currentSpeed = currentSpeed < 0 ? 0 : currentSpeed;
        currentSpeed = currentSpeed > speed ? speed : currentSpeed;

        transform.Rotate(Vector3.up, Time.deltaTime * currentSpeed);
    }

    private bool IsFull()
    {
        return energablers.All(e => e.IsFull());
    }
}
