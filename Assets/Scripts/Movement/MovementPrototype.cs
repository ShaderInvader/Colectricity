using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SelectKeys))]
public class MovementPrototype : MonoBehaviour
{
    public float lifeSpeed = 6f;
    public float deathSpeed = 4f;
    public float scale_speed_factor = 0.1f;
    public float angle = 0.0f;
    public bool takeAngleFromCamera = true;
    public float dashSpeed = 4f;
    public float dashTime = 2f;
    
    Camera cam;
    Rigidbody rb;
    Vector3 start_scale = new Vector3(1,1,1);
    float forward, right;
    bool isDashing = false, readyDash = true;
    public Vector3 movement_vector;
    private Vector3 cur_movement_vector = new Vector3(0, 0, 0);
    public float momentum_changer = 0.3f;
    private SelectKeys selectKeys;

    public float bounceForce;
    public float bounceTime;
    private float timeToEndBounce;
    private bool duringBounce = false;

    public Vector3 GetMovementVector()
    {
        return cur_movement_vector;
    }

    private void OnEnable()
    {
        cam = gameObject.GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        selectKeys = GetComponent<SelectKeys>();
    }
    
    void FixedUpdate()
    {
        if (isDashing)
        {
            StartCoroutine(Dash(movement_vector));
            return;
        }

        if (takeAngleFromCamera)
        {
            angle = cam.transform.eulerAngles.y;
        }

        if (!duringBounce)
        {
            UpdateMovementVect();
            Move(cur_movement_vector);
        }
        else
        {
            timeToEndBounce += Time.deltaTime;
            if(timeToEndBounce > bounceTime)
            {
                duringBounce = false;
            }
        }

        if (selectKeys.Dash && readyDash)
        {
            isDashing = true;
            readyDash = false;
        }
        else if(!selectKeys.Dash)
        {
            readyDash = true;
        }
    }

    void UpdateMovementVect()
    {
        rb.velocity = Vector3.zero;
        forward = Math.Abs(selectKeys.Vertical) > selectKeys.ignoreBelow ? selectKeys.Vertical : 0;
        right = Math.Abs(selectKeys.Horizontal) > selectKeys.ignoreBelow ? selectKeys.Horizontal : 0;
        float speed = GetComponent<Electron>().isDead ? deathSpeed : lifeSpeed;
        float add = (transform.localScale - start_scale).magnitude * scale_speed_factor;
        Vector3 vel = new Vector3(right, 0, forward).normalized * (speed + add);
        movement_vector = Quaternion.Euler(0, angle, 0) * vel;
        cur_movement_vector = Vector3.Lerp(cur_movement_vector, movement_vector, momentum_changer);
    }

    void RotateBall(Vector3 vect)
    {
        rb.angularVelocity = new Vector3(vect.z, 0, -vect.x);
    }

    void Move(Vector3 vect)
    {
        rb.velocity = vect;
        RotateBall(vect);
    }

    IEnumerator Dash(Vector3 vect)
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            Move(vect * dashSpeed);
            yield return null;
        }
        isDashing = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            float currSpeed = GetComponent<Electron>().isDead ? deathSpeed : lifeSpeed;
            float movementMagnitude = collision.gameObject.GetComponent<MovementPrototype>().movement_vector.magnitude;
            if (movementMagnitude < movement_vector.magnitude || movementMagnitude < currSpeed/2)
            {
                return;
            }

            Vector3 dir = transform.position - collision.transform.position;
            dir.y = 0;
            dir = dir.normalized;
            gameObject.GetComponent<Rigidbody>().AddForce(dir * bounceForce);
            duringBounce = true;
            timeToEndBounce = 0;
        }
    }
}
