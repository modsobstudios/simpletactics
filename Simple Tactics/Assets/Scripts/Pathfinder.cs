using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    Grid grid;
    List<Tile> mapGrid;
    List<int> path;

    class SearchNode
    {
        Tile t;
        public List<SearchNode> edges;
        public SearchNode(Tile _t) { t = _t; }
    }

    class PlannerNode
    {
        SearchNode vertex;
        PlannerNode parent;
        float heuristicCost, givenCost, finalCost;
    }

    Dictionary<Tile, SearchNode> nodes;

    // Use this for initialization
    void Start()
    {
        grid = GetComponent<Grid>();
        mapGrid = grid.getGrid();
        //foreach(Tile t in mapGrid)
        //{
        //    nodes[t] = new SearchNode(t);
        //}
        //
        //foreach(SearchNode s in nodes)
        //{
        //    s.edges.Add()
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }
}
