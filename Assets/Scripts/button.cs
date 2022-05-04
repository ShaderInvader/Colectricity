using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{


    public GameObject emission;

    public GameObject platform;

    public Transform platform_position1;
    public Transform platform_position2;

    public bool flag = false;

    public float speed = 255;

    public GameObject energabler;


    void Start()
    {
        emission.SetActive(false);
    }

    void Update()
    {

        if (flag|| energabler.GetComponent<Energabler>().energy_units == 2)
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
        }
    
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exit");
        if (other.tag == "Player")
        {
            emission.SetActive(false);
            flag = false;

        }
    
    }


}
