using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float speed;
    public GameObject left, right;
    public bool isOpenWithFullEnergy;
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
        Debug.Log(areKnobsSwitched() + " : " + closed + " : " + isOpenWithFullEnergy);
        if (areKnobsSwitched() && closed)
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
        else if (!closed && !areKnobsSwitched())
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

    bool areKnobsSwitched()
    {
        foreach(Energabler knob in knobs)
        {
            if (isOpenWithFullEnergy && knob.IsFull() == false) return false;
            if (!isOpenWithFullEnergy && knob.IsEmpty() == false) return false;
        }
        return true;
    }
}
