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

    private GameObject[] graph;

    /// /// ///
    private List<Vector3> links;
    private List<Vector3> from_pos;   // CAN BE REWORKED LATER TO USE NODE CLASS.
    /// /// ///
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
        if (graph != null && links != null && from_pos != null && showGraphGizmos)
        {
            for (int i = 0; i < links.Count; i++)
            {
                Debug.DrawLine(from_pos[i], from_pos[i] + links[i], Color.cyan);
            }
        }
    }
    #endregion

    #region Graph Functions
    public void CreateGraph()
    {
        int numNodes = graphNodeContainer.childCount;
        graph = new GameObject[numNodes];

        links = new List<Vector3>();
        from_pos = new List<Vector3>();        

        for (int i = 0; i < numNodes; i++)
        {
            graph[i] = graphNodeContainer.GetChild(i).gameObject;
        }

        foreach (GameObject node in graph)
        {
            foreach (GameObject nodeToCompare in graph)
            {
                if (node == nodeToCompare) continue;
                float distance = Vector3.Distance(node.transform.position, nodeToCompare.transform.position);

                if (distance < maxNeighbourDistance)
                {
                    Vector3 edge = nodeToCompare.transform.position - node.transform.position;

                    bool cliff = false;
                    int steps = Mathf.FloorToInt(edge.magnitude);
                    for (int i = 0; i < steps; i++)
                    {
                        Vector3 pos = node.transform.position + (edge.normalized * i);

                        RaycastHit hit;
                        if (Physics.Raycast(pos, Vector3.down, out hit, maxNeighbourDistance))
                        {
                            if (hit.distance > cliffSearchDistance) cliff = true;
                        }
                    }

                    if (!Physics.Raycast(node.transform.position, edge, maxNeighbourDistance) && !cliff)
                    {
                        links.Add(edge);
                        from_pos.Add(node.transform.position);
                    }
                }
            }
        }
    }
    #endregion
}
