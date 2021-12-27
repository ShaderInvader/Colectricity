using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerElectrons : MonoBehaviour
{
    public enum Type { None, Giver, Receiver, All };
    [HideInInspector]
    public Type type = Type.None;
    private GameObject receiver = null;
    private GameObject giver = null;
    private FinishLevel fl;

    void Start()
    {
        fl = GetComponentInParent<FinishLevel>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        if (other.GetComponent<Electron>().player == Electron.Type.giver)
        {
            giver = other.gameObject;
            fl.Add(Type.Giver);
            if (type == Type.None)
            {
                type = Type.Giver;
                return;
            }
            type = Type.All;
            return;
        }
        receiver = other.gameObject;
        fl.Add(Type.Receiver);
        if (type == Type.None)
        {
            type = Type.Receiver;
            return;
        }

        type = Type.All;
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
            fl.Remove(Type.Giver);
            if (receiver == null)
            {
                type = Type.None;
                return;
            }
            type = Type.Receiver;
            fl.Add(Type.Receiver);
            return;
        }
        receiver = null;
        fl.Remove(Type.Receiver);
        if (giver == null)
        {
            type = Type.None;
            return;
        }
        type = Type.Giver;
        fl.Add(Type.Giver);
    }
}
