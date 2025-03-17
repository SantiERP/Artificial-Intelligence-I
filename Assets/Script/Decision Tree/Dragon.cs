using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public Transform player;
    public bool canSeePlayer;
    public DecisionNode DecisionTree;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { }
            //DecisionTree.Execute(this);
    }

    public void StartChasing(Transform target)
    {
        Debug.Log("Persigo a " + target.gameObject.name);
    }

    public void ShootFireBall()
    {
        Debug.Log("Escupo bola de fuego");
    }
}
