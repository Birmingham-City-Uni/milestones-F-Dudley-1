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

    [Header("Path Finding Costs")]
    public Node parentNode;
    public List<Node> neighbours;
    public int gCost;
    public int hCost;

    private int heapIndex;

    public Node(bool _isWalkable, Vector3 _worldPosition, List<Node> _neighbours)
    {
        isWalkable = _isWalkable;
        worldPosition = _worldPosition;
        neighbours = _neighbours;
    }

    public Node(bool _isWalkable, Vector3 _worldPosition, int _gridX, int _gridZ)
    {
        isWalkable = _isWalkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridZ = _gridZ;
    }

    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
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
