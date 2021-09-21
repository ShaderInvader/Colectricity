using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SelectKeys))]
public class MovementPrototype : MonoBehaviour
{
    public float speed = 1;
    public float scale_speed_factor = 0.1f;
    public float angle = 0.0f;
    public bool takeAngleFromCamera = true;

    Camera cam;
    Rigidbody rb;
    Vector3 start_scale = new Vector3(1,1,1);
    List<KeyCode> keys;
    int forward, right;

    private void OnEnable()
    {
        cam = gameObject.GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        bool isWSAD = gameObject.GetComponent<SelectKeys>().selection == SelectKeys.Keys.wsad;
        List<KeyCode> WSAD = new List<KeyCode>() { KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.A };
        List<KeyCode> Arrows = new List<KeyCode>() { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow };
        keys = isWSAD ? WSAD : Arrows; 
    }
    void FixedUpdate()
    {
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
        float add = (transform.localScale - start_scale).magnitude*scale_speed_factor;
        Vector3 vel = new Vector3(right, 0, forward).normalized * (speed + add);
        Vector3 rotatedVel = Quaternion.Euler(0, angle, 0) * vel;
        rb.velocity = rotatedVel;
    }
}
