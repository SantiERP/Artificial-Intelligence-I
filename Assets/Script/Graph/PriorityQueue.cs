using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    public int Count { get => _allElements.Count; }
    private Dictionary<T, float> _allElements = new Dictionary<T, float>();

    public void Enqueue(T elem, float priority)
    {
        _allElements[elem] = priority;
    }

    public T Dequeue()
    {
        T lowest = default;
        foreach (var item in _allElements)
        {
            if (lowest == null)
            {
                lowest = item.Key;
                continue;
            }
            if (item.Value < _allElements[lowest])
                lowest = item.Key;
        }
        _allElements.Remove(lowest);
        return lowest;
    }
}
