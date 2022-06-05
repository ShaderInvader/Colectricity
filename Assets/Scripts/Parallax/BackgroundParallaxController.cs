using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallaxController : MonoBehaviour
{
    public string primaryCameraLayer;
    public string secondaryCameraLayer;

    private DynamicSplitscreenController _splitscreenController;

    private ParallaxLevel[] _primaryParallaxLevels;
    private ParallaxLevel[] _secondaryParallaxLevels;

    private void Awake()
    {
        // Yep, this should be injected by DI in the future
        _splitscreenController = GetComponent<DynamicSplitscreenController>();

        // Not a perfect solution (could be betted with DI), but an easy way to let 3d/designers work on background freely ~fmazurek
        GameObject _backgroundPrimary = GameObject.FindGameObjectWithTag("Background");
        // Instatiating the second instance as a clone of found background object
        GameObject _backgroundSecondary = Instantiate(_backgroundPrimary, _backgroundPrimary.transform.parent);

        // Change the names for better scene management
        _backgroundPrimary.name = "Background Primary Camera";
        _backgroundSecondary.name = "Background Secondary Camera";

        // Setting up background layers
        SetLayerRecursively(_backgroundPrimary, LayerMask.NameToLayer(primaryCameraLayer));
        SetLayerRecursively(_backgroundSecondary, LayerMask.NameToLayer(secondaryCameraLayer));

        _primaryParallaxLevels = _backgroundPrimary.GetComponentsInChildren<ParallaxLevel>();
        _secondaryParallaxLevels = _backgroundSecondary.GetComponentsInChildren<ParallaxLevel>();

        // Injecting camera references into parallax layers
        SetReferencePoints(_primaryParallaxLevels, _splitscreenController.primaryCameraPivot);
        SetReferencePoints(_secondaryParallaxLevels, _splitscreenController.secondaryCameraPivot);
    }

    private void SetReferencePoints(ParallaxLevel[] levelArray, Transform referencePoint)
    {
        foreach (var level in levelArray)
        {
            level.referencePoint = referencePoint;
        }
    }

    public static void SetLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;

        for (int i = 0; i < go.transform.childCount; i++)
        {
            SetLayerRecursively(go.transform.GetChild(i).gameObject, layer);
        }
    }
}
