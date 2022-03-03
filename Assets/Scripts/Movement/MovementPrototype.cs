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
    private Vector3 movement_vector;
    private SelectKeys selectKeys;

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

        rb.velocity = Vector3.zero;
        (forward, right) = (0, 0);
        forward = Math.Abs(selectKeys.Vertical) > selectKeys.ignoreBelow ? selectKeys.Vertical : 0;
        right = Math.Abs(selectKeys.Horizontal) > selectKeys.ignoreBelow ? selectKeys.Horizontal : 0;
        float speed = GetComponent<Electron>().isDead ? deathSpeed : lifeSpeed;
        float add = (transform.localScale - start_scale).magnitude * scale_speed_factor;
        Vector3 vel = new Vector3(right, 0, forward).normalized * (speed + add);
        movement_vector = Quaternion.Euler(0, angle, 0) * vel;

        Move(movement_vector);
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

    void Move(Vector3 vect)
    {
        rb.velocity = vect;
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
}
