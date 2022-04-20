using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public string imageObjectName;

    [HideInInspector]
    public Transform image;
   
    [Tooltip("0 meaning center, 0.5 meaning edge of camera")]
    public float indicatorDistanceFromScreen = 0.4f;

    private void Start()
    {
        image = GameObject.Find(imageObjectName).transform;
    }

    void Update()
    {
        foreach (Indicator indicator in IndicatorController.getIndicators())
        {
            if (indicator.transform != transform)
            {
                if (CameraController.getInstance().isVisibleFrom(CameraController.getInstance().getCameraFor(this.transform), indicator.transform.position))
                {
                    IndicatorController.setIndicatorEnable(this, false);
                    continue;
                }
                Camera thisCamera = CameraController.getInstance().getCameraFor(this.transform);
                Vector3 viewportDifference = (thisCamera.WorldToViewportPoint(transform.position) -
                    thisCamera.WorldToViewportPoint(indicator.transform.position));
                if (Mathf.Abs(viewportDifference.x) >= indicatorDistanceFromScreen || Mathf.Abs(viewportDifference.y) >= indicatorDistanceFromScreen)
                {
                    Vector3 end = thisCamera.WorldToViewportPoint(indicator.transform.position) -
                    thisCamera.WorldToViewportPoint(this.transform.position);

                    float angle = 0; 
                       
                    if (Mathf.Abs(end.x) > Mathf.Abs(end.y))
                    {
                        end = new Vector3(Mathf.Sign(end.x) * indicatorDistanceFromScreen, Mathf.Sign(end.x) * (indicatorDistanceFromScreen * end.y) / end.x, 0);
                        angle = (Mathf.Rad2Deg * Mathf.Asin(end.y / end.magnitude) * Mathf.Sign(getAdjustedDifference(end))) + getAdjustedDifference(end);
                    }
                    else
                    {
                        end = new Vector3(Mathf.Sign(end.y) * (indicatorDistanceFromScreen * end.x) / end.y, Mathf.Sign(end.y) * indicatorDistanceFromScreen, 0);
                        angle = (Mathf.Rad2Deg * Mathf.Asin(end.x / end.magnitude) * Mathf.Sign(getAdjustedDifference(end))) + getAdjustedDifference(end);
                    }
                    
                    end = thisCamera.WorldToViewportPoint(this.transform.position) + end;
                    end = thisCamera.ViewportToScreenPoint(end);
                    end.z = 0;
                    image.transform.position = end;
                    image.localEulerAngles = new Vector3(0, 0, angle);
                }
            }
        }
    }

    private float getAdjustedDifference(Vector3 end)
    {
        if (Mathf.Abs(end.x) > Mathf.Abs(end.y))
        {
            if (end.x > 0) // arrow is on right
            {
                return 180;
            }
            else // arrow is on left
            {
                return -0.000000000001f;
            }
        }
        else // arrow is on left
        {
            if (end.y > 0) // arrow is top
            {
                return -90;
            }
            else // arrow is down
            {
                return 90;
            }
        }
    }

    private float constraint(float a, float min, float max)
    {
        if (a < min) return min;
        if (a > max) return max;
        return a;
    }
}
