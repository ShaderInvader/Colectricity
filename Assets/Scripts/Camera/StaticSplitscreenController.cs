using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSplitscreenController : MonoBehaviour
{
    [Header("Cameras")]
    public Camera primaryCamera;
    public Transform primaryCameraPivot;
    public Camera secondaryCamera;
    public Transform secondaryCameraPivot;

    [Header("Setup")]
    public float splitscreenTriggerDistance = 15.0f;
    public float playerSnapLerpDistance = 5.0f;

    // Temp Zone: This will be changed after civilizing the code structure so beware
    public Transform player1;
    public Transform player2;

    public bool testPositions = false;
    private bool splitscreenEnabled = false;

    private void Start()
    {
        // Setup secondary camera for splitscreen
        Rect secondaryCamRect = secondaryCamera.rect;
        secondaryCamRect.width = 0.5f;
        secondaryCamRect.x = 0.5f;
        secondaryCamera.rect = secondaryCamRect;

        ToggleSplitscreen(false);
    }

    private void Update()
    {
        // Calculate the distance between the players
        float distance = Vector3.Magnitude(player2.position - player1.position);
        Vector3 midpoint = (player1.position + player2.position) * 0.5f;

        // Main check if the distance is enough to enable splitscreen
        if (distance > splitscreenTriggerDistance && !splitscreenEnabled)
        {
            splitscreenEnabled = true;
            ToggleSplitscreen(true);
        }
        else if (distance < splitscreenTriggerDistance && splitscreenEnabled)
        {
            splitscreenEnabled = false;
            ToggleSplitscreen(false);
        }

        if (splitscreenEnabled)
        {
            primaryCameraPivot.position = player1.position;
            secondaryCameraPivot.position = player2.position;
        }
        else
        {
            primaryCameraPivot.position = midpoint;
        }
    }

    private void ToggleSplitscreen(bool enabled)
    {
        // Set second camera active/inactive
        secondaryCamera.enabled = enabled;

        // Set primary camera rect for split rendering
        Rect primaryCamRect = primaryCamera.rect;
        primaryCamRect.width = enabled ? 0.5f : 1.0f;
        primaryCamera.rect = primaryCamRect;
    }
}
