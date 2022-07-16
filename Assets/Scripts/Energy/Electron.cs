using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;
using UnityEngine.VFX;

[RequireComponent(typeof(Energabler))]
public class Electron : MonoBehaviour
{
    public enum Type { giver, receiver };
    public Type player;

    public int health = 1;
    private int originalHeath;
    public double regenerateAfter = 10;
    private double regeneratorClock = 0;

    public bool isDead = false;
    public float time_to_shot_ms = 20;
    public float transfer_distance_limit = 0;
    public float enviro_distance_limit = 6;
    public ParticleSystem shockWaveParticleSystem;

    public Material liveBodyMaterial;
    public Material deadBodyMaterial;

    public GameObject head;

    public Material liveHeadMaterial;
    public Material deadHeadMaterial;

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

    public CameraShake cameraShake;
    public CameraShake cameraShake2;
    private int blockadeMask;
    private int activatorMask;
    private Color originalColor;
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

    public AudioClip collectClip;
    public AudioClip dischargeClip;
    public AudioClip transferClip;

    AudioSource audioSource;

    public LightBulbs lb;

    public float heightToDieAt = -6;
    private Vector3 startingPosition;

    private PlayerInput _playerInput;

    private void Start()
    {
        liveBodyMaterial = GetComponent<MeshRenderer>().material;
        liveHeadMaterial = head.GetComponent<MeshRenderer>().material;
        audioSource = GetComponent<AudioSource>();
        startingPosition = transform.position;

        baseY = followCode.offset.y;
        visualEffect1 = arc1.GetComponent<VisualEffect>();
        visualEffect2 = arc2.GetComponent<VisualEffect>();
        visualEffect3 = arc3.GetComponent<VisualEffect>();
        visualEffect4 = arc4.GetComponent<VisualEffect>();
        visualEffect5 = arc5.GetComponent<VisualEffect>();
        visualEffect6 = arc6.GetComponent<VisualEffect>();
        originalHeath = health;
        size_of_energy = GlobalVars.energy_amount_unit;
        blockadeMask = 1 << LayerMask.NameToLayer("Blockade");
        activatorMask = 1 << LayerMask.NameToLayer("Activator");

        _playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        prevEnergy = GetComponent<Energabler>().energy_units;
        followCode.offset = new Vector3(followCode.offset.x, baseY + prevEnergy*mulYVal, followCode.offset.z);

        if (isDead)
        {
            regeneratorClock = 0;
            return;
        }

        if(regeneratorClock > 0)
        {
            regeneratorClock -= Time.deltaTime;
            
            if(regeneratorClock<=0)
            {
                if (health != originalHeath)
                {
                    health += 1;
                    lb.TurnOnNext();
                    if (health < originalHeath)
                    {
                        regeneratorClock = regenerateAfter;
                    }
                }
            }
        }

        var enviroPressed = _playerInput.actions["InteractObject"].WasPerformedThisFrame();
        var playerPressed = _playerInput.actions["InteractPlayer"].WasPerformedThisFrame();

        if ((enviroPressed || playerPressed) && timer == 0)
        {
            if (player == Type.giver)
            {
                if (enviroPressed)
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
                if (enviroPressed)
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

        if (isPlayerTooLow())
        {
            returnToStart();
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
        lb.TurnOffNext();
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            regeneratorClock = regenerateAfter;
            StartCoroutine(BlinkDamageBody(0.4f, Time.deltaTime));
            StartCoroutine(BlinkDamageHead(0.4f, Time.deltaTime));
            StartCoroutine(DamageBlink(0.4f, Time.deltaTime));
        }
    }

    private IEnumerator BlinkDamageBody(float duration, float delta)
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

    private IEnumerator BlinkDamageHead(float duration, float delta)
    {
        originalColor = head.GetComponent<MeshRenderer>().material.color;
        head.GetComponent<MeshRenderer>().material.color = Color.red;
        float timeLeft = duration;
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(delta);
            timeLeft -= delta;
            float progress = timeLeft / duration;
            head.GetComponent<MeshRenderer>().material.color = Color.Lerp(originalColor, Color.red, progress);
        }
        head.GetComponent<MeshRenderer>().material.color = originalColor;
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
        GetComponent<MeshRenderer>().material = deadBodyMaterial;
        head.GetComponent<MeshRenderer>().material = deadHeadMaterial;
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
        lb.TurnOnAll();
        health = originalHeath;
        isDead = false;
        GetComponent<MeshRenderer>().material = liveBodyMaterial;
        head.GetComponent<MeshRenderer>().material = liveHeadMaterial;
        prevEnergy = 0;
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
            audioSource.PlayOneShot(collectClip);
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
        Energabler energabler = GetNearestEnergable(full_acc: false, electron: false, cable: false, isTransfer: true);
        if (energabler == null)
        {
            return;
        }

        if (energabler.GetComponent<Cable>() != null && energabler.GetComponent<Cable>().IsGoodToTransfer())
        {
            return;
        }

        RaycastHit hit;
        float distance = Vector3.Distance(transform.position, energabler.transform.position);
        bool isColliding = Physics.Raycast(transform.position, energabler.transform.position - transform.position, out hit, distance, blockadeMask);
        if (isColliding)
        {
            return;
        }

        if (GetComponent<Energabler>().RemEnergy(size_of_energy))
        {
            audioSource.PlayOneShot(dischargeClip);
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
        bool isColliding = Physics.Raycast(transform.position, elec.transform.position - transform.position, out hit, distance, blockadeMask | activatorMask);
        if (isColliding)
        {
            return;
        }

        bool isElec = elec.gameObject.GetComponent<Electron>() != null;

        if (isElec)
        {
            if (elec.gameObject.GetComponent<Electron>().isDead)
            {
                return;
            }
        }

        if ((isElec
            || (elec.gameObject.GetComponent<Cable>() != null && elec.GetComponent<Cable>().IsGoodToTransfer()))
            && GetComponent<Energabler>().RemEnergy(size_of_energy))
        {
            audioSource.PlayOneShot(transferClip);


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

            StartCoroutine(cameraShake.Shake(0.04f, 0.1f));
            StartCoroutine(cameraShake2.Shake(0.07f, 0.2f));
        }
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

    private bool isPlayerTooLow()
    {
        return transform.position.y <= heightToDieAt;
    }
    
    private void returnToStart()
    {
        transform.position = startingPosition;
    }
}
