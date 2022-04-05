using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferStationScript : DistanceTriggeredOperation
{

    [SerializeField] public List<PlaneState> startingStates;
    [SerializeField] public List<PlaneState> finalStates;
    public float transferSpeed; // from 0.5 to 1, higher = quicker

    [System.Serializable]
    public struct PlaneState {

        public TransferPlane refPlane;
        public Vector3 position;
        public float rotationSpeed;
        public float opacity;
    }

    private void transferStateTo(float transferSpeed, PlaneState planeState)
    {
        planeState.refPlane.transform.localPosition = Vector3.Lerp(planeState.refPlane.transform.localPosition, planeState.position, transferSpeed);

        Material m = planeState.refPlane.GetComponent<Renderer>().material;
        m.EnableKeyword("_EmissiveExposureWeight");
        float newEmissive = Mathf.Lerp(m.GetFloat("_EmissiveExposureWeight"), -1 * planeState.opacity, transferSpeed);
        m.SetFloat("_EmissiveExposureWeight", newEmissive);

        planeState.refPlane.rotationSpeed = Mathf.Lerp(planeState.refPlane.rotationSpeed, planeState.rotationSpeed, transferSpeed);
    }

    private void Update()
    {
        foreach (Electron player in FindObjectsOfType<Electron>())
        {
            if (player.GetComponent<Energabler>().energy > 0)
            {
                checkForDistance(player.transform);
            }
        }
    }

    private void transferToAllStates(List<PlaneState> states, float transferSpeed)
    {
        foreach (PlaneState planeState in states)
        {
            transferStateTo(transferSpeed, planeState);
        }
    }

    protected override void triggerActionWhenInDistance()
    {
        Debug.Log("gracz jest w dystansie");
        transferToAllStates(finalStates, transferSpeed);
    }

    protected override void triggerActionWhenOutOfDistance()
    {
        Debug.Log("gracz jest poza dystansem");
        transferToAllStates(startingStates, transferSpeed);
    }

}
