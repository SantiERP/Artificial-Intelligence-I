using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tiene demasiados vecino chequear la asignacion
public class Node : MonoBehaviour
{
    [SerializeField] List<Node> _neighbors = new List<Node>();
    [SerializeField] LayerMask _nodeLayer;
    [SerializeField] float _checkRadius;
    public bool IsBlocked = false;

    public List<Node> Neighbors
    {
        get { return _neighbors; }
        set { _neighbors = value; }
    }

    public void Initialize(List<Node> allNodesInGrid)
    {
        if(IsBlocked) return;
        FindNeighbors(allNodesInGrid);

    }

    public void AddNeighbor(Node neighbor)
    {
        if (!_neighbors.Contains(neighbor))
            _neighbors.Add(neighbor);
    }

    private void FindNeighbors(List<Node> allNodesInGrid)
    {

        foreach (Node otherNode in allNodesInGrid)
        {
            if (otherNode != this)
            {
                Collider[] _collider = (Physics.OverlapSphere(transform.position, _checkRadius, _nodeLayer));
                foreach (Collider collider in _collider) 
                { 
                    Node n = collider.gameObject.GetComponent<Node>();
                    if (!n.IsBlocked && n != this)
                    {
                        AddNeighbor(n);

                    }
                }
            }
        }
    }
    public void Block(GameObject block, List<Node> n)
    {
        SetBlocked(!IsBlocked, block);
        foreach (Node node in n) node.Neighbors.Clear();
        foreach (Node node in n)
        {
            node.Initialize(n);
        }
    }

    void SetBlocked(bool block, GameObject Gblock)
    {
        IsBlocked = block;
        Instantiate(Gblock, transform.position, Quaternion.identity);
    }

    public List<Node> GetNeighbors()
    {
        return _neighbors;
    }
}
