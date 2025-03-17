using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Leader;

public class FineStateMachineLeader : FineStateMachine 
{
    Dictionary<LeaderStates, State> _allStates = new Dictionary<LeaderStates, State>();

    public override void ChangeState(LeaderStates name)
    {
        if (!_allStates.ContainsKey(name)) return;
        _currentState?.OnExit();
        _currentState = _allStates[name];
        _currentState.OnEnter();
    }

    public void AddState(LeaderStates name, State state)
    {
        if (!_allStates.ContainsKey(name))
            _allStates.Add(name, state);
        else
            _allStates[name] = state;

        state.FSMLeader = this;
    }
}
