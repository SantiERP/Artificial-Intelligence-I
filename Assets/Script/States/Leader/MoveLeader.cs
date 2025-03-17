using UnityEngine;

public class MoveLeader : State
{
    Transform _leaderTransform;
    Leader _leader;
    Vector3 _node;

    public MoveLeader(Leader L, LosAgent Los)
    {
        _leader = L;
        _leaderTransform = L.transform;
    }

    public override void OnEnter()
    {
        _node = GameManager.Instance.NodeObjetive.transform.position;
        _leaderTransform.LookAt(_node);
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (_leader.SearchOpponent()) { _leader.MoveQ = false; _leader.DecisionTree(); } 

        GoToNode();
        _leader.Move();
    }

    void GoToNode()
    {
        //sumarle amvbas fuerzas
        Vector3 steering = _leader.Arrive(_node);

        _leader.AddForce(steering * 2f);
        
        float dis = Vector3.Distance(_leaderTransform.position, _node);
        if (dis <= 0.8f)
        {
            _leader.MoveQ = false;
            _leader.DecisionTree();
        }
        
    }
}
