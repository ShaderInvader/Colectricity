using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SelectKeys))]
public class MovementPrototype : MonoBehaviour
{
    public float speed = 1;

    Rigidbody rb;
    List<KeyCode> keys;
    int forward, right;

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
        (forward, right) = (0, 0);
        
        forward = Input.GetKey(keys[0]) ? ++forward : forward;
        forward = Input.GetKey(keys[1]) ? --forward : forward;
        right = Input.GetKey(keys[2]) ? right + 1 : right;
        right = Input.GetKey(keys[3]) ? right - 1 : right;

        Vector3 vel = new Vector3(right, 0, forward).normalized * speed;

        rb.velocity = vel;
    }
}
