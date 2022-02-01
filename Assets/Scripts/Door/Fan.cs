using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fan : MonoBehaviour
{
    public float speed = 80;
    public float acceleration = 30;
    public bool rotationDirection = true;
    public List<Energabler> energablers = new List<Energabler>();

    private float currentSpeed = 0;
    private Vector3 direction = Vector3.up;

    void Start()
    {
        if (energablers.Count == 0)
        {
            energablers.Add(GetComponentInParent<Energabler>());
        }
    }
    
    void Update()
    {
        direction = rotationDirection ? direction = Vector3.down : Vector3.up;

        float vel = Time.deltaTime * currentSpeed;

        float changeOfSpeed = Time.deltaTime * acceleration;
        currentSpeed += IsFull() ? changeOfSpeed : -changeOfSpeed;

        currentSpeed = currentSpeed < 0 ? 0 : currentSpeed;
        currentSpeed = currentSpeed > speed ? speed : currentSpeed;

        transform.Rotate(direction, Time.deltaTime * currentSpeed);
    }

    private bool IsFull()
    {
        return energablers.All(e => e.IsFull());
    }
}
