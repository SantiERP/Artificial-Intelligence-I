using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinionState
{
    Flee,
    FleeTheta,
    Move,
    Theta,
    Attack
}

public class Minion : SteeringAgent, IDamageable, IFuncionState
{
    GameManager _gameManager;
    [Range(0f,5f)] public float CohesionWeight = 1;
    [Range(0f,5)] public float SeparationWeight = 1;
    [Range(0f, 5)] public float AlignmentWeight = 1;
    [Range(0f, 5)] public float ArrivetWeight = 1;
    [Range(0f, 5)] public float ObstacleDancetWeight = 1;

    FineStateMachineMinion _fmsM;
    NodeCreator _nodeC;
    LosAgent _losAgent;
    public LosAgent LosAgent { get => _losAgent; }
    public FineStateMachineMinion FmsM { get => _fmsM; }
    [SerializeField] Leader _leaderManager;
    [SerializeField] List<Transform> _safeZoneTeam;

    void Start()
    {
        MoveQ = false;
        _life = MaxLife;
        _losAgent = GetComponent<LosAgent>();
        _visual = GetComponentInChildren<Visual>();
        _gameManager = GameManager.Instance;
        _gameManager.AddListMinion(this);
        _nodeC = GameManager.Instance.NodeC;
        
        _fmsM = new FineStateMachineMinion();

        _fmsM.AddState(MinionState.Move, new MoveMinion(this, _leaderManager, _gameManager.GetListAllMinions(),_losAgent));
        _fmsM.AddState(MinionState.Theta, new ThetaMinion(this, _nodeC, _gameManager.GetListAllMinions(),_maxSpeed));
        _fmsM.AddState(MinionState.Attack, new Attack(transform,_team,_losAgent,_damage, _damageTime));
        _fmsM.AddState(MinionState.Flee, new Flee(this, _safeZoneTeam, _timeToRecover));
        _fmsM.AddState(MinionState.FleeTheta, new FleeTheta(this, _nodeC, _safeZoneTeam, _timeToRecover));
        DecisionTree();
    }

    void Update()
    {
        _fmsM.OnUpddate();
    }

    public void DecisionTree()
    {
        DecisionT.Execute(this);
    }

    public void MoveToClick()
    {
        _fmsM.ChangeState(MinionState.Move);
    }
    public void Theta()
    {
        _fmsM.ChangeState(MinionState.Theta);
    }
    public void Attack()
    {
        _fmsM.ChangeState(MinionState.Attack);
    }

    public void FleeState()
    {
        _fmsM.ChangeState(MinionState.Flee);
    }
    public void FleeThetaState()
    {
        _fmsM.ChangeState(MinionState.FleeTheta);
    }
    public Visual MyVisual()
    {
        return _visual;
    }

    public bool IstillAlive()
    {
        return _life > 0;
    }
    public float MyLife()
    {
        return _life;
    }
    public bool CanMove()
    {
        return MoveQ;
    }

    public void ModifyLife(float damage)
    {
        if (damage < 0) StartCoroutine(FlashRed());
        _life += damage;
        _life = Mathf.Clamp(_life, 0, MaxLife);
        _visual.ModififyBarLife(_life, MaxLife);
        if (_life <= 0) 
        {
            MoveQ = false; 
            DecisionTree();
            _index = Random.Range(0, _safeZoneTeam.Count);
        }
    }

    public Vector3 MyPosition()
    {
        return transform.position;
    }

    public Team MyTeam()
    {
        return _team;
    }

    public bool SearchOpponent()
    {
        foreach (var agent in _gameManager.GetListAllCharacters())
        {
            Team T = agent.MyTeam();
            if (_losAgent.InFieldOfView(agent.MyPosition()) && T != _team)
            {
                return true;
            }
        }
        return false;
    }
    public bool MovePathFinding()
    {
        Vector3 _nodeGoal = GameManager.Instance.NodeObjetive.transform.position;
        if (_losAgent.InLineOfSight(_nodeGoal))
        {
            return true;
        }
        return false;
    }

    public bool MovePathFindingFlee()
    {
        Vector3 _nodeGoal = _safeZoneTeam[_index].position;
        if (_losAgent.InLineOfSight(_nodeGoal))
        {
            return true;
        }
        return false;
    }
    private IEnumerator FlashRed()
    {
        _visual.ChangeColor(Color.red);
        yield return new WaitForSeconds(0.8f); // Tiempo que se mantiene rojo
        _visual.BackToOriginal();
    }

}
