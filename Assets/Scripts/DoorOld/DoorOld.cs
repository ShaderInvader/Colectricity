using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOld : MonoBehaviour
{
    public float speed;
    public GameObject left, right;
    bool closed;

    public Diode[] diodes;
    
    float distance = 1f;
    Vector3 startPosRight, startPosLeft;

    void Start()
    {
        startPosRight = right.transform.localPosition;
        startPosLeft = left.transform.localPosition;
        closed = true;
    }

    void Update()
    {
        if (AreKnobsSwitched() && closed)
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
        else if (!closed && !AreKnobsSwitched())
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

    bool AreKnobsSwitched()
    {
        foreach(Diode diode in diodes)
        {
            if (!diode.IsSwitchedOn()) return false;
        }
        return true;
    }
}
