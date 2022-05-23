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

    public int button_type=0;

    public int if_fan = 0;

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
        if (if_fan==1&&(flag || energabler.GetComponent<Energabler>().energy_units == 2))
        {
            float step = speed * Time.deltaTime;

            platform.transform.position = Vector3.MoveTowards(platform.transform.position, platform_position2.position, step);


        }
        else if (if_fan==0 && flag)
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
        if(button_type==0)
        {
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
        else if (button_type==1)
        {
            if (other.tag == "Player")
            {
                if(other.GetComponent<Energabler>().energy_units >= 1)
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
                else if (other.GetComponent<Energabler>().energy_units < 1)
                {
                    emission.SetActive(false);

                    flag = false;
                    if (cable)
                    {
                        cable.material.SetColor(EmissiveColor, Color.Lerp(disabledEmissiveColor, enabledEmissiveColor, 0.0f));
                        cable.material.SetColor(DetailColor, Color.Lerp(disabledDetailColor, enabledDetailColor, 0.0f));
                    }
                }

            }
        }
        else if (button_type == 2)
        {
            if (other.tag == "Player")
            {
                if (other.GetComponent<Energabler>().energy_units >=2)
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
                else if (other.GetComponent<Energabler>().energy_units < 2)
                {
                    emission.SetActive(false);

                    flag = false;
                    if (cable)
                    {
                        cable.material.SetColor(EmissiveColor, Color.Lerp(disabledEmissiveColor, enabledEmissiveColor, 0.0f));
                        cable.material.SetColor(DetailColor, Color.Lerp(disabledDetailColor, enabledDetailColor, 0.0f));
                    }
                }

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
