using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Team
{
    White,
    Black
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] float _distPointclicktoNode;
    [SerializeField] NodeCreator _nodeCreator;
    [SerializeField] GameObject _blockerNode;

    Vector3 _positionNode;

    public NodeCreator NodeC { get => _nodeCreator;}

    Node _node;
    public Node NodeObjetive { get => _node; }

    List<SteeringAgent> _minion = new List<SteeringAgent>();
    List<Leader> _leader = new List<Leader>();
    List<IDamageable> _allCharacters = new List<IDamageable>();
    Leader _leaderToMove = null;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectTeam();
        }
        if (Input.GetMouseButtonDown(1) && _leaderToMove )
        {
            MoveTeam(_leaderToMove);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    void SelectTeam()
    {
        Ray mousepos = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mousepos, out hit))
        {
            foreach (Leader leader in _leader)
            {
                Vector3 dis = leader.transform.position - hit.point;
                if (dis.magnitude < 2)
                {
                    _leaderToMove = leader;
                }
            }
        }

    }

    void MoveTeam(Leader L)
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (Physics.Raycast(r, out hitData))
        {
            _positionNode = hitData.point;
            foreach (Node n in _nodeCreator.GetGraph())
            {
                float dist = Vector3.Magnitude(n.gameObject.transform.position - _positionNode);
                if (dist < _distPointclicktoNode && L.IstillAlive())
                {
                    _node = n;
                    L.MoveQ = true;
                    L.DecisionTree();
                    foreach (Minion item in L.OurMinions) 
                    {
                        if (item.IstillAlive())
                        {
                            item.MoveQ = true; item.DecisionTree();
                        }
                    }
                    break;
                }
            }
        }

    }

    public void AddListMinion(SteeringAgent minion)
    {
        _minion.Add(minion);
        Minion m = minion.gameObject.GetComponent<Minion>();
        _allCharacters.Add(m);
    }
    public void RemoveListMinion(SteeringAgent Minion)
    {
        _minion.Remove(Minion);
    }
    public void AddListLeader(Leader L)
    {
        _leader.Add(L);
        _allCharacters.Add(L);
    }
    public void AddObstacle(SetPosObstacles O)
    {
        NodeC.Obstacles.Add(O);
    }
    public List<Leader> GetListAllLeaders()
    {
        return _leader;
    }
    public List<SteeringAgent> GetListAllMinions()
    {
        return _minion;
    }
    public List<IDamageable> GetListAllCharacters()
    {
        return _allCharacters;
    }
}

