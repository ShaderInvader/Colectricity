using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferStationScript : DistanceTriggeredOperation
{

    [SerializeField] public List<PlaneState> startingStates;
    [SerializeField] public List<PlaneState> finalStates;
    public float transferSpeed; // from 0.5 to 1, higher = quicker

    //private TransferStationScript secondTransferStation;
    private Cable cable;
    private Transform standingPart;
    public TransferStationScript secondTransferStation;
    public float standingPartMaxOpacity;

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

    private void Start()
    {
        transferToAllStates(startingStates, 1);

        TransferStationScript[] transferStations = FindObjectsOfType<TransferStationScript>();
        Cable[] cables = FindObjectsOfType<Cable>();

        if(secondTransferStation == null)
        {
            Debug.LogError("TransferStation nie ma swojego kolegi! podlacz blizniaczy TransferStation zeby nikomu nie bylo smutno!");
        }

        standingPart = transform.Find("transfer_station_part06_low");

        if(standingPart == null)
        {
            Debug.LogError("Nie ma podstawki w transferStation! jest smutno :(");
        }

        cable = cables[0];
        foreach (Cable c in cables)
        {
            if (distanceTo(c) < distanceTo(cable))
            {
                cable = c;
            }
        }

        if(cable == null)
        {
            Debug.LogError("TransferStationScript nie mogl znalezc najblizszego Cable! jest smutno :(");
        }
    }

    private void Update()
    {
        Electron[] players = FindObjectsOfType<Electron>();

        /*
        1. gracz 1 stoi przy transfer station
        2. gracz 1 ma energie 
        2. gracz 2 stoi przy drugim transfer station
        3. gracz 2 ma miejsce na energie 

        wtedy:
        zapal najbli¿sz¹ graczowi 1 transferstation
        */

        // player 1
        Electron e0 = players[0];
        Electron e1 = players[1];


        if (doStuff(e0, e1) || doStuff(e1, e0))
        {
            triggerActionWhenInDistance();
            if (secondTransferStation)
            {
                secondTransferStation.triggerActionWhenInDistance();
            }
        }
        else
        {
            triggerActionWhenOutOfDistance();
            if (secondTransferStation)
            {
                secondTransferStation.triggerActionWhenOutOfDistance();
            }
        }

        if(cable.IsCableReady()) 
        {
            Material m = standingPart.GetComponent<Renderer>().material;
            m.EnableKeyword("_EmissiveExposureWeight");
            m.SetFloat("_EmissiveExposureWeight", standingPartMaxOpacity);
        }
    }

    private bool doStuff(Electron electron1, Electron electron2)
    {
        if (/*checkForDistance(electron1.transform)*/ cable.IsGoodToTransfer() && electron1.GetComponent<Energabler>().energy > 0
            /*&& secondTransferStation.checkForDistance(electron2.transform)*/ && electron1.GetComponent<Energabler>().energy != electron1.GetComponent<Energabler>().max_energy)
        {
            return true;
        } 
        else
        {
            return false;
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
        //Debug.Log("gracz jest w dystansie");
        transferToAllStates(finalStates, transferSpeed);
    }

    protected override void triggerActionWhenOutOfDistance()
    {
        //Debug.Log("gracz jest poza dystansem");
        transferToAllStates(startingStates, transferSpeed);
    }

    private double distanceTo(MonoBehaviour o)
    {
        return Vector3.Distance(this.transform.position, o.gameObject.transform.position);
    }

}
