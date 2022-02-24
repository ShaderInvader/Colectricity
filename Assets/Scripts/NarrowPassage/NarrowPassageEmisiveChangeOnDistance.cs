using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrowPassageEmisiveChangeOnDistance : MonoBehaviour
{
    private Electron[] players;
    private Material material;
    private float intensity;
    private Color color;

    public float minDistance;
    public float minIntensity;
    public float maxIntensity;

    private void Start()
    {
        players = Resources.FindObjectsOfTypeAll<Electron>();
        material = GetComponent<Renderer>().material;
        color = material.GetColor("_Color");
    }

    void Update()
    {
        float nearestPlayerDistanceFromPassage = 2 * minDistance;

        foreach(Electron player in players)
        {
            float actDistance = Vector3.Distance(player.transform.position, this.transform.position);
            nearestPlayerDistanceFromPassage = actDistance < nearestPlayerDistanceFromPassage ? actDistance : nearestPlayerDistanceFromPassage; 
        }

        if (minDistance >= nearestPlayerDistanceFromPassage)
        {
            intensity = Mathf.Lerp(maxIntensity, minIntensity, nearestPlayerDistanceFromPassage / minDistance);
        } 
        else
        {
            intensity = minIntensity;
        }

        float factor = intensity;

        material.SetFloat("Last", factor);
        Debug.Log(factor);
        material.SetColor("_Color", new Color(color.r * factor, color.g * factor, color.b * factor, color.a * factor));
    }
}
