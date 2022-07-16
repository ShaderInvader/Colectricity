using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    private int blockadeMask;
    [HideInInspector]
    public StateMachine sm;
    public List<State> states = new List<State>();
    int stateNumber;
    public float activationDistance = 1f;
    public float deativationDistance = 1.5f;
    public float startZone = 0.2f;
    public Electron target;
    public Animator modelAnimator;

    void Start()
    {
        modelAnimator = GetComponentInChildren<Animator>();
        sm = new StateMachine();
        states.Add(new WaitForElectronInRange(gameObject));
        states.Add(new GetNearElectron(gameObject));
        states.Add(new GoToStartPosition(gameObject));
        sm.CurrentState = states[0];
        blockadeMask = 1 << LayerMask.NameToLayer("Blockade");
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

    public void ChooseStateOfType(Type type)
    {
        for(int i=0; i<states.Count; i++)
        {
            if (states[i].GetType() == type)
            {
                stateNumber = i;
                break;
            }
        }
        sm.CurrentState = states[stateNumber];
    }

    public float GetDistance(Transform el)
    {
        return Vector3.Distance(el.position, transform.position);
    }

    public float GetDistance(Vector3 el)
    {
        return Vector3.Distance(el, transform.position);
    }

    public bool IsVisible(GameObject potentialTarget=null)
    {
        if(potentialTarget == null)
        {
            if (target == null)
            {
                return false;
            }
            potentialTarget = target.gameObject;
        }

        RaycastHit hit;
        Vector3 enemyPosition = transform.position;
        Vector3 targetPosition = potentialTarget.transform.position;
        float distance = Vector3.Distance(enemyPosition, targetPosition);
        bool isColliding = Physics.Raycast(enemyPosition, targetPosition - enemyPosition, out hit, distance, blockadeMask);
        return !isColliding;
    }
}
