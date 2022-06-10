using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStartPoint : MonoBehaviour
{
    private Vector3 startPostion;
    public Vector3 StartPosition { 
        get { return startPostion; } 
        set { startPostion = value; }
    }

    void Start()
    {
        StartPosition = transform.position;   
    }
}
