using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DistanceTriggeredOperation : MonoBehaviour
{
    public float minDistanceToTrigger;

    protected void checkForDistance(Transform other)
    {
        if (Vector3.Distance(other.position, this.transform.position) < minDistanceToTrigger)
        {
            triggerActionWhenInDistance();
        } 
        else
        {
            triggerActionWhenOutOfDistance();
        }
    }

    protected abstract void triggerActionWhenInDistance();
    protected abstract void triggerActionWhenOutOfDistance();
}
