using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Minion;

public class FineStateMachineMinion : FineStateMachine
{
    Dictionary<MinionState, State> _allStates = new Dictionary<MinionState, State>();

    public override void ChangeState(MinionState name)
    {
        if (!_allStates.ContainsKey(name)) return;
        _currentState?.OnExit();
        _currentState = _allStates[name];
        _currentState.OnEnter();
    }

    public void AddState(MinionState name, State state)
    {
        if (!_allStates.ContainsKey(name))
            _allStates.Add(name, state);
        else
            _allStates[name] = state;

        state.FSMMinion = this;
    }
}
