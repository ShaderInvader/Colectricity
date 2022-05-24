using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    public List<MeshRenderer> enemyEffect;

    public void UpdateEnemyEffect(float mul)
    {
        foreach (MeshRenderer mr in enemyEffect)
        {
            Vector4 v = mr.material.GetVector("ScrollSpeedCubes");
            v *= mul;
            mr.material.SetVector("ScrollSpeedCubes", v);
        }
    }
}
