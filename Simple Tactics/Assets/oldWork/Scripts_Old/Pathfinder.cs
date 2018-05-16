using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    oldGrid grid;
    List<oldTile> mapGrid;
    public List<int> path;
    public int bfsStart, bfsGoal;
    float heuristicWeight = 1.2f;
    int tileCt;
    PriorityQueue<double, Queue<PlannerNode>> open;
    class Edge
    {
        public int cost;
        public SearchNode node;
        public Edge(SearchNode _n, int _c) { node = _n; cost = _c; }
    }

    class SearchNode
    {
        public oldTile t;
        public List<Edge> edges;
        public SearchNode(oldTile _t) { t = _t; edges = new List<Edge>(); }
    }

    class PlannerNode
    {
        public SearchNode vertex;
        public PlannerNode parent;
        public float heuristicCost, givenCost, finalCost;
        public PlannerNode(SearchNode _s) { vertex = _s; }
    }

    Dictionary<oldTile, SearchNode> nodes;
    //SortedList<int, PlannerNode> open;

    // Use this for initialization
    void Start()
    {
        grid = GetComponent<oldGrid>();
        nodes = new Dictionary<oldTile, SearchNode>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            mapGrid = grid.getGrid();
            tileCt = (mapGrid[0].gridH * mapGrid[0].gridW) - 1;
            buildSearchGraph();
            BFS(nodes[mapGrid[0]], nodes[mapGrid[tileCt]]);
        }
        if (mapGrid != null)
        {
            foreach (oldTile t in mapGrid)
            {
                if (t.selected)
                {
                    foreach (Edge e in nodes[t].edges)
                    {
                        Debug.Log(e.node.t.name);
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
            BFS(nodes[mapGrid[Random.Range(0, tileCt)]], nodes[mapGrid[Random.Range(0, tileCt)]]);
        if (Input.GetKeyDown(KeyCode.X))
            greedy(nodes[mapGrid[Random.Range(0, tileCt)]], nodes[mapGrid[Random.Range(0, tileCt)]]);
        if (Input.GetKeyDown(KeyCode.Z))
            uniformCost(nodes[mapGrid[Random.Range(0, tileCt)]], nodes[mapGrid[Random.Range(0, tileCt)]]);

        if (Input.GetKeyDown(KeyCode.O))
            Astar(nodes[mapGrid[Random.Range(0, tileCt)]], nodes[mapGrid[Random.Range(0, tileCt)]]);
    }

    void buildSearchGraph()
    {
        foreach (oldTile t in mapGrid)
        {
            nodes[t] = new SearchNode(t);
        }
        foreach (oldTile t in mapGrid)
        {
            int index = t.getNorthIndex();
            if (index != -1)
                nodes[t].edges.Add(new Edge(nodes[mapGrid[index]], 1));

            index = t.getEastIndex();
            if (index != -1)
                nodes[t].edges.Add(new Edge(nodes[mapGrid[index]], 1));

            index = t.getSouthIndex();
            if (index != -1)
                nodes[t].edges.Add(new Edge(nodes[mapGrid[index]], 1));

            index = t.getWestIndex();
            if (index != -1)
                nodes[t].edges.Add(new Edge(nodes[mapGrid[index]], 1));
        }
        int x = 0;
        x = x + 1;
    }

    void BFS(SearchNode _start, SearchNode _goal)
    {
        foreach (oldTile t in mapGrid)
        {
            t.resetColor();
        }
        List<PlannerNode> open = new List<PlannerNode>();
        Dictionary<SearchNode, PlannerNode> visited = new Dictionary<SearchNode, PlannerNode>();
        open.Add(new PlannerNode(_start));
        visited[_start] = open[0];
        while (open.Count != 0)
        {
            PlannerNode current = open[0];
            open.RemoveAt(0);
            if (current.vertex == _goal)
            {
                current.vertex.t.changeColor(Color.yellow);

                while (current.parent != null)
                {
                    path.Add(current.vertex.t.gridIndex);
                    current = current.parent;
                    current.vertex.t.changeColor(Color.yellow);
                }
                return;
            }
            foreach (Edge e in current.vertex.edges)
            {
                SearchNode successor = e.node;
                if (!visited.ContainsKey(successor))
                {
                    PlannerNode successorNode = new PlannerNode(successor);
                    successorNode.parent = current;
                    visited[successor] = successorNode;
                    open.Add(successorNode);
                }
            }
        }
        return;
    }

    void greedy(SearchNode _start, SearchNode _goal)
    {
        foreach (oldTile t in mapGrid)
        {
            t.resetColor();
        }
        List<PlannerNode> open = new List<PlannerNode>();
        Dictionary<SearchNode, PlannerNode> visited = new Dictionary<SearchNode, PlannerNode>();
        open.Add(new PlannerNode(_start));
        visited[_start] = open[0];
        visited[_start].heuristicCost = calculateCost(_start, _goal);

        while (open.Count != 0)
        {
            int lowestIndex = getLowestHeuristic(open);
            PlannerNode current = open[lowestIndex];
            open.Remove(open[lowestIndex]);
            if (current.vertex == _goal)
            {
                current.vertex.t.changeColor(Color.yellow);

                while (current.parent != null)
                {
                    path.Add(current.vertex.t.gridIndex);
                    current = current.parent;
                    current.vertex.t.changeColor(Color.yellow);
                }
                return;
            }
            foreach (Edge e in current.vertex.edges)
            {
                SearchNode successor = e.node;
                if (!visited.ContainsKey(successor))
                {
                    PlannerNode successorNode = new PlannerNode(successor);
                    successorNode.parent = current;
                    successorNode.heuristicCost = calculateCost(successor, _goal);
                    visited[successor] = successorNode;

                    open.Add(successorNode);
                }
            }
        }
        return;
    }

    void uniformCost(SearchNode _start, SearchNode _goal)
    {
        foreach (oldTile t in mapGrid)
        {
            t.resetColor();
        }
        List<PlannerNode> open = new List<PlannerNode>();
        Dictionary<SearchNode, PlannerNode> visited = new Dictionary<SearchNode, PlannerNode>();
        open.Add(new PlannerNode(_start));
        visited[_start] = open[0];
        visited[_start].givenCost = 0;

        while (open.Count != 0)
        {
            int lowestIndex = getLowestGiven(open);
            PlannerNode current = open[lowestIndex];
            open.Remove(open[lowestIndex]);
            if (current.vertex == _goal)
            {
                current.vertex.t.changeColor(Color.yellow);

                while (current.parent != null)
                {
                    path.Add(current.vertex.t.gridIndex);
                    current = current.parent;
                    current.vertex.t.changeColor(Color.yellow);
                }
                return;
            }
            foreach (Edge e in current.vertex.edges)
            {
                SearchNode successor = e.node;
                float tempGiven = current.givenCost + e.cost;
                if (visited.ContainsKey(successor))
                {
                    if (tempGiven < visited[successor].givenCost)
                    {
                        PlannerNode successorNode = new PlannerNode(successor);
                        open.Remove(successorNode);
                        successorNode.givenCost = tempGiven;
                        successorNode.parent = current;
                        open.Add(successorNode);
                    }
                }
                else
                {

                    PlannerNode successorNode = new PlannerNode(successor);
                    successorNode.givenCost = tempGiven;
                    successorNode.parent = current;
                    visited[successor] = successorNode;

                    open.Add(successorNode);
                }

            }
        }
        return;
    }

    void Astar(SearchNode _start, SearchNode _goal)
    {
        foreach (oldTile t in mapGrid)
        {
            t.resetColor();
        }
        List<PlannerNode> open = new List<PlannerNode>();
        Dictionary<SearchNode, PlannerNode> visited = new Dictionary<SearchNode, PlannerNode>();
        open.Add(new PlannerNode(_start));
        visited[_start] = open[0];
        visited[_start].givenCost = 0;
        visited[_start].heuristicCost = calculateCost(_start, _goal);
        visited[_start].finalCost = visited[_start].givenCost + visited[_start].heuristicCost * heuristicWeight;

        while (open.Count != 0)
        {
            int lowestIndex = getLowestFinal(open);
            PlannerNode current = open[lowestIndex];
            open.Remove(open[lowestIndex]);
            if (current.vertex == _goal)
            {
                current.vertex.t.changeColor(Color.yellow);

                while (current.parent != null)
                {
                    path.Add(current.vertex.t.gridIndex);
                    current = current.parent;
                    current.vertex.t.changeColor(Color.yellow);
                }
                return;
            }
            foreach (Edge e in current.vertex.edges)
            {
                SearchNode successor = e.node;
                float tempGiven = current.givenCost + e.cost;
                if (visited.ContainsKey(successor))
                {
                    if (tempGiven < visited[successor].givenCost)
                    {
                        PlannerNode successorNode = new PlannerNode(successor);
                        open.Remove(successorNode);
                        successorNode.givenCost = tempGiven;
                        successorNode.finalCost = successorNode.givenCost + successorNode.heuristicCost * heuristicWeight;
                        successorNode.parent = current;
                        open.Add(successorNode);
                    }
                }
                else
                {

                    PlannerNode successorNode = new PlannerNode(successor);
                    successorNode.givenCost = tempGiven;
                    successorNode.heuristicCost = calculateCost(successor, _goal);
                    successorNode.finalCost = successorNode.givenCost + successorNode.heuristicCost * heuristicWeight;
                    successorNode.parent = current;
                    visited[successor] = successorNode;

                    open.Add(successorNode);
                }

            }
        }
        return;
    }

    int getLowestHeuristic(List<PlannerNode> _d)
    {
        float k = float.MaxValue;
        int index = -1;
        for (int i = 0; i < _d.Count; i++)
        {
            if (_d[i].heuristicCost < k)
            {
                k = _d[i].heuristicCost;
                index = i;
            }
        }

        return index;
    }

    int getLowestGiven(List<PlannerNode> _d)
    {
        float k = float.MaxValue;
        int index = -1;
        for (int i = 0; i < _d.Count; i++)
        {
            if (_d[i].givenCost < k)
            {
                k = _d[i].givenCost;
                index = i;
            }
        }

        return index;
    }

    int getLowestFinal(List<PlannerNode> _d)
    {
        float k = float.MaxValue;
        int index = -1;
        for (int i = 0; i < _d.Count; i++)
        {
            if (_d[i].finalCost < k)
            {
                k = _d[i].finalCost;
                index = i;
            }
        }

        return index;
    }

    SearchNode fancyFunction()
    {
        // i wonder what it does...
        return null;
    }

    float calculateCost(SearchNode _s, SearchNode _g)
    {
        float x1 = _s.t.getTileColumn();
        float x2 = _g.t.getTileColumn();
        float y1 = _s.t.getTileRow();
        float y2 = _g.t.getTileRow();

        return Mathf.Sqrt(Mathf.Pow((x1 - x2), 2) + Mathf.Pow((y1 - y2), 2));

    }
}
