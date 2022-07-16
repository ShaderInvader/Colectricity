using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class help : MonoBehaviour
{
    public GameObject t1;


    public int i = 0;


    void Start()
    {
       
    }

    void Update()
    {
    if (Input.GetKeyDown(KeyCode.H))
    {
            i++;
        if(i==2)
            {
                i= 0;
            }
    }

    if (i== 0)
        {
            t1.SetActive(false);
        }
    if (i == 1)
    {
        t1.SetActive(true);
    }


}
}
