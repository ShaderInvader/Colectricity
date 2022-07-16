using UnityEngine;
public class Energabler : MonoBehaviour
{
    public int energy_units;
    public int max_energy_units;
    [HideInInspector]
    public int energy;
    [HideInInspector]
    public int max_energy;

    new MeshRenderer renderer;
    Transform[] children;

    private void Start()
    {
        energy = energy_units * GlobalVars.energy_amount_unit;
        max_energy = max_energy_units * GlobalVars.energy_amount_unit;
        if (GetComponent<MeshRenderer>())
        {
            renderer = GetComponent<MeshRenderer>();
            renderer.materials[0].SetFloat("_EmissiveIntensity", 0.5f);
        }
        children = new Transform[transform.childCount];
        int i = 0;
        foreach (Transform T in transform)
        {
            children[i] = T;
            i++;
        }
    }

    private void Update()
    {
        energy_units = energy / GlobalVars.energy_amount_unit;
        if (gameObject.tag == "Container")
        {
            Color cEmissive = new Color(93, 203, 255);
            renderer.materials[0].SetColor("_EmissiveColor", Color.Lerp(Color.black, cEmissive, 0.01f * ((float)energy) / max_energy));
        }
        else if (gameObject.tag == "Player") // players
        {
            float s = 1.5f * ((float)energy) / max_energy + 1;
            transform.localScale = new Vector3(s, s, s);
        }
    }

    public void AttachChildren()
    {
        for(int i=0; i<children.Length; i++)
        {
            children[i].parent = transform;
        }
    }

    public bool AddEnergy(int size_of_energy=1)
    {
        if (IsFull(size_of_energy))
        {
            return false;
        }
        energy += size_of_energy;
        return true;
    }

    public bool RemEnergy(int size_of_energy=1)
    {
        if (IsEmpty(size_of_energy))
        {
            return false;
        }
        energy -= size_of_energy;
        return true;
    }

    public bool IsFull(int size=1)
    {
        return energy+size > max_energy;
    }

    public bool IsEmpty(int size=1)
    {
        return energy - size < 0;
    }

    public static void UpdateEmissiveColorFromIntensityAndEmissiveColorLDR(Material material)
    {
        const string kEmissiveColorLDR = "_EmissiveColorLDR";
        const string kEmissiveColor = "_EmissiveColor";
        const string kEmissiveIntensity = "_EmissiveIntensity";

        if (material.HasProperty(kEmissiveColorLDR) && material.HasProperty(kEmissiveIntensity) && material.HasProperty(kEmissiveColor))
        {
            Color emissiveColorLDR = material.GetColor(kEmissiveColorLDR);
            Color emissiveColorLDRLinear = new Color(Mathf.GammaToLinearSpace(emissiveColorLDR.r), Mathf.GammaToLinearSpace(emissiveColorLDR.g), Mathf.GammaToLinearSpace(emissiveColorLDR.b));
            material.SetColor(kEmissiveColor, emissiveColorLDRLinear * material.GetFloat(kEmissiveIntensity));
        }
    }
}
