using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNode : DecisionNode
{
    public DecisionNode falseNode;
    public DecisionNode trueNode;

    public Questions question;

    public override void Execute(IFuncionState NPC)
    {
        switch (question)
        {
            case Questions.ReceiveOrders:
                if (NPC.CanMove())
                {
                    trueNode.Execute(NPC); 
                }
                else 
                {
                    falseNode.Execute(NPC);
                }
                break;
            case Questions.HowMove:
                if (NPC.MovePathFinding())
                {
                    trueNode.Execute(NPC); 
                }
                else
                {
                    falseNode.Execute(NPC);
                }
                break;
            case Questions.HaveLifeToAttack:
                if (NPC.IstillAlive())
                {
                    trueNode.Execute(NPC);
                }
                else 
                { 
                    falseNode.Execute(NPC); 
                }
                break;
            case Questions.WichFlee:
                if (NPC.MovePathFindingFlee()) 
                {
                    trueNode.Execute(NPC);
                    Debug.Log("theta");
                }
                else falseNode.Execute(NPC);
                break;
        }
    }

    public enum Questions
    {
        ReceiveOrders,
        HowMove,
        HaveLifeToAttack,
        WichFlee
    }

    void OnDrawGizmos()
    {
        if (falseNode != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, falseNode.transform.position);  
        }
        if (trueNode != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, trueNode.transform.position);
        }
    }
}
