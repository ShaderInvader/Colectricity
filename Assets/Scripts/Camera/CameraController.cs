using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float minDistanceBetweenPlayers;
    public float oneToTwoLinearSpeed;

    public Transform player1;
    public Transform player2;

    private Camera leftCamera;
    private Camera rightCamera;
    private Camera mainCamera;

    private Vector3 rightOffset;
    private Vector3 leftOffset;

    void OnEnable()
    {
        leftCamera = player1.GetComponentInChildren<Camera>();
        rightCamera = player2.GetComponentInChildren<Camera>();
        mainCamera = GetComponentInChildren<Camera>();
    }

    void Start()
    {
        assignCameras();
    }

    void FixedUpdate()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, Vector3.Lerp(leftCamera.transform.position, rightCamera.transform.position, 0.5f), 0.1f);
        calculateOffset();

        if (mainCamera.enabled && Vector3.Distance(player1.position, player2.position) > minDistanceBetweenPlayers)
        {
            assignCameras();
            changeToTwoCameras();
        }
        else if (!mainCamera.enabled && Vector3.Distance(player1.position, player2.position) < minDistanceBetweenPlayers)
        {
            changeToOneCamera();
        }
    }

    private void calculateOffset()
    {
        float value = (mainCamera.orthographicSize * mainCamera.aspect) / (2 * Mathf.Sqrt(2));
        rightOffset = new Vector3(
                mainCamera.transform.position.x + value,
                mainCamera.transform.position.y,
                mainCamera.transform.position.z + value);
        leftOffset = new Vector3(
                mainCamera.transform.position.x - value,
                mainCamera.transform.position.y,
                mainCamera.transform.position.z - value);
    }

    private void assignCameras()
    {
        if(mainCamera.enabled)
        {
            if (isOnRight(mainCamera, player1.position))
            {
                leftCamera.GetComponent<CameraFollow>().parent = player2;
                rightCamera.GetComponent<CameraFollow>().parent = player1;
            }
            else
            {
                leftCamera.GetComponent<CameraFollow>().parent = player1;
                rightCamera.GetComponent<CameraFollow>().parent = player2;
            }
        }
    }

    private void changeToOneCamera()
    {
        leftCamera.transform.position = Vector3.MoveTowards(leftCamera.transform.position, leftOffset, oneToTwoLinearSpeed);
        rightCamera.transform.position = Vector3.MoveTowards(rightCamera.transform.position, rightOffset, oneToTwoLinearSpeed);
        leftCamera.GetComponent<CameraFollow>().enabled = false;
        rightCamera.GetComponent<CameraFollow>().enabled = false;
        if (Vector3.Distance(leftCamera.transform.position, leftOffset) == 0f
            && Vector3.Distance(rightCamera.transform.position, rightOffset) == 0f)
        {
            leftCamera.GetComponent<CameraFollow>().enabled = true;
            rightCamera.GetComponent<CameraFollow>().enabled = true;
            leftCamera.enabled = false;
            rightCamera.enabled = false;
            mainCamera.enabled = true;
        }
    }

    private void changeToTwoCameras()
    {
        leftCamera.transform.position = leftOffset;
        rightCamera.transform.position = rightOffset;
        leftCamera.enabled = true;
        rightCamera.enabled = true;
        mainCamera.enabled = false;
    }

    private bool isOnRight(Camera camera, Vector3 realPosition)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(realPosition);
        return screenPoint.z > 0 && screenPoint.x > 0.5 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

    private float constraint(float a, float min, float max)
    {
        if (a < min) return min;
        if (a > max) return max;
        return a;
    }
}
