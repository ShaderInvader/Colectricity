using UnityEngine;

[RequireComponent(typeof(Energabler))]
[RequireComponent(typeof(SelectKeys))]
public class Electron : MonoBehaviour
{
    public enum Type { giver, receiver };
    public Type player;
    public float time_to_shot_ms = 20;
    public int size_of_energy = 20;
    public float distance_limit = 5;
    public ParticleSystem shockWaveParticleSystem;

    KeyCode action_key;
    float timer=0;
    private void Start()
    {
        bool isWSAD = gameObject.GetComponent<SelectKeys>().selection == SelectKeys.Keys.wsad;
        action_key = isWSAD ? KeyCode.E : KeyCode.Slash;
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
            timer += time_to_shot_ms/1000;
        }
        timer -= Time.deltaTime;
        timer = timer < 0 ? 0 : timer;
    }

    void Receive()
    {
        Energabler elec = GetNearestEnergable(full_acc: false);
        if (elec != null && (elec.gameObject.GetComponent<Electron>() != null 
            || (elec.gameObject.GetComponent<Cable>() != null && elec.GetComponent<Cable>().IsGoodToTransfer())) 
            && GetComponent<Energabler>().RemEnergy(size_of_energy))
        {
            shockWaveParticleSystem.Play();
            elec.AddEnergy(size_of_energy);
            RenderLine(elec.transform);
            return;
        }

        Energabler energabler = GetNearestEnergable(empty_acc: false);
        if (energabler == null)
        {
            return;
        }
        else if (GetComponent<Energabler>().AddEnergy(size_of_energy))
        {
            shockWaveParticleSystem.Play();
            energabler.RemEnergy(size_of_energy);
            RenderLine(energabler.transform);
        }
    }

    void Give()
    {
        Energabler energabler = GetNearestEnergable(full_acc: false);
        if (energabler == null)
        {
            return;
        }
        else if (energabler.GetComponent<Cable>()!=null && !energabler.GetComponent<Cable>().IsGoodToTransfer())
        {
            return;
        }
        else if (GetComponent<Energabler>().RemEnergy(size_of_energy))
        {
            shockWaveParticleSystem.Play();
            energabler.AddEnergy(size_of_energy);
            RenderLine(energabler.transform);
        }
    }

    Energabler GetNearestEnergable(bool full_acc=true, bool empty_acc=true)
    {
        Energabler[] energables = FindObjectsOfType<Energabler>();
        Energabler eMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        bool theSameObj, isNotQualified;
        foreach (Energabler e in energables)
        {
            isNotQualified = (!full_acc && e.IsFull(size_of_energy)) || (!empty_acc && e.IsEmpty(size_of_energy));
            theSameObj = e.gameObject.GetInstanceID() == gameObject.GetInstanceID();
            if (isNotQualified || theSameObj)
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
        if(distance_limit>0 && minDist>distance_limit)
        {
            return null;
        }
        return eMin;
    }

    private void RenderLine(Transform target)
    {
        GameObject go = new GameObject();
        LineRenderer lr = go.AddComponent<LineRenderer>();

        lr.startWidth = .1f;
        lr.endWidth = .1f;

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, target.position);
    }
}
