using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public List<Transform> markers = new List<Transform>();
    public Material defaultMarkerMaterial;
    public Material energizedMarkerMaterial;

    private Energabler energabler;

    void Start()
    {
        this.energabler = GetComponent<Energabler>();
    }

    void Update()
    {
        int lastEnergizedMarker = (energabler.energy_units * markers.Count) / energabler.max_energy_units;
        for (int i=0; i<lastEnergizedMarker; i++)
        {
            markers[i].GetComponent<MeshRenderer>().material = energizedMarkerMaterial;
        }
        for (int i = lastEnergizedMarker; i < markers.Count; i++)
        {
            markers[i].GetComponent<MeshRenderer>().material = defaultMarkerMaterial;
        }
    }
}
