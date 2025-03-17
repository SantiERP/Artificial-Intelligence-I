using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LeaderStates
{
    Flee,
    FleeTheta,
    Move,
    Attack,
    Theta
}

public class Leader : SteeringAgent, IDamageable, IFuncionState
{
    FineStateMachineLeader _fmsL;
    GameManager _gameManager;
    LosAgent _losAgent;
    Vector3 _nodeGoal;
    [SerializeField] List<SteeringAgent> _ourMinions = new List<SteeringAgent>();
    [SerializeField] NodeCreator _nodeCreator;
    [SerializeField] List<Transform> _safeZoneTeam;

    Vector3 _position = Vector3.zero;

    public Vector3 Position { get { return _position; } }
    public List<SteeringAgent> OurMinions { get => _ourMinions; }
    public FineStateMachine FmsL { get => _fmsL; }
    public LosAgent LosAgent { get => _losAgent; }

    void Start()
    {
        MoveQ = false;
        _life = MaxLife;
        _gameManager = GameManager.Instance;
        _gameManager.AddListLeader(this);
        _losAgent = GetComponent<LosAgent>();
        _visual = GetComponentInChildren<Visual>();

        _fmsL = new FineStateMachineLeader();

        _fmsL.AddState(LeaderStates.Flee, new Flee(this, _safeZoneTeam, _timeToRecover));
        _fmsL.AddState(LeaderStates.FleeTheta, new FleeTheta(this, _nodeCreator, _safeZoneTeam, _timeToRecover));
        _fmsL.AddState(LeaderStates.Move, new MoveLeader(this, _losAgent));
        _fmsL.AddState(LeaderStates.Attack, new Attack(transform, _team, _losAgent, _damage, _damageTime));
        _fmsL.AddState(LeaderStates.Theta, new ThetaLeader(this, _nodeCreator, _losAgent));

        DecisionTree();
    }

    void Update()
    {
        _fmsL?.OnUpddate();      
    }

    public void MoveToClick()
    {
        _fmsL.ChangeState(LeaderStates.Move);
    }

    public bool MovePathFinding()
    {
        _nodeGoal = GameManager.Instance.NodeObjetive.transform.position;
        if (_losAgent.InLineOfSight(_nodeGoal))
        {
            return true;
        }
        return false;
    }
    public bool MovePathFindingFlee()
    {
        Vector3 safezone = _safeZoneTeam[_index].position;
        if (_losAgent.InLineOfSight(safezone))
        {
            return true;
        }
        return false;
    }

    public void Attack()
    {
        _fmsL.ChangeState(LeaderStates.Attack);
    }

    public void Theta()
    {
        _fmsL.ChangeState(LeaderStates.Theta);
    }
    public void FleeThetaState()
    {
        _fmsL.ChangeState(LeaderStates.FleeTheta);
    }
    public Visual MyVisual()
    {
        return _visual;
    }
    public float MyLife()
    {
        return _life;
    }
    public void DecisionTree()
    {
        DecisionT.Execute(this);
    }

    public void FleeState()
    {
        _fmsL.ChangeState(LeaderStates.Flee);
    }

    public void ModifyLife(float mod)
    {
        if(mod < 0) StartCoroutine(FlashRed());
        _life += mod;
        _life = Mathf.Clamp(_life, 0, MaxLife);
        _visual.ModififyBarLife(_life, MaxLife);
        if (_life <= 0)
        {
            MoveQ = false;
            _index = Random.Range(0, _safeZoneTeam.Count);
            DecisionTree();
        }
    }

    public bool IstillAlive()
    {
        return _life > 0;
    }
    public bool CanMove()
    {
        return MoveQ;
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
    private IEnumerator FlashRed()
    {
        _visual.ChangeColor(Color.red);
        yield return new WaitForSeconds(0.8f); // Tiempo que se mantiene rojo
        _visual.BackToOriginal();
    }
}
