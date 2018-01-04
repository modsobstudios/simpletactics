using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    Grid grid;
    List<Tile> mapGrid;
    public List<int> path;
    public int bfsStart, bfsGoal;
    int tileCt;

    class Edge
    {
        public int cost;
        public SearchNode node;
        public Edge(SearchNode _n, int _c) { node = _n; cost = _c; }
    }

    class SearchNode
    {
        public Tile t;
        public List<Edge> edges;
        public SearchNode(Tile _t) { t = _t; edges = new List<Edge>(); }
    }

    class PlannerNode
    {
        public SearchNode vertex;
        public PlannerNode parent;
        public float heuristicCost, givenCost, finalCost;
        public PlannerNode(SearchNode _s) { vertex = _s; }
    }

    Dictionary<Tile, SearchNode> nodes;
    //SortedList<int, PlannerNode> open;

    // Use this for initialization
    void Start()
    {
        grid = GetComponent<Grid>();
        nodes = new Dictionary<Tile, SearchNode>();

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
            foreach (Tile t in mapGrid)
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
    }

    void buildSearchGraph()
    {
        foreach (Tile t in mapGrid)
        {
            nodes[t] = new SearchNode(t);
        }
        foreach (Tile t in mapGrid)
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
        foreach(Tile t in mapGrid)
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

    SearchNode fancyFunction()
    {
        // i wonder what it does...
        return null;
    }
}
