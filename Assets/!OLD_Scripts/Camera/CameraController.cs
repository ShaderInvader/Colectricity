using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float minDistanceBetweenPlayers2To1;
    public float minDistanceBetweenPlayers1To2;
    public float twoToOneLinearSpeed = 0.6f;
    public float oneToTwoAcceleratingSpeed = 0.1f;
    public int cameraChangeCooldownSeconds = 1;

    private Transform player1;
    private Transform player2;

    private Camera leftCamera;
    private Camera rightCamera;
    private Camera mainCamera;

    private float defaultPlayerCameraFollowSpeed;
    private Vector3 rightOffset;
    private Vector3 leftOffset;
    private float toNextChange = 0;
   
    [SerializeField]
    private bool isChangingState = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Debug.LogError("There are two instances of CameraController in the scene!");
    }

    void OnEnable()
    {
        player1 = GameObject.FindObjectsOfType<Electron>()[0].transform;
        player2 = GameObject.FindObjectsOfType<Electron>()[1].transform;

        if(player1.name == "Oddawacz")
        {
            /*
             Quick fix so that placing Oddawacz and Pobieracz in the correct order doesn't matter
            TODO: Swap LeftCamera and RightCamera naming, current is not correct
             */
            Transform pom = player1;
            player1 = player2;
            player2 = pom;
        }

        leftCamera = player1.GetComponentInChildren<Camera>();
        rightCamera = player2.GetComponentInChildren<Camera>();
        mainCamera = GetComponentInChildren<Camera>();
        leftCamera.enabled = false;
        rightCamera.enabled = false;
    }

    void Start()
    {
        assignCameras();
        defaultPlayerCameraFollowSpeed = leftCamera.GetComponent<CameraFollow>().followSpeed;
    }

    void FixedUpdate()
    {
        if(leftCamera.GetComponent<CameraFollow>().parent == rightCamera.GetComponent<CameraFollow>().parent) // why the f this situation happens?
        {
            assignCameras();
        }
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, Vector3.Lerp(leftCamera.transform.position, rightCamera.transform.position, 0.5f), 0.1f);
        calculateOffset();

        if (toNextChange == 0 && mainCamera.enabled && Vector3.Distance(player1.position, player2.position) > minDistanceBetweenPlayers1To2)
        {
            assignCameras();
            changeToTwoCameras();
        }
        else if ((toNextChange == 0 && !mainCamera.enabled && Vector3.Distance(player1.position, player2.position) < minDistanceBetweenPlayers2To1) || isChangingState)
        {
            changeToOneCamera();
        }
        toNextChange = constraint(toNextChange - Time.deltaTime, 0, cameraChangeCooldownSeconds);
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
        leftCamera.transform.position = Vector3.MoveTowards(leftCamera.transform.position, leftOffset, twoToOneLinearSpeed);
        rightCamera.transform.position = Vector3.MoveTowards(rightCamera.transform.position, rightOffset, twoToOneLinearSpeed);
        leftCamera.GetComponent<CameraFollow>().enabled = false;
        rightCamera.GetComponent<CameraFollow>().enabled = false;
        isChangingState = true;
        if (Vector3.Distance(leftCamera.transform.position, leftOffset) == 0f
            && Vector3.Distance(rightCamera.transform.position, rightOffset) == 0f)
        {
            leftCamera.GetComponent<CameraFollow>().enabled = true;
            rightCamera.GetComponent<CameraFollow>().enabled = true;
            leftCamera.enabled = false;
            rightCamera.enabled = false;
            mainCamera.enabled = true;
            toNextChange = cameraChangeCooldownSeconds;
            isChangingState = false;
        }
    }

    private void changeToTwoCameras()
    {
        leftCamera.transform.position = leftOffset;
        rightCamera.transform.position = rightOffset;
        leftCamera.GetComponent<CameraFollow>().followSpeed = oneToTwoAcceleratingSpeed;
        rightCamera.GetComponent<CameraFollow>().followSpeed = oneToTwoAcceleratingSpeed;
        leftCamera.enabled = true;
        rightCamera.enabled = true;
        mainCamera.enabled = false;
        toNextChange = cameraChangeCooldownSeconds;
        isChangingState = false;
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

    public static CameraController getInstance()
    {
        return instance;
    }

    public Camera getCameraFor(Transform player)
    {
        if (leftCamera.GetComponent<CameraFollow>().parent == player) return leftCamera;
        else if (rightCamera.GetComponent<CameraFollow>().parent == player) return rightCamera;
        else throw new System.Exception($"Tried getting camera for {player.name} but got nothing! Most likely the CameraController is not yet fully initialized.");
    }

    public bool isVisibleFrom(Camera camera, Vector3 realPosition)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(realPosition);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

    public bool isOneCameraMode()
    {
        return mainCamera.enabled || isChangingState;
    }
}