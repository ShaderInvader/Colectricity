using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockbackOnEnergyChange : MonoBehaviour
{
    public float knockbackForce = 20f;
    public float knockbackSlowDown = 0.9f;
    public float afterKnockbackTime = 1f;
    public bool pobieraczDealingDmg;
    public bool oddawaczDealingDmg;

    private int current;
    private int previous;
    private Transform pobieracz;
    private Transform oddawacz;
    private Rigidbody rb;
    private Vector3 knockBackVector;
    private float time = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Electron[] electrons = (Electron[]) Object.FindObjectsOfType(typeof(Electron));
        foreach(Electron e in electrons)
        {
            if(e.player == Electron.Type.giver)
            {
                oddawacz = e.gameObject.transform;
            }
            else if(e.player == Electron.Type.receiver)
            {
                pobieracz = e.gameObject.transform;
            }
        }
    }

    void Update()
    {
        current = GetComponent<Energabler>().energy_units;
        if (previous > current && pobieraczDealingDmg)
        {
            time = afterKnockbackTime;
            GetComponent<SimpleAI>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            rb.velocity = Vector3.zero;

            knockBackVector = new Vector3(this.transform.position.x - pobieracz.position.x,
                this.transform.position.y - pobieracz.position.y,
                this.transform.position.z - pobieracz.position.z).normalized;
            rb.velocity = knockBackVector * knockbackForce;

        }
        if (previous < current && oddawaczDealingDmg)
        {
            time = afterKnockbackTime;
            GetComponent<SimpleAI>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            rb.velocity = Vector3.zero;

            knockBackVector = new Vector3(this.transform.position.x - oddawacz.position.x,
                this.transform.position.y - oddawacz.position.y,
                this.transform.position.z - oddawacz.position.z).normalized;
            rb.velocity = knockBackVector * knockbackForce;
        }
        if(rb.velocity.magnitude > 0)
        {
            rb.velocity = rb.velocity * knockbackSlowDown;
        }
        if (Mathf.Abs(rb.velocity.magnitude) < 0.1 && time > 0)
        {
            rb.velocity = Vector3.zero;
            time = time - Time.deltaTime;
        }
       time = time > 0 ? time : 0;
        if (time == 0)
        {
            GetComponent<SimpleAI>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
        }
        previous = current;
    }
}
