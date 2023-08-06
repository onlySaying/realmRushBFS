using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoodinates { get { return startCoordinates; } }
    [SerializeField] Vector2Int destinateCoordinates;
    public Vector2Int DestinationCoodinates { get { return destinateCoordinates; } }


    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if(gridManager != null) 
        {
            grid = gridManager.Grid;

            startNode = grid[startCoordinates];
            destinationNode = grid[destinateCoordinates];
            
        }
    }
    void Start()
    {
        GetNewPath();
    }
    
    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.RestetNodes();
        BreathFirstSearch(coordinates);
        return BuildPath();
    }

    void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();

        foreach(Vector2Int direction in directions)
        {
            Vector2Int neighborcoords = currentSearchNode.coordinates + direction;

            if(grid.ContainsKey(neighborcoords)) 
            {
                neighbors.Add(grid[neighborcoords]);
            }
        }

        foreach(Node neighbor in neighbors) 
        {
            if(!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.ConnectTo = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    void BreathFirstSearch(Vector2Int coordinates)
    {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear();
        reached.Clear();
        bool isRunning = true;

        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates, grid[coordinates]);
        while (frontier.Count > 0) 
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if(currentSearchNode.coordinates == destinateCoordinates)
            {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath ()
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true; 

        while(currentNode.ConnectTo != null)
        {
            currentNode = currentNode.ConnectTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();
        return path;
    }

    public bool willBlockPath(Vector2Int coordinates)
    {
        if(grid.ContainsKey(coordinates)) 
        {
            bool previousState = grid[coordinates].isWalkable;
            grid[coordinates].isWalkable = false;
            List<Node> newpath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            if(newpath.Count <= 1) 
            {
                GetNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifiReceivers()
    {
        BroadcastMessage("RecalculatePath", false ,SendMessageOptions.DontRequireReceiver);
    }
}
