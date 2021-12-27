using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface NodeContainer
{
    int MaxSize
    {
        get;
    }

    void CreateContainer();
    void CreateDebugVisuals();

    List<Node> GetNodeNeighbours(Node _node);
    Node GetNodeFromWorldPoint(Vector3 _worldPosition);

    void EnablePathingVisuals(bool isEnabled);
}