using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isPlaceable;
    [SerializeField] Tower towerPrefab;
    GridManager gridManager;
    public bool IsPlaceable { get { return isPlaceable; }}
    Vector2Int coordinates = new Vector2Int();

    [SerializeField]PathFinder pathFinder ;

    private void Awake()
    {
        gridManager= FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    private void Start()
    {
        if (gridManager != null) 
        { 
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position); 
            if(!isPlaceable) 
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }
    private void OnMouseDown()
    {
      
        if (gridManager.GetNode(coordinates).isWalkable && !pathFinder.willBlockPath(coordinates))
        {
            // 타워가 만들어지지 않을때도 작동함 이 때 타워는 createtower에 의해 타워가 생성되지는 않지만 땅이
            // 사용불가능 땅으로 만들어짐 
            bool isSuccessful = towerPrefab.CreateTower(towerPrefab, transform.position);
            if(isSuccessful)
            {
                gridManager.BlockNode(coordinates);
                pathFinder.NotifiReceivers();
            }
        }
        /*if(isPlaceable) 
        {
            bool isPlaced = towerPrefab.CreateTower(towerPrefab, transform.position);
            isPlaceable = !isPlaced;
        }*/
        
    } 

    
}