using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;

    public float speed;
    public float restingTime;

    private float timer = 0;
    private direction dir = direction.A;
    Vector3 vel = new Vector3();

    private void Start()
    {
        transform.position = pointA;
        vel = (pointB - pointA).normalized * speed; dir = direction.B;
        dir = direction.B;
    }

    private void Update()
    {
        if (GetComponent<Energabler>().energy == 0) GameObject.Destroy(this.gameObject);

        if (timer > 0)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            timer -= Time.deltaTime;
            return;
        }
        else if (Vector3.Distance(pointA, transform.position) == 0 && dir == direction.A)
        {
            timer = restingTime;
            dir = direction.B;
            vel = (pointB - pointA).normalized * speed; dir = direction.B;
        }
        else if (Vector3.Distance(pointB, transform.position) == 0 && dir == direction.B)
        {
            timer = restingTime;
            dir = direction.A;
            vel = (pointA - pointB).normalized * speed;
        }
        else
        {
            GetComponent<Rigidbody>().velocity = vel;
        }

        if (Vector3.Distance(pointB, pointA) < Vector3.Distance(transform.position, pointA))
        {
            transform.position = pointB;
        }

        if (Vector3.Distance(pointB, pointA) < Vector3.Distance(transform.position, pointB))
        {
            transform.position = pointA;
        }
    }
}

public enum direction
{
    A,
    B
}