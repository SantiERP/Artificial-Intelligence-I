using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFinding
{
    static readonly List<Vector3> EMPTY = new List<Vector3>();

    public static List<Vector3> AStar(Node start, Node goal)
    {
        var frontier = new PriorityQueue<Node>();
        // var frontier2 = new Priority
        frontier.Enqueue(start, 0);

        var cameFrom = new Dictionary<Node, Node>(); //path A->B is stored as came_from[B] == A
        cameFrom.Add(start, null);

        var costSoFar = new Dictionary<Node, int>();
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            if (current == goal)
            {
                var path = new List<Vector3>();
                while (current != start)
                {
                    path.Add(current.transform.position);
                    current = cameFrom[current];
                }
                path.Add(start.transform.position); // optional
                path.Reverse();

                return path;
            }

            foreach (var item in current.Neighbors)
            {
                if (item.IsBlocked) continue;
                var newCost = costSoFar[current];
                if (!costSoFar.ContainsKey(item) || costSoFar[item] > newCost)
                {
                    frontier.Enqueue(item, newCost + Heuristic(item.transform.position, goal.transform.position));
                    cameFrom[item] = current;
                    costSoFar[item] = newCost;
                }
            }
        }

        return null;
    }

    public static List<Vector3> ThetaStar(Node start, Node goal, LayerMask wallLayer)
    {
        if (start == null || goal == null) return EMPTY;

        var path = AStar(start, goal);

        int current = 0;

        while (current + 2 < path.Count)
        {
            if (InLineOfSight(path[current], path[current + 2], wallLayer))
                path.RemoveAt(current+1);
            else current++;
        }

        return path;
    }



    public static bool InLineOfSight(Vector3 start, Vector3 end, LayerMask layer)
    {
        Vector3 dir = (end - start);
        return !Physics.Raycast(start, dir, dir.magnitude, layer);
    }

    public static float Heuristic(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b);
    }

    public static IEnumerator BFSRoutine(Node start, Node goal)
    {
        var time = new WaitForSeconds(0.01f);

        var frontier = new Queue<Node>();
        frontier.Enqueue(start);

        var cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);
        Node current = null;
        while (frontier.Count > 0)
        {
            current = frontier.Dequeue();

            yield return time;
            if (current == goal) break;

            foreach (var item in current.Neighbors)
            {
                if (item.IsBlocked) continue;
                if (!cameFrom.ContainsKey(item))
                {
                    frontier.Enqueue(item);
                    cameFrom.Add(item, current);
                }
            }

            //yield return time;

        }

        if (current == goal)
        {
            while (current != null)
            {
                current = cameFrom[current];
                yield return time;
            }
        }
    }


    public static IEnumerator DijkstraRoutine(Node start, Node goal)
    {
        var time = new WaitForSeconds(0.01f);

        var frontier = new PriorityQueue<Node>();
        // var frontier2 = new Priority
        frontier.Enqueue(start, 0);

        var cameFrom = new Dictionary<Node, Node>(); //path A->B is stored as came_from[B] == A
        cameFrom.Add(start, null);

        var costSoFar = new Dictionary<Node, int>();
        costSoFar[start] = 0;
        Node current = null;

        while (frontier.Count > 0)
        {
            current = frontier.Dequeue();

            yield return time;
            if (current == goal) break;

            foreach (var item in current.Neighbors)
            {
                if (item.IsBlocked) continue;
                var newCost = costSoFar[current];
                if (!costSoFar.ContainsKey(item) || costSoFar[item] > newCost)
                {
                    frontier.Enqueue(item, newCost);
                    cameFrom[item] = current;
                    costSoFar[item] = newCost;
                }
            }
            yield return time;
        }

        if (current == goal)
        {
            while (current != null)
            {
                current = cameFrom[current];
                yield return time;
            }
        }
    }

    public static IEnumerator AstarRoutine(Node start, Node goal)
    {
        var time = new WaitForSeconds(0.01f);

        var frontier = new PriorityQueue<Node>();
        // var frontier2 = new Priority
        frontier.Enqueue(start, 0);

        var cameFrom = new Dictionary<Node, Node>(); //path A->B is stored as came_from[B] == A
        cameFrom.Add(start, null);

        var costSoFar = new Dictionary<Node, int>();
        costSoFar[start] = 0;
        Node current = null;

        while (frontier.Count > 0)
        {
            current = frontier.Dequeue();

            yield return time;
            if (current == goal) break;

            foreach (var item in current.Neighbors)
            {
                if (item.IsBlocked) continue;
                var newCost = costSoFar[current];
                if (!costSoFar.ContainsKey(item) || costSoFar[item] > newCost)
                {
                    frontier.Enqueue(item, newCost + Heuristic(item.transform.position, goal.transform.position));
                    cameFrom[item] = current;
                    costSoFar[item] = newCost;
                }
            }
            yield return time;
        }

        if (current == goal)
        {
            while (current != null)
            {
                current = cameFrom[current];
                yield return time;
            }
        }
    }
}
