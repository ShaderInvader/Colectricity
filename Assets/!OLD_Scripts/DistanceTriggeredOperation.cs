using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DistanceTriggeredOperation : MonoBehaviour
{
    protected float minDistanceToTrigger;

    protected bool checkForDistance(Transform other)
    {
        if (Vector3.Distance(other.position, this.transform.position) < minDistanceToTrigger)
        {
            return true;
        } 
        else
        {
            return false;
        }
    }

    protected abstract void triggerActionWhenInDistance();
    protected abstract void triggerActionWhenOutOfDistance();
}
