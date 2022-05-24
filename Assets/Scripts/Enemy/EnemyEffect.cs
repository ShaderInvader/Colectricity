using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    public List<MeshRenderer> enemyEffect;
    public float changingDuration;
    public float speedUpBy = 2;
    public int steps;

    public void SpeedUpEffect()
    {
        UpdateEnemyEffect(speedUpBy);
    }

    public void SlowDownEffect()
    {
        float slowDownBy = 1 / speedUpBy;
        UpdateEnemyEffect(slowDownBy);
    }

    void UpdateEnemyEffect(float mul)
    {
        List<Vector4> startValues = new List<Vector4>();
        List<Vector4> endValues = new List<Vector4>();
        foreach (MeshRenderer mr in enemyEffect)
        {
            Vector4 startVal = mr.material.GetVector("ScrollSpeedCubes");
            Vector4 endVal = startVal * mul;
            startValues.Add(startVal);
            endValues.Add(endVal);
        }
        StartCoroutine(ChangeMaterialProps(startValues, endValues));
    }

    IEnumerator ChangeMaterialProps(List<Vector4> startValues, List<Vector4> endValues)
    {
        Debug.Log("Start");
        int countList = startValues.Count;
        float stepDuration = changingDuration / steps;

        for (float step = 0; step <= steps; step++)
        {
            float lerpVal = step / steps;
            for (int i = 0; i < countList; i++)
            {
                Vector4 val = Vector4.Lerp(startValues[i], endValues[i], lerpVal);
                enemyEffect[i].material.SetVector("ScrollSpeedCubes", val);
            }

            yield return new WaitForSeconds(stepDuration);
        }
        Debug.Log("End");
    }
}
