using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class GoToStartPosition : State
{
    private Electron[] electrons;
    private SimpleAI ai;
    private NavMeshAgent agent;
    private Vector3 destination;

    public GoToStartPosition(GameObject go) : base(go) { }
    private ActionBase[] actions;

    public override void Enter()
    {
        base.Enter();
        go.GetComponentInChildren<EnemyEffect>().SpeedUpEffect();
        ai = go.GetComponent<SimpleAI>();
        agent = go.GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        destination = go.GetComponent<GetStartPoint>().StartPosition;
        electrons = Object.FindObjectsOfType<Electron>();
    }

    public override void Update()
    {
        base.Update();

        float dist = ai.GetDistance(destination);
        if (ai.startZone > dist)
        {
            ai.target = null;
            agent.isStopped = true;
            ai.NextState();
            return;
        }

        foreach (Electron e in electrons)
        {
            if (e.isDead)
            {
                continue;
            }
            dist = ai.GetDistance(e.transform);
            if (ai.activationDistance > dist)
            {
                ai.target = e;
                ai.ChooseStateOfType(typeof(GetNearElectron));
                return;
            }
        }

        agent.SetDestination(destination);
    }

    public override void Exit()
    {
        go.GetComponentInChildren<EnemyEffect>().SlowDownEffect();
        base.Exit();
    }
}
