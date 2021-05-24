using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class scene_loader : MonoBehaviour
{

    public string sceneName;
    public int count = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if(count==2)
        {
            SceneManager.LoadScene(sceneName); 
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
            SceneManager.LoadScene("5");

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
        if (Input.GetKeyDown("z"))
        {
            SceneManager.LoadScene("10");
        }
        if (Input.GetKeyDown("x"))
        {
            SceneManager.LoadScene("11");
        }
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
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
