using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosObstacles : MonoBehaviour
{
    [SerializeField] float _magnitude;
    public void PosObject(List<Node> _nodes)
    {
        foreach (Node node in _nodes) 
        { 
            float dis = Vector3.Magnitude(node.transform.position - transform.position);
            if (dis < _magnitude) 
            {
                transform.position = node.transform.position;
                node.IsBlocked = true;
            }
        }
    }
}
