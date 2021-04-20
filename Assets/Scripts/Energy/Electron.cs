using UnityEngine;

[RequireComponent(typeof(Energabler))]
[RequireComponent(typeof(SelectKeys))]
public class Electron : MonoBehaviour
{
    public enum Type { giver, receiver };
    public Type player;

    KeyCode action_key;
    float timer=0;
    private void Start()
    {
        bool isWSAD = gameObject.GetComponent<SelectKeys>().selection == SelectKeys.Keys.wsad;
        action_key = isWSAD ? KeyCode.LeftShift : KeyCode.RightShift;
    }

    void Update()
    {
        if (Input.GetKey(action_key) && timer==0)
        {
            if(player==Type.giver)
            {
                Give();
            }
            else
            {
                Receive();
            }
            timer += 1;
        }
        timer -= Time.deltaTime;
        timer = timer < 0 ? 0 : timer;
    }

    void Receive()
    {
        Energabler elec = GetNearestEnergable(full_acc: false);
        if(elec.gameObject.GetComponent<Electron>() != null && GetComponent<Energabler>().RemEnergy())
        {
            elec.AddEnergy();  
        }
        else if (GetComponent<Energabler>().AddEnergy())
        {
            Energabler temp_e = GetNearestEnergable(empty_acc: false);
            temp_e.RemEnergy();
        }
    }

    void Give()
    {
        if (GetComponent<Energabler>().RemEnergy())
        {
            Energabler temp_e = GetNearestEnergable(full_acc: false);
            temp_e.AddEnergy();
        }
    }

    Energabler GetNearestEnergable(bool full_acc=true, bool empty_acc=true)
    {
        Energabler[] energables = FindObjectsOfType<Energabler>();
        Energabler eMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Energabler e in energables)
        {
            if((!full_acc && e.IsFull()) || (!empty_acc && e.IsEmpty()) || e.gameObject.GetInstanceID()==gameObject.GetInstanceID())
            {
                continue;
            }
            float dist = Vector3.Distance(e.transform.position, currentPos);
            if (dist < minDist)
            {
                eMin = e;
                minDist = dist;
            }
        }
        return eMin;
    }
}
