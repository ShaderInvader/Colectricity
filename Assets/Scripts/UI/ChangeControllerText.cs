using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeControllerText : MonoBehaviour
{
    void Start()
    {
        foreach(ControllerInfo.controllerEnum controllerType in System.Enum.GetValues(typeof(ControllerInfo.controllerEnum)))
        {
            Transform[] texts = this.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in texts)
            {
                if(t.gameObject.name == ControllerInfo.getObjectName(controllerType)) 
                {
                    if (controllerType == ControllerInfo.controllerPick)
                    {
                        t.gameObject.SetActive(true);
                    } 
                    else
                    {
                        t.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
