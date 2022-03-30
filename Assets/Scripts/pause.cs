using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class pause : MonoBehaviour
{
    public GameObject pause_ui;
    bool is_paused=false;
    public EventSystem wEvents;

    void Start()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(is_paused==false)
            {
                is_paused = true;
                pause_ui.SetActive(true);
                Time.timeScale = 0;
                Cursor.visible = true;
            }
            else
            {
                is_paused = false;
                pause_ui.SetActive(false);
                Time.timeScale = 1;
                Cursor.visible = false;
            }

        }
        wEvents.SetSelectedGameObject(null);


    }

    public void resume_game()
    {
        is_paused = false;
        pause_ui.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
    }

    public void quit_game()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }


}
