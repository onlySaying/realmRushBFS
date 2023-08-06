using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CordinateLabeler : MonoBehaviour
{
    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();

    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;
    [SerializeField] Color exploreColor = Color.yellow;
    [SerializeField] Color pathColor = new Color(1.0f,0.5f,0.0f);
    GridManager gridmanager;
    private void Awake()
    {
        gridmanager = FindObjectOfType<GridManager>();
        label = GetComponent<TextMeshPro>();
        label.enabled = false;
        DisplayCordinate();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCordinate();
            updateObjectName();
            label.enabled = true;
        }

        SetLabelColor();
        ToggleLabels();
    }
    void DisplayCordinate()
    {
        if (gridmanager == null) { return; }
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridmanager.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridmanager.UnityGridSize);
        label.text = coordinates.x + "," + coordinates.y;
    }

    void updateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }

    void SetLabelColor()
    {
        if (gridmanager == null) { return; }

        Node node = gridmanager.GetNode(coordinates);

        if (node == null) { return; }

        if (!node.isWalkable)
        {
            label.color = blockedColor;
        }
        else if (node.isPath)
        {
            label.color = pathColor;
        }
        else if(node.isExplored)
        {
            label.color = exploreColor;
        }
        else
        {
            label.color = defaultColor;
        }
    }

    void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            label.enabled = !label.IsActive();
        }
    }
}
