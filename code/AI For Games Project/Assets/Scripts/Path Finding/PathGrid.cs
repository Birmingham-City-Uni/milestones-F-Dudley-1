using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGrid : MonoBehaviour
{
    [Header("Grid Attributes")]
    public Vector3 gridPositionOffset;
    public Vector3 gridWorldSize;
    public float nodeDiameter;    
    public LayerMask unwalkableTerrainMask;

    private Node[,] grid;
    private int gridSizeX, gridSizeZ;
    
    [Header("Debug")]
    public bool showPathOnly;

    #region Unity Functions
    private void Awake()
    {
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
        CreateGrid();
    }

    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube((transform.position + gridPositionOffset) + Vector3.up * (gridWorldSize.y/2), gridWorldSize);

        if(grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = node.isWalkable ? Color.gray : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter-0.1f));
            }
        }
    }
    #endregion

    #region Grid Functions
    private void CreateGrid()
    {
        float nodeRadius = nodeDiameter / 2;
        grid = new Node[gridSizeX, gridSizeZ];
        Vector3 worldBottomLeft = (transform.position + gridPositionOffset) - (Vector3.right * (gridSizeX/2)) - (Vector3.forward * (gridSizeZ/2)) + Vector3.up * (gridWorldSize.y/2);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 pointInWorld = worldBottomLeft + (Vector3.right * (x * nodeDiameter + nodeRadius)) + (Vector3.forward * (z * nodeDiameter + nodeRadius));
                bool isWalkable = !(Physics.CheckBox(pointInWorld, Vector3.one * nodeRadius, Quaternion.identity, unwalkableTerrainMask, QueryTriggerInteraction.Ignore));
                grid[x,z] = new Node(isWalkable, pointInWorld);
            }
        }
    }

    public Node GetNodeFromWorldPoint(Vector3 _worldPosition)
    {
        float percentX = Mathf.Clamp01((_worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x);
        float percentZ = Mathf.Clamp01((_worldPosition.z + gridWorldSize.z/2) / gridWorldSize.z);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);
        
        return grid[x,z];
    }
    #endregion
}