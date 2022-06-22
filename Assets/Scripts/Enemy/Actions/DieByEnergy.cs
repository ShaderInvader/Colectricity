using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DieByEnergy : MonoBehaviour
{
    public DissolveController dissolveController;
    public GameObject enemyEffect;
    public int energyUnitsToDie = 3;

    private SimpleAI sai;

    private int _current = 0;
    private int _previous = 0;
    private int _max;


    private void Start()
    {
        _max = energyUnitsToDie;
        sai = GetComponent<SimpleAI>();
    }

    void Update()
    {
        _current = GetComponent<Energabler>().energy_units;
        if (_previous>_current)
        {
            energyUnitsToDie -= (_previous - _current);
            sai.modelAnimator.SetTrigger("getting_hit");
            if (energyUnitsToDie==0)
            {
                if (enemyEffect)
                {
                    Destroy(enemyEffect);
                }
                StartCoroutine(DeathCoroutine());
            }
        }
        _previous = _current;
    }

    private IEnumerator DeathCoroutine()
    {
        yield return dissolveController.Dissolve();
        Destroy(gameObject);
    }

}
