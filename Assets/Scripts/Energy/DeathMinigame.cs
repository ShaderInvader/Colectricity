using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DeathMinigame : MonoBehaviour
{
    public GameObject collectable;
    public int howMany;
    public float howFar = 3;
    public string cubeName = "cube1";

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

            go.GetComponent<MeshRenderer>().material = elec.liveMaterial;
            go.GetComponent<Light>().color = elec.liveMaterial.color;
            go.name = cubeName;
            collectables.Add(go);
        }
    }

    Vector3 RandomOffset()
    {
        Vector2 randPosition = Random.insideUnitCircle * howFar;
        randPosition.x = randPosition.x > 0 ? randPosition.x + Random.value : randPosition.x - Random.value;
        randPosition.y = randPosition.y > 0 ? randPosition.y + Random.value : randPosition.y - Random.value;
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

        GetComponent<MeshRenderer>().material.color = Color.Lerp(elec.liveMaterial.color, elec.deadMaterial.color, collectables.Count / (float)howMany);

        if (collectables.Count == 0)
        {
            gameObject.GetComponent<Electron>().Reborn();
            enabled = false;
        }
    }
}
