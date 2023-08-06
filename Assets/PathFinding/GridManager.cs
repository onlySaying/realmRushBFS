using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    [Tooltip("World Grid Size - should match UnityEditor snap Settings.")]
    [SerializeField] int unityGridSize = 10;
    public int UnityGridSize { get{ return unityGridSize; } }
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } }
    private void Awake()
    {
        CreateGrid();
    }

    public Node GetNode(Vector2Int coordinates)
    {
        if(!grid.ContainsKey(coordinates))
        {
            return null;
        }
        return grid[coordinates];
    } 

    public void BlockNode(Vector2Int coordinates)
    {
        if(grid.ContainsKey(coordinates)) 
        {
            grid[coordinates].isWalkable = false;
        }
    }

    public void RestetNodes()
    {
        foreach(KeyValuePair<Vector2Int, Node> entry in grid)
        {
            entry.Value.ConnectTo = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
        }

    }

     public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
       
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x / unityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z / unityGridSize);

        return coordinates;
    }

    public Vector3 GetPositionFromCoordinates(Vector2Int coodinates)
    {
        Vector3 position = new Vector3();
        position.x = coodinates.x * unityGridSize;
        position.z = coodinates.y * unityGridSize;

        return position;
    }

    void CreateGrid()
    {
        for(int x = 0; x < gridSize.x; x++) 
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coordinate = new Vector2Int(x, y);
                grid.Add(coordinate, new Node(coordinate, true));
            }
        }
    }

}
