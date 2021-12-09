using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DeathMinigame : MonoBehaviour
{
    public GameObject collectable;
    public int howMany;

    private List<GameObject> collectables = new List<GameObject>();

    void OnEnable()
    {
        for (int i = 0; i < howMany; i++)
        {
            GameObject go = Instantiate(collectable, transform.position + RandomOffset(), Quaternion.identity);
            go.GetComponent<MeshRenderer>().material = GetComponent<Electron>().liveMaterial;

            collectables.Add(go);
        }
    }

    Vector3 RandomOffset()
    {
        Vector2 randPosition = Random.insideUnitCircle * 2;
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
        Destroy(coll);
        collectables.Remove(coll);
        if (collectables.Count == 0)
        {
            gameObject.GetComponent<Electron>().Reborn();
            enabled = false;
        }
    }
}
