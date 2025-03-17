using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : DecisionNode
{
    public Actions action;
    public override void Execute(IFuncionState NPC)
    {
        switch (action)
        {
            case Actions.Wait:
                break;
            case Actions.Move:
                NPC.MoveToClick();
                break;
            case Actions.MovePathFinding:
                NPC.Theta();
                break;
            case Actions.Attack:
                NPC.Attack();
                break;
            case Actions.Flee:
                NPC.FleeState();
                break;
            case Actions.FleeTheta:
                NPC.FleeThetaState();
                break;               
        }
    }

    public enum Actions
    {
        Move,
        MovePathFinding,
        Wait,
        Attack,
        Flee,
        FleeTheta
    }
}
