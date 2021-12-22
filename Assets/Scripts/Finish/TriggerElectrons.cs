using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerElectrons : MonoBehaviour
{
    private enum Type { none, giver, receiver };
    private Type type = Type.none;
    private GameObject receiver = null;
    private GameObject giver = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        if (other.GetComponent<Electron>().player == Electron.Type.giver)
        {
            giver = other.gameObject;
            if (type == Type.none)
            {
                type = Type.giver;
            }
            return;
        }
        receiver = other.gameObject;
        if (type == Type.none)
        {
            type = Type.receiver;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        if (other.GetComponent<Electron>().player == Electron.Type.giver)
        {
            giver = null;
            if (receiver == null)
            {
                type = Type.none;
                return;
            }
            type = Type.receiver;
            return;
        }
        receiver = null;
        if (giver == null)
        {
            type = Type.none;
            return;
        }
        type = Type.giver;
    }
}
