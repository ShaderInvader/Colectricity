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

    public void StartGame()
    {
        StartCoroutine(LoadAsynchronously(startLevelName));
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
