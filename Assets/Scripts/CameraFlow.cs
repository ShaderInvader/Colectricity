using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlow : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public Camera mainCamera; // main Camera

    public Camera camera1;
    public Camera camera2;

    void Start()
    {
        //camera1 = player1.GetComponentInChildren<Camera>();
        //camera2 = player2.GetComponentInChildren<Camera>();
    }

    void FixedUpdate()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, Vector3.Lerp(camera1.transform.position, camera2.transform.position, 0.5f), 0.1f);
        if (isVisibleFrom(mainCamera, player2.position) && isVisibleFrom(mainCamera, player1.position))
        {
            camera1.enabled = false;
            camera2.enabled = false;
            mainCamera.enabled = true;
        }
        else
        {
            camera1.enabled = true;
            camera2.enabled = true;
            mainCamera.enabled = false;
        }
    }

    private bool isVisibleFrom(Camera camera, Vector3 realPosition)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(realPosition);
        return screenPoint.z > 0.25 && screenPoint.x > 0.25 && screenPoint.x < 0.75 && screenPoint.y > 0.25 && screenPoint.y < 0.75;
    }
}
