using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
    public MeshRenderer cable;
    [Space]
    [ColorUsage(false, true)] public Color disabledEmissiveColor;
    [ColorUsage(false, true)] public Color enabledEmissiveColor;
    [Space]
    [ColorUsage(false, true)] public Color disabledDetailColor;
    [ColorUsage(false, true)] public Color enabledDetailColor;

    public GameObject emission;

    public GameObject platform;

    public Transform platform_position1;
    public Transform platform_position2;

    public bool flag = false;

    public float speed = 255;

    public GameObject energabler;

    private static readonly int EmissiveColor = Shader.PropertyToID("Emissive_Color");
    private static readonly int DetailColor = Shader.PropertyToID("Detail_Color");

    void Start()
    {
        emission.SetActive(false);
        // HACK HACK HACK THIS IS A HACK PLEASE MAKE IT MORE CIVILIZED IN THE FUTURE
        if (cable)
        {
            cable.material.SetColor(EmissiveColor, Color.Lerp(disabledEmissiveColor, enabledEmissiveColor, 0.0f));
            cable.material.SetColor(DetailColor, Color.Lerp(disabledDetailColor, enabledDetailColor, 0.0f));
        }
    }

    void Update()
    {
        if (flag || energabler.GetComponent<Energabler>().energy_units == 2)
        {
            float step = speed * Time.deltaTime;

            platform.transform.position = Vector3.MoveTowards(platform.transform.position, platform_position2.position, step);


        }
        else
        {
            float step = speed * Time.deltaTime;

            platform.transform.position = Vector3.MoveTowards(platform.transform.position, platform_position1.position, step);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("entr");
        if (other.tag == "Player")
        {
            emission.SetActive(true);

            flag = true;

            // HACK HACK HACK THIS IS A HACK PLEASE MAKE IT MORE CIVILIZED IN THE FUTURE
            if (cable)
            {
                cable.material.SetColor(EmissiveColor, Color.Lerp(disabledEmissiveColor, enabledEmissiveColor, 1.0f));
                cable.material.SetColor(DetailColor, Color.Lerp(disabledDetailColor, enabledDetailColor, 1.0f));
            }
        }
    
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exit");
        if (other.tag == "Player")
        {
            emission.SetActive(false);
            flag = false;

            // HACK HACK HACK THIS IS A HACK PLEASE MAKE IT MORE CIVILIZED IN THE FUTURE
            if (cable)
            {
                cable.material.SetColor(EmissiveColor, Color.Lerp(disabledEmissiveColor, enabledEmissiveColor, 0.0f));
                cable.material.SetColor(DetailColor, Color.Lerp(disabledDetailColor, enabledDetailColor, 0.0f));
            }
        }
    
    }


}
