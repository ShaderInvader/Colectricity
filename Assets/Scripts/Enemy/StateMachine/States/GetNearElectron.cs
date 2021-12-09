using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class GetNearElectron : State
{
    private SimpleAI ai;
    private NavMeshAgent agent;

    public GetNearElectron(GameObject go) : base(go) { }

    private ActionBase[] actions;

    public override void Enter()
    {
        base.Enter();
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

        if (ai.target.dead)
        {
            ai.target = null;
            agent.isStopped = true;
            ai.NextState();
            return;
        }

        float dist = ai.GetDistance(ai.target.transform);
        if (ai.deativationDistance < dist)
        {
            ai.target = null;
            agent.isStopped = true;
            ai.NextState();
            return;
        }

        agent.SetDestination(ai.target.transform.position);
    }

    public override void Exit()
    {
        foreach (ActionBase act in actions)
        {
            act.enabled = false;
        }
        base.Exit();
    }
}
