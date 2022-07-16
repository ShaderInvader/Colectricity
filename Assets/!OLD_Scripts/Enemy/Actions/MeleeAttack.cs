using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SimpleAI))]
public class MeleeAttack : ActionBase
{
    public float attackAnimationDelay;
    private float attackDelayT;
    private bool isAttackAnimationPlaying;

    private SimpleAI sai;
    
    private float sinceLast;

    void Start()
    {
        sai = GetComponent<SimpleAI>();
    }

    void OnEnable()
    {
        isAttackAnimationPlaying = false;
        sinceLast = timeBetween;
        attackDelayT = attackAnimationDelay;
    }

    void Update()
    {
        if(!isAttackAnimationPlaying)
        {
            sinceLast -= Time.deltaTime;
            sinceLast = sinceLast < 0 ? 0 : sinceLast;

            if (hasValidTarget() && sinceLast == 0)
            {
                tryToAttack();
            }
        } 
        else
        {
            attackDelayT -= Time.deltaTime;
            attackDelayT = attackDelayT < 0 ? 0 : attackDelayT;

            if (attackDelayT == 0)
            {
                if(hasValidTarget())
                {
                    Attack();
                }
                attackDelayT = attackAnimationDelay;
                isAttackAnimationPlaying = false;
            }
        }

    }

    void Attack()
    {
        
        sai.target.ReceiveDamage(units);
    }

    void tryToAttack()
    {
        sinceLast = timeBetween;
        isAttackAnimationPlaying = true;
        sai.modelAnimator.SetTrigger("attacking");
    }

    private bool hasValidTarget()
    {
        if (sai.target == null)
        {
            return false;
        }

        if (Vector3.Distance(sai.target.transform.position, transform.position) > radius)
        {
            return false;
        }

        return true;
    }
}
