using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPrototype : MonoBehaviour
{
    public bool wsad = false;

    Transform obj;
    List<KeyCode> keys;
    int forward, right;

    private void Start()
    {
        obj = gameObject.GetComponent<Transform>();
        keys = wsad ? new List<KeyCode>() { KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.A }
            : new List<KeyCode>() { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow };
    }

    void Update()
    {
        (forward, right) = (0, 0);
        
        forward = Input.GetKey(keys[0]) ? ++forward : forward;
        forward = Input.GetKey(keys[1]) ? --forward : forward;
        right = Input.GetKey(keys[2]) ? right + 1 : right;
        right = Input.GetKey(keys[3]) ? right - 1 : right;

        Vector3 mov = new Vector3(right, 0, forward);
        obj.Translate(mov.normalized);
    }
}
