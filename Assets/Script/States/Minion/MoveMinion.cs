using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMinion : State
{
    Minion _minion;
    Transform _leaderPosition;
    List<SteeringAgent> _agents;
    LosAgent _losAgent;
    Vector3 _node;

    public MoveMinion(Minion M, Leader L, List<SteeringAgent> List,LosAgent losAgent)
    {
        _minion = M;
        _leaderPosition = L.transform;
        _agents = List;
        _losAgent = losAgent;
    }

    public override void OnEnter()
    {
        _node = GameManager.Instance.NodeObjetive.transform.position;
        if (!_losAgent.InLineOfSight(_node))
        {
            _minion.DecisionTree();
        }

    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (_minion.SearchOpponent()) {_minion.MoveQ = false; _minion.DecisionTree(); }

        Flocking();
        _minion.Move();

        float dis = Vector3.Distance(_minion.transform.position, _node);
    }

    void Flocking()
    {
        Vector3 force = _minion.ObstacleAvoidance()*_minion.ObstacleDancetWeight;
        Vector3 arrive = _minion.Arrive(_leaderPosition.position) * _minion.ArrivetWeight;
        Vector3 sterring =_minion.Separation(_agents) * _minion.SeparationWeight + _minion.Alignment(_agents)*_minion.AlignmentWeight
            + _minion.Cohesion(_agents) * _minion.CohesionWeight;
        _minion.AddForce(arrive + sterring + force );
    }
}
