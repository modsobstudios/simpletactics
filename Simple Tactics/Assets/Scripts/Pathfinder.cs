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
    Dictionary<Tile, SearchNode> nodes;
    Dictionary<Tile, SearchNode> tempNodes;
    List<PlannerNode> stepOpen;
    Dictionary<SearchNode, PlannerNode> stepVisited;
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
            //Debug.Log("Running A* from " + _start.name + " to " + _goal.name + "!");
            Astar(nodes[_start], nodes[_goal]);
            //Debug.Log("Returned a path of size " + path.Count + "!");
          //  highlightPath();
        }
        else
        {
            Debug.Log(_start.name + " and " + _goal.name + " are the same tile!");
            path.Clear();
        }
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
            if (index != -1 && mapGrid[index].cost != int.MaxValue)
                nodes[t].edges.Add(new Edge(nodes[mapGrid[index]], mapGrid[index].cost));

            index = t.getEastIndex();
            if (index != -1 && mapGrid[index].cost != int.MaxValue)
                nodes[t].edges.Add(new Edge(nodes[mapGrid[index]], mapGrid[index].cost));

            index = t.getSouthIndex();
            if (index != -1 && mapGrid[index].cost != int.MaxValue)
                nodes[t].edges.Add(new Edge(nodes[mapGrid[index]], mapGrid[index].cost));

            index = t.getWestIndex();
            if (index != -1 && mapGrid[index].cost != int.MaxValue)
                nodes[t].edges.Add(new Edge(nodes[mapGrid[index]], mapGrid[index].cost));
        }
        int x = 0;
        x = x + 1;
    }

    public void buildTempSearchGraph(List<Tile> _tiles)
    {
        // Start fresh
        if (tempNodes != null)
            tempNodes.Clear();
        tempNodes = new Dictionary<Tile, SearchNode>();

        // Create a node for each tile
        foreach (Tile t in _tiles)
            tempNodes[t] = new SearchNode(t);

        // Create edges for each tile
        foreach (Tile t in _tiles)
        {
            // If the tile's neighbor exists, is not impassable, and is part of the movement range, add it
            int index = t.getNorthIndex();
            if (index != -1 && mapGrid[index].cost != int.MaxValue && _tiles.Contains(mapGrid[index]))
                tempNodes[t].edges.Add(new Edge(tempNodes[mapGrid[index]], mapGrid[index].cost));

            index = t.getEastIndex();
            if (index != -1 && mapGrid[index].cost != int.MaxValue && _tiles.Contains(mapGrid[index]))
                tempNodes[t].edges.Add(new Edge(tempNodes[mapGrid[index]], mapGrid[index].cost));

            index = t.getSouthIndex();
            if (index != -1 && mapGrid[index].cost != int.MaxValue && _tiles.Contains(mapGrid[index]))
                tempNodes[t].edges.Add(new Edge(tempNodes[mapGrid[index]], mapGrid[index].cost));

            index = t.getWestIndex();
            if (index != -1 && mapGrid[index].cost != int.MaxValue && _tiles.Contains(mapGrid[index]))
                tempNodes[t].edges.Add(new Edge(tempNodes[mapGrid[index]], mapGrid[index].cost));
        }
    }

    public List<Tile> runLimitedAStar(List<Tile> _tiles, Tile _start, Tile _goal)
    {
        // Run A* on only the tiles in the specific range
        Astar(tempNodes[_start], tempNodes[_goal]);
        return path;
    }

    // TODO: Needs to be refactored to work only on the tiles returned by moveRange.
    void Astar(SearchNode _start, SearchNode _goal)
    {
        // Start fresh
        deselectPath();

        List<PlannerNode> open = new List<PlannerNode>();
        Dictionary<SearchNode, PlannerNode> visited = new Dictionary<SearchNode, PlannerNode>();

        // Start the list with the start node
        open.Add(new PlannerNode(_start));
        // We've been to the start node
        visited[_start] = open[0];
        // TODO: Figure out which costs are being used and how. Currently functional, but slightly blackboxed.
        visited[_start].givenCost = 0;
        visited[_start].heuristicCost = calculateCost(_start, _goal);
        visited[_start].finalCost = visited[_start].givenCost + visited[_start].heuristicCost * heuristicWeight;

        // While there are tiles left to explore
        while (open.Count != 0)
        {
            // Prioritize the tile with the lowest cost
            int lowestIndex = getLowestFinal(open);
            PlannerNode current = open[lowestIndex];
            // Remove it from the open list
            open.Remove(open[lowestIndex]);
            // If this is where we want to be
            if (current.vertex == _goal)
            {
                while (current.parent != null)
                {
                    // Add every node along this path to the list
                    path.Add(current.vertex.t);
                    current = current.parent;
                }
                // Return the list
                return;
            }
            // If this isn't the goal, get its neighbors
            foreach (Edge e in current.vertex.edges)
            {
                SearchNode successor = e.node;
                // Calculate cost
                float tempGiven = current.givenCost + e.cost;
                // If we've been here before
                if (visited.ContainsKey(successor))
                {
                    // If this previously visited tile is now a better choice based on our current path
                    if (tempGiven < visited[successor].givenCost)
                    {
                        // Remove the old reference, update costs, and return to the list with new priority.
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
                    // If we haven't been here before, add it to the visited list
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
        // The list is empty, return from procedure.
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

    public void getTestedMoveRange(int range, Tile current)
    {
        List<Tile> tempath;
        // Get all tiles within absolute range
        getMoveRange(range, current);
        // Add these tiles to a pathfinding search graph
        buildTempSearchGraph(moveRange);

        foreach (Tile t in moveRange)
        {
            // Reset any previous out-of-range markers
            t.outOfRange = false;
            // Find a path to the given tile
            tempath = runLimitedAStar(moveRange, current, t);
            // If the path to the tile is greater than the character's movement range
            // OR the path cannot be completed
            // Mark this tile for removal
            if (tempath.Count > range || tempath.Count == 0)
                t.outOfRange = true;
        }
        // Remove all tiles marked for removal
        moveRange.RemoveAll(t => (t.outOfRange));
        // Remove the tile beneath the character
        moveRange.Remove(current);
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


    IEnumerator stepAstar(SearchNode _start, SearchNode _goal)
    {
        // Start fresh
        deselectPath();

        stepOpen = new List<PlannerNode>();
        stepVisited = new Dictionary<SearchNode, PlannerNode>();

        // Start the list with the start node
        stepOpen.Add(new PlannerNode(_start));
        // We've been to the start node
        stepVisited[_start] = stepOpen[0];
        // TODO: Figure out which costs are being used and how. Currently functional, but slightly blackboxed.
        stepVisited[_start].givenCost = 0;
        stepVisited[_start].heuristicCost = calculateCost(_start, _goal);
        stepVisited[_start].finalCost = stepVisited[_start].givenCost + stepVisited[_start].heuristicCost * heuristicWeight;

        // While there are tiles left to explore
        while (stepOpen.Count != 0)
        {
            foreach (PlannerNode s in stepOpen)
                s.vertex.t.setTemporaryColor(Color.green);
            foreach (PlannerNode s in stepVisited.Values)
                s.vertex.t.setTemporaryColor(Color.red);

            ProcessList(_start, _goal);
            yield return new WaitForSecondsRealtime(1);
        }
    }

    IEnumerator ProcessList(SearchNode _start, SearchNode _goal)
    {
        // Prioritize the tile with the lowest cost
        int lowestIndex = getLowestFinal(stepOpen);
        PlannerNode current = stepOpen[lowestIndex];
        // Remove it from the stepOpen list
        stepOpen.Remove(stepOpen[lowestIndex]);
        // If this is where we want to be
        if (current.vertex == _goal)
        {
            while (current.parent != null)
            {
                // Add every node along this path to the list
                path.Add(current.vertex.t);
                yield return new WaitForSeconds(1);
                current.vertex.t.setTemporaryColor(Color.yellow);
                current = current.parent;
            }
            // Return the list
            yield return "success";
        }
        // If this isn't the goal, get its neighbors
        foreach (Edge e in current.vertex.edges)
        {
            SearchNode successor = e.node;
            // Calculate cost
            float tempGiven = current.givenCost + e.cost;
            // If we've been here before
            if (stepVisited.ContainsKey(successor))
            {
                // If this previously stepVisited tile is now a better choice based on our current path
                if (tempGiven < stepVisited[successor].givenCost)
                {
                    // Remove the old reference, update costs, and return to the list with new priority.
                    PlannerNode successorNode = new PlannerNode(successor);
                    stepOpen.Remove(successorNode);
                    successorNode.givenCost = tempGiven;
                    successorNode.finalCost = successorNode.givenCost + successorNode.heuristicCost * heuristicWeight;
                    successorNode.parent = current;
                    stepOpen.Add(successorNode);
                }
            }
            else
            {
                // If we haven't been here before, add it to the stepVisited list
                PlannerNode successorNode = new PlannerNode(successor);
                successorNode.givenCost = tempGiven;
                successorNode.heuristicCost = calculateCost(successor, _goal);
                successorNode.finalCost = successorNode.givenCost + successorNode.heuristicCost * heuristicWeight;
                successorNode.parent = current;
                stepVisited[successor] = successorNode;

                stepOpen.Add(successorNode);
            }
        }
    }
}
