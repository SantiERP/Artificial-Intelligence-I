using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : State
{
    SteeringAgent _agent;
    List<Transform> _safezones;
    int _index;
    float _count=0;
    float _timeToCompleteLife;

    public Flee(SteeringAgent S, List<Transform> safe, float T)
    {
        _agent = S;
        _safezones = safe;
        _timeToCompleteLife = T;
    }

    public override void OnEnter()
    {
        _index = Random.Range(0, _safezones.Count);
    }

    public override void OnExit()
    {
        _count = 0;

    }

    public override void OnUpdate()
    {
        Vector3 force = _agent.ObstacleAvoidance();
        Vector3 sterring = _agent.Arrive(_safezones[_index].position);
        _agent.AddForce(sterring * 2 + force * 2);
        _agent.Move();

        if (_count >= _timeToCompleteLife) 
        {
            _agent.gameObject.GetComponent<IDamageable>()?.ModifyLife(_agent.MaxLife);
            _agent.gameObject.GetComponent<IFuncionState>()?.DecisionTree();
        }
        _count += Time.deltaTime;
    }

}
