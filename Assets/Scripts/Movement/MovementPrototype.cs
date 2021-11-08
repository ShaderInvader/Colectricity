using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SelectKeys))]
public class MovementPrototype : MonoBehaviour
{
    public float speed = 1;
    public float scale_speed_factor = 0.1f;
    public float angle = 0.0f;
    public bool takeAngleFromCamera = true;
    public float dashSpeed = 4f;
    public float dashTime = 2f;
    
    Camera cam;
    Rigidbody rb;
    Vector3 start_scale = new Vector3(1,1,1);
    List<KeyCode> keys;
    int forward, right;
    bool isDashing = false;
    private Vector3 movement_vector;

    List<KeyCode> WSAD = new List<KeyCode>() { KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.A, KeyCode.R };
    List<KeyCode> Arrows = new List<KeyCode>() { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.Period };

    private void OnEnable()
    {
        cam = gameObject.GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        bool isWSAD = gameObject.GetComponent<SelectKeys>().selection == SelectKeys.Keys.wsad;
        keys = isWSAD ? WSAD : Arrows; 
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
        forward = Input.GetKey(keys[0]) ? ++forward : forward;
        forward = Input.GetKey(keys[1]) ? --forward : forward;
        right = Input.GetKey(keys[2]) ? right + 1 : right;
        right = Input.GetKey(keys[3]) ? right - 1 : right;
        float add = (transform.localScale - start_scale).magnitude * scale_speed_factor;
        Vector3 vel = new Vector3(right, 0, forward).normalized * (speed + add);
        movement_vector = Quaternion.Euler(0, angle, 0) * vel;

        Move(movement_vector);
        if (Input.GetKeyDown(keys[4]))
        {
            isDashing = true;
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
