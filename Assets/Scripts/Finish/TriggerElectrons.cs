using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerElectrons : MonoBehaviour
{
    public enum Type { None, Giver, Receiver };
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
            if (type == Type.None)
            {
                type = Type.Giver;
                fl.Add(Type.Giver);
            }
            return;
        }
        receiver = other.gameObject;
        if (type == Type.None)
        {
            type = Type.Receiver;
            fl.Add(Type.Receiver);
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
