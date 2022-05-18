using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class DeathMinigame : MonoBehaviour
{
    public GameObject collectable;
    public int howMany;
    public float howFarMax = 5.5f;
    public float howFarMin = 3.5f;
    public string cubeName = "cube1";
    public Material particle_mat;

    private Electron elec;
    private List<GameObject> collectables = new List<GameObject>();

    void OnEnable()
    {
        elec = GetComponent<Electron>();
        for (int i = 0; i < howMany; i++)
        {
            GameObject go = Instantiate(collectable, transform.position + RandomOffset(), Quaternion.identity);

            RaycastHit hit;
            float distance = Vector3.Distance(transform.position, go.transform.position);
            bool isColliding = Physics.Raycast(transform.position, go.transform.position - transform.position, out hit, distance);
            if (hit.transform.gameObject.GetComponent<Collectable>() == null)
            {
                Destroy(go);
                i--;
                continue;
            }

            go.GetComponent<MeshRenderer>().material = particle_mat;
            go.GetComponent<Light>().color = elec.liveBodyMaterial.color;
            go.name = cubeName;
            collectables.Add(go);
        }
    }

    Vector3 RandomOffset()
    {
        Vector2 randPosition = Random.insideUnitCircle * howFarMin;
        float randDist = howFarMax - howFarMin;
        randPosition.x = randPosition.x > 0 ? randPosition.x + randDist * Random.value : randPosition.x - randDist * Random.value;
        randPosition.y = randPosition.y > 0 ? randPosition.y + randDist * Random.value : randPosition.y - randDist * Random.value;
        return new Vector3(randPosition.x, 0, randPosition.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject coll = other.gameObject;
        if (coll.GetComponent<Collectable>() == null)
        {
            return;
        }
        if (coll.name != cubeName)
        {
            return;
        }
        Destroy(coll);
        collectables.Remove(coll);

        float progress = collectables.Count / (float) howMany;

        GetComponent<MeshRenderer>().material.color = Color.Lerp(elec.liveBodyMaterial.color, elec.deadBodyMaterial.color, progress);
        //GetComponent<MeshRenderer>().material.color = Color.Lerp(elec.liveHeadMaterial.color, elec.deadHeadMaterial.color, progress);
        //GetComponent<Light>().intensity = 

        if (collectables.Count == 0)
        {
            gameObject.GetComponent<Electron>().Reborn();
            enabled = false;
        }
    }
}
