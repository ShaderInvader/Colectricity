using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    public List<MeshRenderer> enemyEffect;
    public float changingSpeed;
    public float speedUpEffectBy = 2;
    List<Vector4> startValues = new List<Vector4>();
    List<Vector4> endValues = new List<Vector4>();
    float t = 0;

    public void SpeedUpEffect()
    {
        UpdateEnemyEffect(speedUpEffectBy);
    }

    public void SlowDownEffect()
    {
        float slowDownBy = 1 / speedUpEffectBy;
        UpdateEnemyEffect(slowDownBy);
    }

    void UpdateEnemyEffect(float mul)
    {
        startValues.Clear();
        endValues.Clear();
        t = 0;
        foreach (MeshRenderer mr in enemyEffect)
        {
            Vector4 startVal = mr.material.GetVector("ScrollSpeedCubes");
            Vector4 endVal = startVal * mul;
            startValues.Add(startVal);
            endValues.Add(endVal);
        }
    }

    void Update()
    {
        if (startValues.Count == 0)
        {
            return;
        }

        t += Time.deltaTime * changingSpeed;

        if (t >= 1)
        {
            SetPropValue(1);
            startValues.Clear();
            endValues.Clear();
            return;
        }
        SetPropValue(t);
    }

    void SetPropValue(float t)
    {
        for (int i = 0; i < startValues.Count; i++)
        {
            Vector4 val = Vector4.Lerp(startValues[i], endValues[i], t);
            enemyEffect[i].material.SetVector("ScrollSpeedCubes", val);
        }
    }
}
