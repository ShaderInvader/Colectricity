using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    private StateMachine sm;
    public List<State> states = new List<State>();
    int stateNumber;
    public float activationDistance = 1f;
    public float deativationDistance = 1.5f;
    public Electron target;
    void Start()
    {
        sm = new StateMachine();
        states.Add(new WaitForElectronInRange(gameObject));
        states.Add(new GetNearElectron(gameObject));
        sm.CurrentState = states[0];
    }

    private void Update()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        sm.CurrentState.Update();
    }

    private void FixedUpdate()
    {
        sm.CurrentState.FixedUpdate();
    }

    public void NextState()
    {
        stateNumber = (stateNumber+1)%states.Count;
        sm.CurrentState = states[stateNumber];
    }
    public float GetDistance(Transform el)
    {
        return Vector3.Distance(el.position, transform.position);
    }
}
