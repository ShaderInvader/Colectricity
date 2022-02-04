using UnityEngine;

[RequireComponent(typeof(Energabler))]
public class ShowLightning : MonoBehaviour
{
    public SpriteRenderer img;
    private Energabler en;
    void Start()
    {
        en = GetComponent<Energabler>();
    }
    
    void Update()
    {
        if (en.energy_units == en.max_energy_units)
        {
            img.enabled = true;
        }
        else
        {
            img.enabled = false;
        }
    }
}
