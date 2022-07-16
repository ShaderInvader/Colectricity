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

    private List<Collider> players;

    void Start()
    {
        players = new List<Collider>();
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

        int sum = sumEnergyForPlayersOnButton();
        bool shouldBeTurnedOn = false;

        switch (button_type)
        {
            case 1:
                if (sum >= 1)
                {
                    Debug.Log(2 + " " + sum + " " + players.Count);
                    shouldBeTurnedOn = true;
                }
                break;

            case 2:
                if (sum >= 2)
                {
                    Debug.Log(3 + " " + sum + " " + players.Count);
                    shouldBeTurnedOn = true;
                }
                break;

            default:
                if (players.Count > 0)
                {
                    Debug.Log(1 + " " + sum + " " + players.Count);
                    shouldBeTurnedOn = true;
                }
                break;
        }

        if(shouldBeTurnedOn)
        {
            buttonIsTurnedOn();
        }
        else
        {
            buttonIsTurnedOff();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("entr");
        if (!players.Contains(other) && other.tag == "Player")
        {
            players.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exit");
        players.Remove(other);
    }

    private int sumEnergyForPlayersOnButton()
    {
        int sum = 0;
        players.ForEach(player =>
        {
            Collider other = player.GetComponent<Collider>();
            sum = sum + other.GetComponent<Energabler>().energy_units;
        });
        return sum;
    }

    private void buttonIsTurnedOn()
    {
        emission.SetActive(true);
        flag = true;

        if (cable)
        {
            cable.material.SetColor(EmissiveColor, Color.Lerp(disabledEmissiveColor, enabledEmissiveColor, 1.0f));
            cable.material.SetColor(DetailColor, Color.Lerp(disabledDetailColor, enabledDetailColor, 1.0f));
        }
    }

    private void buttonIsTurnedOff()
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
