using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DynamicSplitscreenController : MonoBehaviour
{
    [Header("Cameras")]
    public Camera primaryCamera;
    public Volume primaryVolume;
    public Transform primaryCameraPivot;
    public Camera secondaryCamera;
    public Volume secondaryVolume;
    public Transform secondaryCameraPivot;

    [Header("Setup")]
    public float splitscreenTriggerDistance = 15.0f;
    public float playerSnapLerpDistance = 5.0f;
    public float splitCameraOffset = 1.0f;
    public float splitRotationOffset = 45.0f;

    // Temp Zone: This will be changed after civilizing the code structure so beware
    public Transform player1;
    public Transform player2;

    private Vector3 _direction = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector2 _splitVector = new Vector2(0.0f, 0.0f);
    private ScreenSplitPostProcess _screenSplitPostProcess;

    private void Awake()
    {
        primaryVolume.profile.TryGet(out _screenSplitPostProcess);
    }

    private void Update()
    {
        float distance = Vector3.Distance(player1.position, player2.position);
        Vector3 midpoint = (player1.position + player2.position) * 0.5f;
        Vector3 startDirection = Vector3.Normalize(player1.position - player2.position);
        _direction = Quaternion.AngleAxis(splitRotationOffset, Vector3.up) * startDirection;
        _splitVector.x = _direction.x;
        _splitVector.y = _direction.z;

        if (distance >= splitscreenTriggerDistance)
        {
            // Render two cameras, start lerping cameras to players, calculate split axis
            secondaryCamera.enabled = true;

            primaryCameraPivot.position = Vector3.Lerp(midpoint, player1.position - startDirection * splitCameraOffset, ((distance - splitscreenTriggerDistance) / playerSnapLerpDistance));
            secondaryCameraPivot.position = Vector3.Lerp(midpoint, player2.position + startDirection * splitCameraOffset, ((distance - splitscreenTriggerDistance) / playerSnapLerpDistance));

            _screenSplitPostProcess.enabled.Override(true);
            _screenSplitPostProcess.splitAxis.Override(_splitVector);
        }
        else
        {
            // Render one camera, set position in the midpoint of two players
            secondaryCamera.enabled = false;

            primaryCameraPivot.position = midpoint;
            secondaryCameraPivot.position = midpoint; // Just to be sure no jerking occurs when switching to double camera

            _screenSplitPostProcess.enabled.Override(false);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 midpoint = (player1.position + player2.position) * 0.5f;
        Gizmos.DrawLine(midpoint, midpoint + _direction);
    }
}
