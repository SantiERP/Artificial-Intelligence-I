using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFuncionState 
{
    public bool MovePathFinding();
    public bool MovePathFindingFlee();
    public bool IstillAlive();
    public bool CanMove();
    public void MoveToClick();
    public void Theta();
    public void Attack();
    public void FleeState();
    public void FleeThetaState();
    public void DecisionTree();
}
