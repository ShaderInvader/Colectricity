using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float speed;
    public GameObject left, right;
    bool closed;

    Energabler[] knobs;
    
    float distance = 1f;
    Vector3 startPosRight, startPosLeft;

    void Start()
    {
        startPosRight = right.transform.localPosition;
        startPosLeft = left.transform.localPosition;
        knobs = GetComponentsInChildren<Energabler>();
        closed = true;
    }

    void Update()
    {
        if (areKnobsFull() && closed)
        {
            Vector3 epsilon = Vector3.right * Time.deltaTime;
            right.transform.Translate(epsilon);
            left.transform.Translate(-epsilon);
            if(right.transform.localPosition.x>=startPosRight.x+distance)
            {
                right.transform.localPosition = startPosRight + Vector3.right*distance;
                left.transform.localPosition = startPosLeft + Vector3.left*distance;
                closed = false;
            }
        }   
        else if (!closed && !areKnobsFull())
        {
            Vector3 epsilon = Vector3.right * Time.deltaTime;
            right.transform.Translate(-epsilon);
            left.transform.Translate(epsilon);
            if (right.transform.localPosition.x <= startPosRight.x)
            {
                right.transform.localPosition = startPosRight;
                left.transform.localPosition = startPosLeft;
                closed = true;
            }
        }
    }

    bool areKnobsFull()
    {
        foreach(Energabler knob in knobs)
        {
            if(knob.IsFull() == false) return false;
        }
        return true;
    }
}
