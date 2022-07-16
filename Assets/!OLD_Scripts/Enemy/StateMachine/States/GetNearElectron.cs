using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;

public class GetNearElectron : State
{
    private SimpleAI ai;
    private NavMeshAgent agent;

    public GetNearElectron(GameObject go) : base(go) { }

    private ActionBase[] actions;

    public override void Enter()
    {
        base.Enter();
        go.GetComponentInChildren<EnemyEffect>().SpeedUpEffect();
        ai = go.GetComponent<SimpleAI>();
        agent = go.GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        actions = go.GetComponents<ActionBase>();
        foreach (ActionBase act in actions)
        {
            act.enabled = true;
        }
    }

    public override void Update()
    {
        base.Update();

        if (ai.target.isDead)
        {
            StartExitting(typeof(GoToStartPosition));
            return;
        }

        float dist = ai.GetDistance(ai.target.transform);
        if (ai.deativationDistance < dist)
        {
            StartExitting(typeof(GoToStartPosition));
            return;
        }

        if(!ai.IsVisible())
        {
            StartExitting(typeof(GoToStartPosition));
            return;
        }

        agent.SetDestination(ai.target.transform.position);
    }
    
    private void StartExitting(Type type)
    {
        ai.target = null;
        agent.isStopped = true;
        ai.ChooseStateOfType(type);
    }

    public override void Exit()
    {
        go.GetComponentInChildren<EnemyEffect>().SlowDownEffect();
        foreach (ActionBase act in actions)
        {
            act.enabled = false;
        }
        base.Exit();
    }
}
