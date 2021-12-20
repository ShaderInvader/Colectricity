using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForElectronInRange : State
{
    private Electron[] electrons;
    private SimpleAI ai;

    public WaitForElectronInRange(GameObject go) : base(go) { }

    public override void Enter()
    {
        base.Enter();
        electrons = Object.FindObjectsOfType<Electron>();
        ai = go.GetComponent<SimpleAI>();
    }

    public override void Update()
    {
        base.Update();
        float dist;
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
                ai.NextState();
                return;
            }
        }
    }
}
