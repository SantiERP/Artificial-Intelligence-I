using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ThetaLeader : State
{
    List<Vector3> _path;
    List<IDamageable> _damageables;
    Leader _leader;
    Transform _transform;
    NodeCreator _creator;
    Node _start;
    LosAgent _losAgent;
    float _speed;

    public ThetaLeader(Leader L, NodeCreator N, LosAgent F)
    {
        _leader = L;
        _transform = L.transform;
        _creator = N;
        _losAgent = F;
        _speed = L.MaxSpeed;
    }

    public override void OnEnter()
    {
        Vector3 start = _transform.position;
        foreach(Node N in _creator.GetGraph())
        {
            float dist = Vector3.Magnitude(start - N.transform.position);
            if (dist < 2)
            {
                _start = N;
            }

        }
        _path = PathFinding.AStar(_start, GameManager.Instance.NodeObjetive);
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (_leader.SearchOpponent()) { _leader.MoveQ = false; _leader.DecisionTree(); }
        TravelPath();
    }

    void TravelPath()
    {
        if (_path == null || !_path.Any()) return;
        _transform.LookAt(_path[0]);
        Vector3 dir = _path[0] - _transform.position;
        _transform.position += dir.normalized * _speed * Time.deltaTime;

        if (dir.magnitude <= 0.1f)
        {
            _transform.position = _path[0];
            _path.RemoveAt(0);
        }

    }

}
