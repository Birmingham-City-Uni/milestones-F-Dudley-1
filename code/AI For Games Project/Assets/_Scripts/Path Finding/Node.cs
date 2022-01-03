using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node : IHeapItem<Node>
{
    [Header("World Variables")]
    public bool isWalkable;
    public Vector3 worldPosition;
    public int gridX, gridZ;
    public int movementPenalty;

    [Header("Path Finding Costs")]
    public Node parentNode;
    public List<Node> neighbours;
    public int gCost = 0;
    public int hCost = 0;

    private int heapIndex;

    public Node(bool _isWalkable, Vector3 _worldPosition, int _penalty, List<Node> _neighbours)
    {
        isWalkable = _isWalkable;
        worldPosition = _worldPosition;
        movementPenalty = _penalty;        
        neighbours = _neighbours;
    }

    public Node(bool _isWalkable, Vector3 _worldPosition, int _gridX, int _gridZ, int _penalty)
    {
        isWalkable = _isWalkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridZ = _gridZ;
        movementPenalty = _penalty;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node _NodeToCompare)
    {
        int compare = fCost.CompareTo(_NodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(_NodeToCompare.hCost);
        }

        return -compare;
    }
}