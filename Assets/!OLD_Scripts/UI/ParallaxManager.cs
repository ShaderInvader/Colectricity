using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    public List<string> layerNames = new List<string>() {"Right", "Main", "Left"};

    void Start()
    {
        List<int> layerIndexes = new List<int>();
        foreach (string ln in layerNames)
        {
            layerIndexes.Add(LayerMask.NameToLayer(ln));
        }

        Camera[] cams = FindObjectsOfType<Camera>();
        foreach (Camera cam in cams)
        {
            if (layerIndexes.Contains(cam.gameObject.layer))
            {
                foreach (int idx in layerIndexes)
                {
                    Hide(cam, idx);
                }
                Show(cam, cam.gameObject.layer);
                GameObject background = Instantiate(transform.GetChild(0).gameObject, transform.position, transform.rotation, cam.transform);
                SetLayerRecurrent(background, cam.gameObject.layer);
            }
        }
        Destroy(gameObject);
    }

    void SetLayerRecurrent(GameObject go, int layer)
    {
        go.layer = layer;
        for (int i = 0; i < go.transform.childCount; i++)
        {
            SetLayerRecurrent(go.transform.GetChild(i).gameObject, layer);
        }
    }
    private void Show(Camera cam, int layer)
    {
        cam.cullingMask |= 1 << layer;
    }
    
    private void Hide(Camera cam, int layer)
    {
        cam.cullingMask &= ~(1 << layer);
    }
}
