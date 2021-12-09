using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathGrid : MonoBehaviour, NodeContainer
{
    [Header("Grid Attributes")]
    public Vector3 gridPositionOffset;
    public Vector3 gridWorldSize;
    public float nodeDiameter;
    public LayerMask unwalkableTerrainMask;

    public int MaxSize
    {
        get {
            return gridSizeX * gridSizeZ;
        }
    }

    public Node[,] grid;
    private int gridSizeX, gridSizeZ;

    [Header("Debug")]
    public bool showGridGizmos;

    #region Unity Functions
    private void Awake()
    {
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
        CreateContainer();
    }

    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube((transform.position + gridPositionOffset) + Vector3.up * (gridWorldSize.y/2), gridWorldSize);

        if(grid != null && showGridGizmos)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = node.isWalkable ? Color.gray : Color.red;                        
                Gizmos.DrawWireCube(node.worldPosition, Vector3.one * (nodeDiameter-0.1f));                    
            }
        }
    }
    #endregion

    #region Grid Functions
    public void CreateContainer()
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
                grid[x,z] = new Node(isWalkable, pointInWorld, x, z);

                grid[x,z].neighbours = GetNodeNeighbours(grid[x,z]);
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

    public List<Node> GetNodeNeighbours(Node _node)
    {
        List<Node> neighbourNodes = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0) continue;

                int checkX = _node.gridX + x;
                int checkZ = _node.gridZ + z;

                if (checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ)
                {
                    neighbourNodes.Add(grid[checkX, checkZ]);
                }
            }
        }

        return neighbourNodes;
    }
    #endregion
}
