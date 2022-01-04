using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathGraph : MonoBehaviour, NodeContainer
{
    [Header("Graph Attributes")]
    /// <summary>
    /// The Container To Check For Placed Node Objects.
    /// </summary>
    public Transform graphNodeContainer;

    [Space]

    /// <summary>
    /// The Max Distance To Check For Neighbouring Nodes.
    /// </summary>
    public float maxNeighbourDistance = 10.0f;

    /// <summary>
    /// The Distance At Which To Check Downwards For Cliff Detection.
    /// </summary>
    public float cliffSearchDistance = 1.5f;

    /// <summary>
    /// The List of Nodes in The PathGraph.
    /// </summary>
    public List<Node> graph;

    [Header("Debug")]

    /// <summary>
    /// Detemines If Debug Visuals Would Be Shown For The PathGraph.
    /// </summary>
    public bool showGraphGizmos;

    public int MaxSize
    {
        get { return graphNodeContainer.childCount; }
    }

    #region Unity Functions

    /// <summary>
    /// Unitys Start Function.
    /// </summary>
    private void Start()
    {
        CreateContainer();
    }

    /// <summary>
    /// Unitys Update Function.
    /// </summary>
    private void Update()
    {

    }

    /// <summary>
    /// Unitys OnDrawGizmos Function.
    /// </summary>
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

        foreach (Transform child in graphNodeContainer) // Iterates Through all Nodes GameObjects And Adds Them To The List.
        {
            graph.Add(new Node(true, child.position, 0, new List<Node>()));
            child.gameObject.SetActive(false);
        }

        foreach (Node node in graph) // Assigns All The Neighbour Nodes of The Nodes.
        {
            node.neighbours = GetNodeNeighbours(node);
        }
    }

    public void CreateDebugVisuals()
    {

    }

    public Node GetNodeFromWorldPoint(Vector3 _worldPosition)
    {
        Node closestNode = new Node(true, _worldPosition, 0, null);
        float closestDistance = float.MaxValue;

        foreach (Node node in graph)
        {
            float distance = Vector3.Distance(_worldPosition, node.worldPosition); // Checks The Distance To The Closes Node Position.
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
            float distance = Vector3.Distance(_node.worldPosition, nodeToCompare.worldPosition); // Checks The Distance Of The Node From The Current Node.

            if (distance < maxNeighbourDistance)
            {
                Vector3 edge = nodeToCompare.worldPosition - _node.worldPosition; // Vector To Iterate Across To Check For Edges.

                bool cliff = false;
                int steps = Mathf.FloorToInt(edge.magnitude);
                for (int i = 0; i < steps; i++) // Steps Across The Direction of The Other Node, and Checks For Cliffs.
                {
                    Vector3 pos = _node.worldPosition + (edge.normalized * i);

                    RaycastHit hit;
                    if (Physics.Raycast(pos, Vector3.down, out hit, maxNeighbourDistance))
                    {
                        if (hit.distance > cliffSearchDistance) cliff = true;
                    }
                }

                // Final Check For Collisions In The Direction and Adds it To The List if No Cliff Was Found.
                if (!Physics.Raycast(_node.worldPosition, edge, maxNeighbourDistance) && !cliff)
                {
                    neighbours.Add(nodeToCompare);
                }
            }
        }

        return neighbours;
    }

    public void EnablePathingVisuals(bool isEnabled)
    {

    }
    #endregion
}