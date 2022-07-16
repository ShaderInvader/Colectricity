using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected GameObject go;
    
    public State(GameObject go)
    {
        this.go = go;
    }
    
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}
