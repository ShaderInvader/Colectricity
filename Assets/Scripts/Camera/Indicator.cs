using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public Transform image;
    [Tooltip("0 meaning center, 0.5 meaning edge of camera")]
    public float indicatorDistanceFromScreen = 0.4f;

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

                    if (Mathf.Abs(end.x) > Mathf.Abs(end.y))
                    {
                        end = new Vector3(Mathf.Sign(end.x) * indicatorDistanceFromScreen, Mathf.Sign(end.x) * (indicatorDistanceFromScreen * end.y) / end.x, 0);
                    }
                    else
                    {
                        end = new Vector3(Mathf.Sign(end.y) * (indicatorDistanceFromScreen * end.x) / end.y, Mathf.Sign(end.y) * indicatorDistanceFromScreen, 0);
                    }
                    end = thisCamera.WorldToViewportPoint(this.transform.position) + end;
                    end = thisCamera.ViewportToScreenPoint(end);
                    end.z = 0;
                    image.transform.position = end;
                }
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
