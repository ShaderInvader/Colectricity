using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IndicatorController : MonoBehaviour
{
    private static List<Indicator> indicatorList;

    private void Start()
    {
        disableIndicators();
    }

    private void OnEnable()
    {
        indicatorList = FindObjectsOfType<Indicator>().ToList();
        Debug.Log("Found " + indicatorList.Count + " indicators.");
    }

    void FixedUpdate()
    {
        if (CameraController.getInstance().isOneCameraMode())
        {
            disableIndicators();
        }
        else
        {
            enableIndicators();
        }
    }

    public static List<Indicator> getIndicators()
    {
        return indicatorList;
    }

    public static void enableIndicators()
    {
        setIndicatorsEnable(true);
    }

    public static void disableIndicators()
    {
        setIndicatorsEnable(false);
    }

    private static void setIndicatorsEnable(bool value)
    {
        indicatorList.ForEach(indicator => {
            indicator.image.gameObject.SetActive(value); 
        });
    }

    public static void setIndicatorEnable(Indicator indicator, bool value)
    {
        indicator.image.gameObject.SetActive(value);
    }
}
