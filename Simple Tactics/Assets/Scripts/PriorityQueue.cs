using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue : MonoBehaviour
{



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}


// Modified from Eric Lippert's article 'Path Finding Using A* in C# 3.0, Part Three
// https://blogs.msdn.microsoft.com/ericlippert/2007/10/08/path-finding-using-a-in-c-3-0-part-three/
public class PriorityQueue<P, V>
{
    private SortedDictionary<P, Queue<V>> list = new SortedDictionary<P, Queue<V>>();
    public void Enqueue(P priority, V value)
    {
        Queue<V> q;
        if (!list.TryGetValue(priority, out q))
        {
            q = new Queue<V>();
            list.Add(priority, q);
        }
        q.Enqueue(value);
    }
    public V Dequeue()
    {
        // will throw if there isn’t any first element!
        var pair = list.GetEnumerator().Current;
        var v = pair.Value.Dequeue();
        if (pair.Value.Count == 0) // nothing left of the top priority.
            list.Remove(pair.Key);
        return v;
    }
    public bool IsEmpty
    {
        get { return (list.Count == 0); }
    }

    public V Front()
    {
        var p = list.GetEnumerator().Current;
        return p.Value.Peek();
    }
}