using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface NodeContainer
{
    void CreateContainer();
    List<Node> GetNodeNeighbours(Node _node);

    Node GetNodeFromWorldPoint(Vector3 _worldPosition);
}