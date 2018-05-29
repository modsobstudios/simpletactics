using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    Grid grid;
    List<Tile> mapGrid;
    public List<Tile> atkRange;
    public List<Tile> innerAtkRange;
    public List<Tile> innerAtkExtents;
    public List<Tile> moveRangeExtents;
    public List<Tile> path;
    public List<Tile> moveAndAtkRange;
    private List<Tile> moveRange;
    public int bfsStart, bfsGoal;
    float heuristicWeight = 1.2f;
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

    public List<Tile> Path
    {
        get
        {
            return path;
        }

        set
        {
            path = value;
        }
    }

    public List<Tile> MoveRange
    {
        get
        {
            return moveRange;
        }

        set
        {
            moveRange = value;
        }
    }

    public List<Tile> AtkRange
    {
        get
        {
            return atkRange;
        }

        set
        {
            atkRange = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        atkRange = new List<Tile>();
        innerAtkRange = new List<Tile>();
        innerAtkExtents = new List<Tile>();
        moveRange = new List<Tile>();
        moveAndAtkRange = new List<Tile>();
        moveRangeExtents = new List<Tile>();

        grid = GameObject.Find("Grid").GetComponent<Grid>();
        nodes = new Dictionary<Tile, SearchNode>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Astar(nodes[mapGrid[Random.Range(0, tileCt)]], nodes[mapGrid[Random.Range(0, tileCt)]]);
            highlightPath();
        }
    }

    public void initializePathfinding()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        mapGrid = grid.getGrid();
        tileCt = (grid.Height * grid.Width) - 1;
        buildSearchGraph();
    }

    public List<Tile> getPath(Tile _start, Tile _goal)
    {
        if (_start != _goal)
        {
            Astar(nodes[_start], nodes[_goal]);
            //highlightPath();
        }
        else
            path.Clear();
        return path;
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


    void Astar(SearchNode _start, SearchNode _goal)
    {
        deselectPath();
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
                // current.vertex.t.setTemporaryColor(Color.yellow);

                while (current.parent != null)
                {
                    path.Add(current.vertex.t);
                    current = current.parent;
                    //  current.vertex.t.setTemporaryColor(Color.yellow);
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

    void highlightPath()
    {
        foreach (Tile t in path)
            t.setTemporaryColor(Color.yellow);
    }

    void deselectPath()
    {
        foreach (Tile t in path)
        {
            t.setDefaultColor();
        }
        path.Clear();
    }


    public void getMoveRange(int range, Tile current)
    {
        // If there's range left
        if (range > 0)
        {
            // Add existing neighbors to the move range
            if (current.getNorthIndex() != -1)
                moveRange.Add(mapGrid[current.getNorthIndex()]);
            if (current.getEastIndex() != -1)
                moveRange.Add(mapGrid[current.getEastIndex()]);
            if (current.getSouthIndex() != -1)
                moveRange.Add(mapGrid[current.getSouthIndex()]);
            if (current.getWestIndex() != -1)
                moveRange.Add(mapGrid[current.getWestIndex()]);

            // Recurse all existing neighbors
            if (current.getNorthIndex() != -1)
                getMoveRange(range - 1, mapGrid[current.getNorthIndex()]);
            if (current.getEastIndex() != -1)
                getMoveRange(range - 1, mapGrid[current.getEastIndex()]);
            if (current.getSouthIndex() != -1)
                getMoveRange(range - 1, mapGrid[current.getSouthIndex()]);
            if (current.getWestIndex() != -1)
                getMoveRange(range - 1, mapGrid[current.getWestIndex()]);
        }
        else
        {
            // If this tile is at the end of a range, it is an outer border.
            moveRangeExtents.Add(current);
        }
    }

    public void getInnerAtkRange(int minRange, Tile current)
    {
        // If there is range left
        if (minRange > 0)
        {
            // Add existing tiles to the inner range
            if (current.getNorthIndex() != -1)
                innerAtkRange.Add(mapGrid[current.getNorthIndex()]);
            if (current.getEastIndex() != -1)
                innerAtkRange.Add(mapGrid[current.getEastIndex()]);
            if (current.getSouthIndex() != -1)
                innerAtkRange.Add(mapGrid[current.getSouthIndex()]);
            if (current.getWestIndex() != -1)
                innerAtkRange.Add(mapGrid[current.getWestIndex()]);

            // Recurse all existing neighbors
            if (current.getNorthIndex() != -1)
                getInnerAtkRange(minRange - 1, mapGrid[current.getNorthIndex()]);
            if (current.getEastIndex() != -1)
                getInnerAtkRange(minRange - 1, mapGrid[current.getEastIndex()]);
            if (current.getSouthIndex() != -1)
                getInnerAtkRange(minRange - 1, mapGrid[current.getSouthIndex()]);
            if (current.getWestIndex() != -1)
                getInnerAtkRange(minRange - 1, mapGrid[current.getWestIndex()]);
        }
        else
        {
            // If this tile is at the end of a range, it is an outer border
            innerAtkExtents.Add(current);
        }
    }

    public void getOuterAtkRange(int maxRange, Tile current)
    {
        // If there is range left
        if (maxRange > 0)
        {
            if (current.getNorthIndex() != -1)
         //       if (!innerAtkRange.Contains(mapGrid[current.getNorthIndex()]))
                    atkRange.Add(mapGrid[current.getNorthIndex()]);
            if (current.getEastIndex() != -1)
           //     if (!innerAtkRange.Contains(mapGrid[current.getEastIndex()]))
                    atkRange.Add(mapGrid[current.getEastIndex()]);
            if (current.getSouthIndex() != -1)
             //   if (!innerAtkRange.Contains(mapGrid[current.getSouthIndex()]))
                    atkRange.Add(mapGrid[current.getSouthIndex()]);
            if (current.getWestIndex() != -1)
               // if (!innerAtkRange.Contains(mapGrid[current.getWestIndex()]))
                    atkRange.Add(mapGrid[current.getWestIndex()]);
            if (current.getNorthIndex() != -1)
                getOuterAtkRange(maxRange - 1, mapGrid[current.getNorthIndex()]);
            if (current.getEastIndex() != -1)
                getOuterAtkRange(maxRange - 1, mapGrid[current.getEastIndex()]);
            if (current.getSouthIndex() != -1)
                getOuterAtkRange(maxRange - 1, mapGrid[current.getSouthIndex()]);
            if (current.getWestIndex() != -1)
                getOuterAtkRange(maxRange - 1, mapGrid[current.getWestIndex()]);
        }
    }

    public void getRangedAtkRange(int minRange, int maxRange, Tile current)
    {
        getInnerAtkRange(minRange, current);
        getOuterAtkRange(maxRange, current);
        // Remove the inner range from the outer range rather than check the inner list for every tile.
        atkRange.RemoveAll(t => innerAtkRange.Contains(t));
    }

    public void resetLists()
    {
        atkRange.Clear();
        moveRange.Clear();
        moveRangeExtents.Clear();
        innerAtkExtents.Clear();
        innerAtkRange.Clear();
    }
}
