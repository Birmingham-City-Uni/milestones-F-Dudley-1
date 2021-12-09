using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathGraph : MonoBehaviour, NodeContainer
{
    [Header("Graph Attributes")]
    public Transform graphNodeContainer;

    [Space]

    public float maxNeighbourDistance = 10.0f;
    public float cliffSearchDistance = 1.5f;

    public List<Node> graph;

    [Header("Debug")]
    public bool showGraphGizmos;

    // Get Setters
    public int MaxSize
    {
        get { return graphNodeContainer.childCount; }
    }

    #region Unity Functions
    private void Start()
    {
        CreateContainer();
    }

    private void Update()
    {

    }

    private void OnDrawGizmos()
    {
        if (graph != null && showGraphGizmos)
        {
            foreach (Node node in graph)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(node.worldPosition, 0.25f);

                foreach (Node neighbour in node.neighbours)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(node.worldPosition, neighbour.worldPosition);
                }
            }
        }
    }
    #endregion

    #region Graph Functions

    public void CreateContainer()
    {
        int numNodes = graphNodeContainer.childCount;
        graph = new List<Node>(numNodes);

        foreach (Transform child in graphNodeContainer)
        {
            graph.Add(new Node(true, child.position, new List<Node>()));
            child.gameObject.SetActive(false);
        }

        foreach (Node node in graph)
        {
            node.neighbours = GetNodeNeighbours(node);
        }
    }

    public Node GetNodeFromWorldPoint(Vector3 _worldPosition)
    {
        Node closestNode = new Node(true, _worldPosition, null);
        float closestDistance = float.MaxValue;

        foreach (Node node in graph)
        {
            float distance = Vector3.Distance(_worldPosition, node.worldPosition);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNode = node;
            }
        }

        return closestNode;
    }

    public List<Node> GetNodeNeighbours(Node _node)
    {
        List<Node> neighbours = new List<Node>();

        foreach (Node nodeToCompare in graph)
        {
            if (_node == nodeToCompare) continue;
            float distance = Vector3.Distance(_node.worldPosition, nodeToCompare.worldPosition);

            if (distance < maxNeighbourDistance)
            {
                Vector3 edge = nodeToCompare.worldPosition - _node.worldPosition;

                bool cliff = false;
                int steps = Mathf.FloorToInt(edge.magnitude);
                for (int i = 0; i < steps; i++)
                {
                    Vector3 pos = _node.worldPosition + (edge.normalized * i);

                    RaycastHit hit;
                    if (Physics.Raycast(pos, Vector3.down, out hit, maxNeighbourDistance))
                    {
                        if (hit.distance > cliffSearchDistance) cliff = true;
                    }
                }

                if (!Physics.Raycast(_node.worldPosition, edge, maxNeighbourDistance) && !cliff)
                {
                    neighbours.Add(nodeToCompare);
                }
            }
        }

        return neighbours;
    }
    #endregion
}