using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ThetaMinion : State
{
    List<Vector3> _path;
    List<SteeringAgent> _agents;

    Minion _minion;
    Transform _transform;
    NodeCreator _creator;
    Node _start;

    float _speed;

    public ThetaMinion(Minion M, NodeCreator N, List<SteeringAgent> List, float speed)
    {
        _minion = M;
        _transform = M.transform;
        _creator = N;
        _agents = List;
        _speed = speed;
    }

    public override void OnEnter()
    {
        Vector3 start = _transform.position;
        foreach (Node N in _creator.GetGraph())
        {
            float dist = Vector3.Magnitude(start - N.transform.position);
            if (dist < 2)
            {
                _start = N;
            }

        }
        _minion.transform.LookAt(GameManager.Instance.NodeObjetive.transform);

        _path = PathFinding.AStar(_start, GameManager.Instance.NodeObjetive);

    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (_minion.SearchOpponent()) {_minion.MoveQ = false; _minion.DecisionTree(); }
        TravelPath();
    }

    void TravelPath() 
    {
        if (_path == null || !_path.Any()) return;

        Vector3 dir = _path[0] - _transform.position;
        _transform.position += dir.normalized * _speed * Time.deltaTime;
        if (dir.magnitude <= 0.1f)
        {
            _transform.LookAt(_path[1]);
            _transform.position = _path[0];
            _path.RemoveAt(0);
        }

        if (dir.magnitude <= 0.5f && _path.Count == 1) { _minion.DecisionTree(); }
    }

}
