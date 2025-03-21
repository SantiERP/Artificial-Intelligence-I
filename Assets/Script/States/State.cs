using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State 
{
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();

    public FineStateMachineLeader FSMLeader;
    public FineStateMachineMinion FSMMinion;
}
