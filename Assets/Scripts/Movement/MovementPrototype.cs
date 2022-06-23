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

    private Vector3 originalAppliedVector, appliedVector = new Vector3(0, 0, 0);
    private float originalDurationVector, durationAppliedVector=0;

    public List<string> knockbackables;
    public float bounceForce;
    public float bounceTime;

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
            durationAppliedVector = 0;
            appliedVector = Vector3.zero;
            StartCoroutine(Dash(movement_vector));
            return;
        }

        if (takeAngleFromCamera)
        {
            angle = cam.transform.eulerAngles.y;
        }

        if (durationAppliedVector > 0)
        {
            durationAppliedVector -= Time.deltaTime;
            if (durationAppliedVector < 0)
            {
                durationAppliedVector = 0;
            }
            appliedVector = Vector3.Lerp(Vector3.zero, originalAppliedVector, durationAppliedVector / originalDurationVector);
        }

        UpdateMovementVect();
        Move(cur_movement_vector);

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
        cur_movement_vector += appliedVector;
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

    void ApplyVector(Vector3 appVect, float duration)
    {
        originalAppliedVector = appVect;
        durationAppliedVector = originalDurationVector = duration;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ApplyKnockback(collision.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        ApplyKnockback(other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        ApplyKnockback(other.gameObject);
    }

    void ApplyKnockback(GameObject other)
    {
        string collisionTag = other.transform.tag;
        if (!knockbackables.Contains(collisionTag))
        {
            return;
        }

        if (other.transform.tag == "Player")
        {
            float currSpeed = GetComponent<Electron>().isDead ? deathSpeed : lifeSpeed;
            float movementMagnitude = other.gameObject.GetComponent<MovementPrototype>().movement_vector.magnitude;
            if (movementMagnitude < movement_vector.magnitude || movementMagnitude < currSpeed / 2)
            {
                return;
            }
        }

        if (other.transform.tag == "SmallDoor")
        {
            if (GetComponent<Energabler>().energy_units == 0)
            {
                return;
            }
        }

        Vector3 dir = transform.position - other.transform.position;
        dir.y = 0;
        dir = dir.normalized;
        ApplyVector(dir * bounceForce, bounceTime);
    }


}
