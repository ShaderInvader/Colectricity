using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

[RequireComponent(typeof(Energabler))]
[RequireComponent(typeof(SelectKeys))]
public class Electron : MonoBehaviour
{
    public enum Type { giver, receiver };
    public Type player;
    public int health = 1;
    public bool isDead = false;
    public float time_to_shot_ms = 20;
    public float distance_limit = 5;
    public ParticleSystem shockWaveParticleSystem;

    public Material liveMaterial;
    public Material deadMaterial;

    int size_of_energy;
    KeyCode enviro_key, player_key;
    float timer=0;

    private int originalHeath;
    public CameraShake cameraShake;
    private int blockadeMask;
    private Color originalColor;

    private void Start()
    {
        originalHeath = health;
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
        else
        {
            StartCoroutine(BlinkDamage(0.4f, Time.deltaTime));
            StartCoroutine(DamageBlink(0.4f, Time.deltaTime));
        }
    }

    private IEnumerator BlinkDamage(float duration, float delta)
    {
        originalColor = GetComponent<MeshRenderer>().material.color;
        GetComponent<MeshRenderer>().material.color = Color.red;
        float timeLeft = duration;
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(delta);
            timeLeft -= delta;
            float progress = timeLeft / duration;
            GetComponent<MeshRenderer>().material.color = Color.Lerp(originalColor, Color.red, progress);
        }
        GetComponent<MeshRenderer>().material.color = originalColor;
    }

    private IEnumerator DamageBlink(float duration, float delta)
    {
        Camera[] cameras = FindObjectsOfType<Camera>();
        Camera mutualCam = null;
        Camera electronsCam = null;

        foreach (Camera cam in cameras)
        {
            CameraFollow cf = cam.GetComponent<CameraFollow>();
            if (cf == null)
            {
                mutualCam = cam;
            }
            else if (ReferenceEquals(cf.parent.gameObject, gameObject))
            {
                electronsCam = cam;
            }
        }
        if (electronsCam == null)
        {
            yield break;
        }

        Image mutualImg = mutualCam == null ? null : mutualCam.GetComponentInChildren<Image>();
        Image electronsImage = electronsCam.GetComponentInChildren<Image>();

        float r = electronsImage.color.r, g = electronsImage.color.g, b = electronsImage.color.b;

        float timeLeft = duration;

        if (mutualImg != null)
        {
            mutualImg.color = new Color(r, g, b, 1);
        }
        electronsImage.color = new Color(r, g, b, 1);

        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(delta);
            timeLeft -= delta;
            float progress = timeLeft / duration;
            if (mutualImg != null)
            {
                mutualImg.color = new Color(r, g, b, progress);
            }
            electronsImage.color = new Color(r, g, b, progress);
        }
        if (mutualImg != null)
        {
            mutualImg.color = new Color(r, g, b, 0);
        }
        electronsImage.color = new Color(r, g, b, 0);
    }

    void Die()
    {
        isDead = true;
        GetComponent<DeathMinigame>().enabled = true;
        GetComponent<MeshRenderer>().material = deadMaterial;
        ResetIfAllDead();
    }

    public void ResetIfAllDead()
    {
        Electron[] electrons = FindObjectsOfType<Electron>();
        foreach (Electron e in electrons)
        {
            if (!e.isDead)
            {
                return;
            }
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Reborn()
    {
        health = originalHeath;
        isDead = false;
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
