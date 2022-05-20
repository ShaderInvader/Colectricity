using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    public string sceneName = "";
    public float timeLag = 0.5f;
    public Light ReceiverLight;
    public Light GiverLight;
    private bool isReceiverReady = false;
    private bool isGiverReady = false;


    public void Add(TriggerElectrons.Type type)
    {
        if (type == TriggerElectrons.Type.Giver)
        {
            isGiverReady = true;
            GiverLight.enabled = true;
        }
        else
        {
            isReceiverReady = true;
            ReceiverLight.enabled = true;
        }
        TryLoadLevel();
    }

    public void Remove(TriggerElectrons.Type type)
    {
        if (type == TriggerElectrons.Type.Giver)
        {
            isGiverReady = false;
            GiverLight.enabled = false;
        }
        else
        {
            isReceiverReady = false;
            ReceiverLight.enabled = false;
        }
    }

    void TryLoadLevel()
    {
        if (isGiverReady && isReceiverReady)
        {
            StartCoroutine(LoadLevelLag());
        }
    }

    IEnumerator LoadLevelLag()
    {
        yield return FadeToBlackController.Instance.FadeOut();
        //yield return new WaitForSeconds(timeLag);
        if (isGiverReady && isReceiverReady)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
