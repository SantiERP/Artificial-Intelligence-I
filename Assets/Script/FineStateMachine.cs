using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Minion;

public class FineStateMachine
{
    protected State _currentState;
    protected State CurrentState
    {
        get { return _currentState; }
    }
    public virtual void ChangeState(LeaderStates L)
    {

    }

    public virtual void ChangeState(MinionState M)
    {

    }

    public void OnUpddate()
    {
        _currentState?.OnUpdate();
    }

}
