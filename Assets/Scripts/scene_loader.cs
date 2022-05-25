using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class scene_loader : MonoBehaviour
{

    public string sceneName;
    public bool changeLevel = true;
    public int count = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if(count==2 && changeLevel)
        {
            SceneManager.LoadScene(sceneName); 
        }

        if (Input.GetKeyDown("0"))
        {
            SceneManager.LoadScene("0_tutorial");
        }
        if (Input.GetKeyDown("1"))
        {
            SceneManager.LoadScene("1");
        }
        if (Input.GetKeyDown("2"))
        {
            SceneManager.LoadScene("2");
        }
        if (Input.GetKeyDown("3"))
        {
            SceneManager.LoadScene("3");
        }
        if (Input.GetKeyDown("4"))
        {
            SceneManager.LoadScene("4");
        }
        if (Input.GetKeyDown("5"))
        {
            SceneManager.LoadScene("5_tutorial_enemy");
        }
        if (Input.GetKeyDown("6"))
        {
            SceneManager.LoadScene("6");
        }
        if (Input.GetKeyDown("7"))
        {
            SceneManager.LoadScene("7");
        }
        if (Input.GetKeyDown("8"))
        {
            SceneManager.LoadScene("8");
        }
        if (Input.GetKeyDown("9"))
        {
            SceneManager.LoadScene("9");
        }
        if (Input.GetKeyDown("-"))
        {
            SceneManager.LoadScene("thankyouscene");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            count++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            count--;
        }
    }
}
