using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;
using UnityEngine.VFX;

[RequireComponent(typeof(Energabler))]
[RequireComponent(typeof(SelectKeys))]
public class Electron : MonoBehaviour
{
    public enum Type { giver, receiver };
    public Type player;
    public int health = 1;
    public bool isDead = false;
    public float time_to_shot_ms = 20;
    public float transfer_distance_limit = 0;
    public float enviro_distance_limit = 6;
    public ParticleSystem shockWaveParticleSystem;

    public Material liveMaterial;
    public Material deadMaterial;

    public double intensityPower = 0.5;

    public GameObject arc1;
    public GameObject arc2;
    public GameObject arc3;
    public GameObject arc4;
    public GameObject arc5;
    public GameObject arc6;
    public GameObject arc6_entry;

    public UpdateEmmision[] wheretoupdateenergy;

    int size_of_energy;
    float timer = 0;

    private int originalHeath;
    public CameraShake cameraShake;
    public CameraShake cameraShake2;
    private int blockadeMask;
    private Color originalColor;
    private SelectKeys selectKeys;
    private int prevEnergy;
    private VisualEffect visualEffect1;
    private VisualEffect visualEffect2;
    private VisualEffect visualEffect3;
    private VisualEffect visualEffect4;
    private VisualEffect visualEffect5;
    private VisualEffect visualEffect6;

    public SmoothFollowParent followCode;
    float baseY;
    public float mulYVal;


    private void Start()
    {
        baseY = followCode.offset.y;
        visualEffect1 = arc1.GetComponent<VisualEffect>();
        visualEffect2 = arc2.GetComponent<VisualEffect>();
        visualEffect3 = arc3.GetComponent<VisualEffect>();
        visualEffect4 = arc4.GetComponent<VisualEffect>();
        visualEffect5 = arc5.GetComponent<VisualEffect>();
        visualEffect6 = arc6.GetComponent<VisualEffect>();
        selectKeys = GetComponent<SelectKeys>();
        originalHeath = health;
        size_of_energy = GlobalVars.energy_amount_unit;
        bool isWSAD = gameObject.GetComponent<SelectKeys>().selection == SelectKeys.Keys.left;
        blockadeMask = 1 << LayerMask.NameToLayer("Blockade");
    }

    void Update()
    {
        prevEnergy = GetComponent<Energabler>().energy_units;
        followCode.offset = new Vector3(followCode.offset.x, baseY + prevEnergy*mulYVal, followCode.offset.z);
        bool enviro_pressed = selectKeys.Env, player_pressed = selectKeys.Play;

        if ((enviro_pressed || player_pressed) && timer == 0)
        {
            if (player == Type.giver)
            {
                if (enviro_pressed)
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
            timer += time_to_shot_ms / 1000;
        }
        timer -= Time.deltaTime;
        timer = timer < 0 ? 0 : timer;
    }

    public void UpdateEmission(bool reborn=false)
    {
        int nextEnergy = GetComponent<Energabler>().energy / GlobalVars.energy_amount_unit;
        double emmisionIntensity = Math.Pow((nextEnergy + 0.01f) / (prevEnergy + 0.01f), intensityPower);
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        Color currColor = mr.material.GetColor("_EmissiveColor");
        mr.material.SetColor("_EmissiveColor", currColor * (float)emmisionIntensity);

        if(reborn)
        {
            return;
        }

        foreach (UpdateEmmision ue in wheretoupdateenergy)
        {
            ue.UpdateValue(GetComponent<Energabler>().energy, prevEnergy, intensityPower);
        }
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
        prevEnergy = 0;
        UpdateEmission(true);
    }

    void Receive()
    {
        Energabler energabler = GetNearestEnergable(empty_acc: false, electron: false, cable: false);
        if (energabler == null)
        {
            return;
        }
        else if (GetComponent<Energabler>().AddEnergy(size_of_energy))
        {
            shockWaveParticleSystem.Play();
            StartCoroutine(cameraShake.Shake(0.07f, 0.2f));
            StartCoroutine(cameraShake2.Shake(0.07f, 0.2f));
            arc6_entry.transform.position = energabler.transform.position;
            visualEffect6.Play();

            energabler.RemEnergy(size_of_energy);
            //RenderLine(energabler.transform);
        }
        UpdateEmission();
    }

    void Give()
    {
        Energabler energabler = GetNearestEnergable(full_acc: false, electron: false, cable: false);
        if (energabler == null)
        {
            return;
        }
        else if (energabler.GetComponent<Cable>() != null && energabler.GetComponent<Cable>().IsGoodToTransfer())
        {
            return;
        }
        else if (GetComponent<Energabler>().RemEnergy(size_of_energy))
        {
            arc1.transform.position = energabler.transform.position;
            arc2.transform.position = energabler.transform.position;
            arc3.transform.position = energabler.transform.position;
            visualEffect1.Play();
            visualEffect2.Play();
            visualEffect3.Play();
            shockWaveParticleSystem.Play();
            StartCoroutine(cameraShake.Shake(0.07f, 0.2f));
            StartCoroutine(cameraShake2.Shake(0.07f, 0.2f));
            energabler.AddEnergy(size_of_energy);
            //RenderLine(energabler.transform);
        }
        UpdateEmission();
        
    }

    void TransferToElectron()
    {
        Energabler elec = GetNearestEnergable(full_acc: false, isTransfer: true);
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

        bool isElec = elec.gameObject.GetComponent<Electron>() != null;
        if ((isElec
            || (elec.gameObject.GetComponent<Cable>() != null && elec.GetComponent<Cable>().IsGoodToTransfer()))
            && GetComponent<Energabler>().RemEnergy(size_of_energy))
        {
            if (player == Type.giver)
            {
                arc4.transform.position = elec.transform.position;
                visualEffect4.Play();
                shockWaveParticleSystem.Play();
            }
            else
            {
                arc5.transform.position = elec.transform.position;
                visualEffect5.Play();
            }

            elec.AddEnergy(size_of_energy);
            //RenderLine(elec.transform);
            UpdateEmission();
            if (isElec)
            {
                elec.GetComponent<Electron>().UpdateEmission();
            }
        }

            StartCoroutine(cameraShake.Shake(0.04f, 0.1f));
            StartCoroutine(cameraShake2.Shake(0.07f, 0.2f));

    }

    Energabler GetNearestEnergable(bool full_acc = true, bool empty_acc = true, bool cable = true, bool electron = true, bool isTransfer = false)
    {
        float distance_limit = isTransfer ? transfer_distance_limit : enviro_distance_limit;
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
        if ((distance_limit > 0 && minDist > distance_limit) 
            || (eMin.GetComponent<Cable>() != null && eMin.GetComponent<Cable>().getRangeOfConnection() > 0 && minDist > eMin.GetComponent<Cable>().getRangeOfConnection()))
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
