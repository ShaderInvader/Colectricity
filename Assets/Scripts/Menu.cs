using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Text percentage;
    public string startLevelName;

    public GameObject startButton;
    public GameObject quitButton;

    public GameObject controllerDialog;

    public void Start()
    {
        controllerDialog.SetActive(false);
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
