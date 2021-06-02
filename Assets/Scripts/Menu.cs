using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
