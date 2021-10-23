using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    [Header("World Variables")]
    public bool isWalkable;
    public Vector3 worldPosition;
    public int gridX, gridZ;

    [Header("Path Finding Costs")]
    public Node parentNode;    
    public int gCost;
    public int hCost;

    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    public Node(bool _isWalkable, Vector3 _worldPosition, int _gridX, int _gridZ)
    {
        isWalkable = _isWalkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridZ = _gridZ;
    }
}
