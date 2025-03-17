using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class NodeCreator : MonoBehaviour
{
    [SerializeField] float _distY;
    [SerializeField] float _distX;
    [SerializeField] int _amountRow;
    [SerializeField] int _amountColumn;
    [SerializeField] LayerMask _obstacleLayer;

    [SerializeField] Node _prefabNode;
    Node N;
    Vector3 _position;
    List<Node> _nodes = new List<Node>();
    [SerializeField] List<SetPosObstacles> _obstacles = new List<SetPosObstacles>();  
    public List<SetPosObstacles> Obstacles {  get { return _obstacles; } set { _obstacles = value; } }


    private void Awake()
    {
        _position = transform.position;
        CreateGraph();
    }

    public void CreateGraph()
    {
        for (int y = 0; y < _amountRow; ++y)
        {
            for (int x = 0; x < _amountColumn; ++x)
            {
                N = Instantiate(_prefabNode, transform.position, transform.rotation);
                _nodes.Add(N);
                transform.position += Vector3.right * _distX;
            }
            _position -= Vector3.forward * _distY;
            transform.position = _position;
        }
        foreach (SetPosObstacles item in _obstacles)
        {
            item.PosObject(_nodes);
        }
        foreach (Node n in _nodes)
        {
            n.Initialize(_nodes);
        }
    } 

    public List<Node> GetGraph()
    {
        return _nodes;
    }
}
