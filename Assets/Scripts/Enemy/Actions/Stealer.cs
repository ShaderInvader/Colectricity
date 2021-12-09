using UnityEngine;

public class Stealer : MonoBehaviour
{
    public float stealingRadius;
    public int stealEnergyUnits;
    public float timeBetweenStealing;
    
    GameObject[] all;
    int stealingAmmount;

    private float sinceLast;

    private void Start()
    {
        stealingAmmount = GlobalVars.energy_amount_unit;
        all = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        if(sinceLast <= 0)
        {
            sinceLast = timeBetweenStealing;
            foreach (GameObject g in all) // very slow method, needed refactoring and optimalization
            {
                if (Vector3.Distance(g.transform.position, transform.position) < stealingRadius)
                {
                    for(int i=0; i<stealEnergyUnits; i++)
                    {
                        bool res = g.GetComponent<Energabler>().RemEnergy(stealingAmmount);
                        if (res)
                        {
                            GameObject.FindGameObjectsWithTag("Container")[0].GetComponent<Energabler>().AddEnergy(stealingAmmount);
                        }
                    }
                    break;
                }
            }
        }
        sinceLast -= Time.deltaTime;
    }
}
