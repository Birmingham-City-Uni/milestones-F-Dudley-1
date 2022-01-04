using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node : IHeapItem<Node>
{
    [Header("World Variables")]

    /// <summary>
    /// Identifys If The Current Pathfinding Node is Walkable By Agents
    /// </summary>
    public bool isWalkable;

    /// <summary>
    /// The Position in World Where The Node is Located.
    /// </summary>
    public Vector3 worldPosition;

    /// <summary>
    /// The X and Z Index Position in a NodeGrid of the Node.
    /// </summary>
    public int gridX, gridZ;

    /// <summary>
    /// The Region Penalty For Movement For That Current Node.
    /// </summary>
    public int movementPenalty;

    [Header("Path Finding Costs")]

    /// <summary>
    /// The Current Assigned Parent Node While Pathfinding.
    /// </summary>
    public Node parentNode;

    /// <summary>
    /// The Nodes Current Neighbour Nodes.
    /// </summary>
    public List<Node> neighbours;

    /// <summary>
    /// The Current Cumulative Distance Cost of The Node From The Start Position.
    /// </summary>
    public int gCost = 0;

    /// <summary>
    /// The Distance of The Node To Target.
    /// </summary>
    public int hCost = 0;

    /// <summary>
    /// The Index of The Node in The Heap.
    /// </summary>
    private int heapIndex;

    /// <summary>
    /// The Nodes Constructor.
    /// </summary>
    /// <param name="_isWalkable">Assigns if The Node can Be Walked By Agents.</param>
    /// <param name="_worldPosition">Assigns if The Node can Be Walked By Agents.</param>
    /// <param name="_penalty">Assigns The Penalty of The Node From The Maps Region Cost.</param>
    /// <param name="_neighbours">Assigns The Nodes Currnt Neighbours.</param>
    public Node(bool _isWalkable, Vector3 _worldPosition, int _penalty, List<Node> _neighbours)
    {
        isWalkable = _isWalkable;
        worldPosition = _worldPosition;
        movementPenalty = _penalty;
        neighbours = _neighbours;
    }

    /// <summary>
    /// The Nodes Constructor.
    /// </summary>
    /// <param name="_isWalkable">Assigns if The Node can Be Walked By Agents.</param>
    /// <param name="_worldPosition">Assigns if The Node can Be Walked By Agents.</param>
    /// <param name="_gridX">The X Index Position of the Node In The NodeGrid.</param>
    /// <param name="_gridZ">The Z Index Position of the Node In The NodeGrid.</param>
    /// <param name="_penalty">Assigns The Penalty of The Node From The Maps Region Cost.</param>
    public Node(bool _isWalkable, Vector3 _worldPosition, int _gridX, int _gridZ, int _penalty)
    {
        isWalkable = _isWalkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridZ = _gridZ;
        movementPenalty = _penalty;
    }

    /// <summary>
    /// The Total Cost of The Current gCost and hCost.
    /// </summary>
    /// <value>The Added Value of The Nodes Current gCost and hCost.</value>
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    /// <summary>
    /// The Nodes Current Index in The Heap.
    /// </summary>
    /// <value>int value that holds the Nodes Current Index in the Heap.</value>
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

    /// <summary>
    /// Compares The Current Node With The Node Parameter.
    /// </summary>
    /// <param name="_NodeToCompare">The Node To Be Compared To The Current Node.</param>
    /// <returns>Returns The Reversed Comparison Value of The fCost. If fCost Comparision Equals 0 Defaults to hCost Comparison.</returns>
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