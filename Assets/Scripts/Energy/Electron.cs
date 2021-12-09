using System;
using UnityEngine;

[RequireComponent(typeof(Energabler))]
[RequireComponent(typeof(SelectKeys))]
public class Electron : MonoBehaviour
{
    public enum Type { giver, receiver };
    public Type player;
    public int health = 1;
    public bool dead = false;
    public float time_to_shot_ms = 20;
    public float distance_limit = 5;
    public ParticleSystem shockWaveParticleSystem;

    public Material liveMaterial;
    public Material deadMaterial;

    int size_of_energy;
    KeyCode enviro_key, player_key;
    float timer=0;

    public CameraShake cameraShake;
    private int blockadeMask;

    private void Start()
    {
        size_of_energy = GlobalVars.energy_amount_unit;
        bool isWSAD = gameObject.GetComponent<SelectKeys>().selection == SelectKeys.Keys.wsad;
        enviro_key = isWSAD ? KeyCode.E : KeyCode.Slash;
        player_key = isWSAD ? KeyCode.Q : KeyCode.Quote;
        blockadeMask = 1 << LayerMask.NameToLayer("Blockade");
    }

    void Update()
    {
        bool enviro_pressed = Input.GetKey(enviro_key), player_pressed = Input.GetKey(player_key);

        if ((enviro_pressed || player_pressed) && timer==0)
        {
            if(player==Type.giver)
            {
                if(enviro_pressed)
                {
                    Give();
                }
                else
                {
                    TransferToElectron();
                }
            }
            else
            {
                if (enviro_pressed)
                {
                    Receive();
                }
                else
                {
                    TransferToElectron();
                }
            }
            timer += time_to_shot_ms/1000;
        }
        timer -= Time.deltaTime;
        timer = timer < 0 ? 0 : timer;
    }

    public void ReceiveDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        dead = true;
        GetComponent<DeathMinigame>().enabled = true;
        GetComponent<MeshRenderer>().material = deadMaterial;
    }

    public void Reborn()
    {
        dead = false;
        GetComponent<MeshRenderer>().material = liveMaterial;
    }

    void Receive()
    {
        Energabler energabler = GetNearestEnergable(empty_acc: false, electron:false, cable: false);
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
        Energabler energabler = GetNearestEnergable(full_acc: false, electron: false, cable: false);
        if (energabler == null)
        {
            return;
        }
        else if (energabler.GetComponent<Cable>()!=null && energabler.GetComponent<Cable>().IsGoodToTransfer())
        {
            return;
        }
        else if (GetComponent<Energabler>().RemEnergy(size_of_energy))
        {
            shockWaveParticleSystem.Play();
            energabler.AddEnergy(size_of_energy);
            RenderLine(energabler.transform);
        }

        StartCoroutine(cameraShake.Shake(0.07f, 0.2f));
    }

    void TransferToElectron()
    {
        Energabler elec = GetNearestEnergable(full_acc: false);
        if (elec == null)
        {
            return;
        }

        RaycastHit hit;
        float distance = Vector3.Distance(transform.position, elec.transform.position);
        bool isColliding = Physics.Raycast(transform.position, elec.transform.position - transform.position, out hit, distance, blockadeMask);
        if (isColliding)
        {
            return;
        }
        if ((elec.gameObject.GetComponent<Electron>() != null
            || (elec.gameObject.GetComponent<Cable>() != null && elec.GetComponent<Cable>().IsGoodToTransfer()))
            && GetComponent<Energabler>().RemEnergy(size_of_energy))
        {
            shockWaveParticleSystem.Play();
            elec.AddEnergy(size_of_energy);
            RenderLine(elec.transform);
            return;
        }
    }

    Energabler GetNearestEnergable(bool full_acc=true, bool empty_acc=true, bool cable=true, bool electron=true)
    {
        Energabler[] energables = FindObjectsOfType<Energabler>();
        Energabler eMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        bool theSameObj, isNotQualified, goodType;
        foreach (Energabler e in energables)
        {
            isNotQualified = (!full_acc && e.IsFull(size_of_energy)) || (!empty_acc && e.IsEmpty(size_of_energy));
            goodType = (electron || e.gameObject.GetComponent<Electron>() == null) && (cable || e.gameObject.GetComponent<Cable>() == null);
            theSameObj = e.gameObject.GetInstanceID() == gameObject.GetInstanceID();
            if (isNotQualified || theSameObj || !goodType)
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
