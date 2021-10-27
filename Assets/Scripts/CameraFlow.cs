using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlow : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public Camera mainCamera; // main Camera

    public float minDistanceBetweenPlayers;
    public float speed;

    public Camera camera1;
    public Camera camera2;

    void Start()
    {
    }

    void FixedUpdate()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, Vector3.Lerp(camera1.transform.position, camera2.transform.position, 0.5f), 0.1f);
        Vector3 rightCamera = new Vector3(
                mainCamera.transform.position.x + 5.7f,
                mainCamera.transform.position.y,
                mainCamera.transform.position.z + 5.7f);
        Vector3 leftCamera = new Vector3(
                mainCamera.transform.position.x - 5.7f,
                mainCamera.transform.position.y,
                mainCamera.transform.position.z - 5.7f);

        if (mainCamera.enabled && Vector3.Distance(player1.position, player2.position) > minDistanceBetweenPlayers) // !(isVisibleFrom(mainCamera, player2.position) && isVisibleFrom(mainCamera, player1.position)))
        {
            camera1.transform.position = rightCamera;
            camera2.transform.position = leftCamera;
            camera1.enabled = true;
            camera2.enabled = true;
            mainCamera.enabled = false;
        }
        else if (!mainCamera.enabled && Vector3.Distance(player1.position, player2.position) < minDistanceBetweenPlayers) //(isVisibleFrom(mainCamera, player2.position) && isVisibleFrom(mainCamera, player1.position)))
        {
            camera1.transform.position = Vector3.MoveTowards(camera1.transform.position, rightCamera, speed); //Vector3.Lerp(camera1.transform.position, rightCamera, 0.3f);
            camera2.transform.position = Vector3.MoveTowards(camera2.transform.position, leftCamera, speed); //Vector3.Lerp(camera2.transform.position, leftCamera, 0.3f);
            camera1.GetComponent<CameraController>().enabled = false;
            camera2.GetComponent<CameraController>().enabled = false;
            if (Vector3.Distance(camera1.transform.position, rightCamera) == 0f
                && Vector3.Distance(camera2.transform.position, leftCamera) == 0f)
            {
                camera1.GetComponent<CameraController>().enabled = true;
                camera2.GetComponent<CameraController>().enabled = true;
                camera1.enabled = false;
                camera2.enabled = false;
                mainCamera.enabled = true;
            }
        }
    }

    private bool isVisibleFrom(Camera camera, Vector3 realPosition)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(realPosition);
        return screenPoint.z > 0.25 && screenPoint.x > 0.25 && screenPoint.x < 0.75 && screenPoint.y > 0.25 && screenPoint.y < 0.75;
    }

    private float constraint(float a, float min, float max)
    {
        if (a < min) return min;
        if (a > max) return max;
        return a;
    }
}
