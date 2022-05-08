using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Interface For any Possible Node Containers For Pathfinding.
/// </summary>
interface NodeContainer
{
    /// <summary>
    /// The Max Size of The Node Container.
    /// </summary>
    /// <value>An Int of The Containers Max Size.</value>
    int MaxSize
    {
        get;
    }

    /// <summary>
    /// Creates The Node Containers Contents.
    /// </summary>
    void CreateContainer();

    /// <summary>
    /// Creates any Debug Assets For Visualizing the Debug Visualizer.
    /// </summary>
    void CreateDebugVisuals();

    /// <summary>
    /// Gets The Nodes Current Neighbours.
    /// </summary>
    /// <param name="_node">The Node Thats Neighbours are Needed.</param>
    /// <returns>Returns as List of Neighbours of The Current Node.</returns>
    List<Node> GetNodeNeighbours(Node _node);

    /// <summary>
    /// Gets A Node From an Inputted World Position.
    /// </summary>
    /// <param name="_worldPosition">The World Position To Find a Node From.</param>
    /// <returns></returns>
    Node GetNodeFromWorldPoint(Vector3 _worldPosition);

    /// <summary>
    /// Enables The Pathfinding Debug Visuals Depending On The Parameter.
    /// </summary>
    /// <param name="isEnabled">The Desired Visability of The Debug Visuals. True if Rendered, False if Hidden.</param>
    void EnablePathingVisuals(bool isEnabled);
}