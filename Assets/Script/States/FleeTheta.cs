using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FleeTheta : State
{
    List<Vector3> _path;
    List<Transform> _safezones;

    SteeringAgent _agent;
    NodeCreator _creator;
    Node _start;
    Node _goal;

    float _speed;
    float _count;
    float _timeToCompleteLife;


    public FleeTheta(SteeringAgent S,NodeCreator N, List<Transform> safe, float T)
    {
        _agent = S;
        _creator = N;
        _speed = S.MaxSpeed;
        _safezones = safe;
        _timeToCompleteLife = T;
    }

    public override void OnEnter()
    {
        Vector3 start = _agent.transform.position;
        foreach (Node N in _creator.GetGraph())
        {
            float dist = Vector3.Magnitude(start - N.transform.position);
            float distN = Vector3.Magnitude(_safezones[_agent.Index].position - N.transform.position);
            if (dist < 2)
            {
                _start = N;
            }
            if (distN < 2)
            {
                _goal = N;
            }

        }
        _path = PathFinding.AStar(_start,_goal);
    }

    public override void OnExit()
    {
        _count = 0;
    }

    public override void OnUpdate()
    {
        TravelPath();
        if (_count >= _timeToCompleteLife)
        {
            _agent.gameObject.GetComponent<IDamageable>()?.ModifyLife(_agent.MaxLife);
            _agent.gameObject.GetComponent<IFuncionState>()?.DecisionTree();
        }
        _count += Time.deltaTime;

    }

    void TravelPath()
    {
        if (_path == null || !_path.Any()) return;

        Vector3 dir = _path[0] - _agent.transform.position;
        _agent.transform.position += dir.normalized * _speed * Time.deltaTime;
        _agent.transform.LookAt(_path[0]);

        if (dir.magnitude <= 0.2f)
        {
            _agent.transform.LookAt(_path[0]);
            _agent.transform.position = _path[0];
            _path.RemoveAt(0);
        }

    }


}
