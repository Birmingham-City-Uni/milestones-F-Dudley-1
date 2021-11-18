using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGraph : MonoBehaviour
{
    [Header("Graph Attributes")]
    public Transform graphNodeContainer;

    [Space]

    public float maxNeighbourDistance = 10.0f;
    public float cliffSearchDistance = 1.5f;

    private List<Node> graph;

    [Header("Debug")]
    public bool showGraphGizmos;

    #region Unity Functions
    private void Start()
    {
        CreateGraph();
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
    public void CreateGraph()
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
            foreach (Node nodeToCompare in graph)
            {
                if (node == nodeToCompare) continue;
                float distance = Vector3.Distance(node.worldPosition, nodeToCompare.worldPosition);

                if (distance < maxNeighbourDistance)
                {
                    Vector3 edge = nodeToCompare.worldPosition - node.worldPosition;

                    bool cliff = false;
                    int steps = Mathf.FloorToInt(edge.magnitude);
                    for (int i = 0; i < steps; i++)
                    {
                        Vector3 pos = node.worldPosition + (edge.normalized * i);

                        RaycastHit hit;
                        if (Physics.Raycast(pos, Vector3.down, out hit, maxNeighbourDistance))
                        {
                            if (hit.distance > cliffSearchDistance) cliff = true;
                        }
                    }

                    if (!Physics.Raycast(node.worldPosition, edge, maxNeighbourDistance) && !cliff)
                    {
                        node.neighbours.Add(nodeToCompare);
                    }
                }
            }
        }
    }
    #endregion
}