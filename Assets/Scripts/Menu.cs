using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Text percentage;
    public string startLevelName;

    public GameObject startButton;
    public GameObject quitButton;
    public GameObject creditsButton;

    public GameObject controllerDialog;
    public GameObject creditsDialog;

    public EventSystem wEvents;

    public void Update()
    {
        wEvents.SetSelectedGameObject(null);

        if (Input.GetKey(KeyCode.Escape) && controllerDialog.activeSelf)
        {
            getBackFromControllerDialog();
        }
        else if(Input.GetKey(KeyCode.Escape) && creditsDialog.activeSelf)
        {
            getBackFromCreditsDialog();
        }

        Destroy(GameObject.FindWithTag("music"));
    }
    public void StartGame()
    {
        StartCoroutine(LoadAsynchronously(startLevelName));
    }

    public void pickBoth()
    {
        pickController(ControllerInfo.controllerEnum.BOTH);
    }

    public void pickKeyboard()
    {
        pickController(ControllerInfo.controllerEnum.KEYBOARD);
    }

    public void pickPad()
    {
        pickController(ControllerInfo.controllerEnum.PAD);
    }

    public void pickController(ControllerInfo.controllerEnum controllerType)
    {
        ControllerInfo.controllerPick = controllerType;
        StartGame();
    }

    public void showControllerDialog()
    {
        controllerDialog.SetActive(true);
        startButton.SetActive(false);
        quitButton.SetActive(false);
        creditsButton.SetActive(false);
    }

    public void showCreditsDialog()
    {
        controllerDialog.SetActive(false);
        startButton.SetActive(false);
        quitButton.SetActive(false);
        creditsButton.SetActive(false);
        creditsDialog.SetActive(true);
    }

    public void getBackFromControllerDialog()
    {
        controllerDialog.SetActive(false);
        startButton.SetActive(true);
        quitButton.SetActive(true);
        creditsButton.SetActive(true);
    }

    public void getBackFromCreditsDialog()
    {
        creditsDialog.SetActive(false);
        startButton.SetActive(true);
        quitButton.SetActive(true);
        creditsButton.SetActive(true);
    }

    IEnumerator LoadAsynchronously(string sceneIndex)
    {
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            percentage.text = progress * 100 + "%";
            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
