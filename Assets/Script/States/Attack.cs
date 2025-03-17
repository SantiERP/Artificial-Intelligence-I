using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    LosAgent _losAgent;
    Transform _transform;
    List<IDamageable> _damageables = new List<IDamageable>();
    Team _team;
    float _damage;
    float _countTime;
    float _damageTime;

    public Attack(Transform T,Team r,LosAgent LS, float damage, float damagetime)
    {
        _transform = T;
        _team = r;
        _losAgent = LS;
        _damage = damage;
        _damageTime = damagetime;
    }

    public override void OnEnter()
    {
        _damageables = GameManager.Instance.GetListAllCharacters();
    }

    public override void OnExit()
    {
        _countTime = 0;
    }

    public override void OnUpdate()
    {
        foreach (var agent in _damageables)
        {
            Team T = agent.MyTeam();
            if (_losAgent.InFieldOfView(agent.MyPosition()) && T != _team && agent.MyLife()>0)
            {
                if (_countTime >= _damageTime)
                {
                    agent.ModifyLife(_damage * -1);
                    _countTime = 0;
                }
                _countTime += Time.deltaTime;
                _transform.LookAt(agent.MyPosition());
                Debug.DrawLine(_transform.position, agent.MyPosition(), Color.red);
            }
        }
    }
}
