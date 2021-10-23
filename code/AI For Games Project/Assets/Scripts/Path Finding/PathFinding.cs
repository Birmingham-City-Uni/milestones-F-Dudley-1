using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{

    [Header("Debug")]
    public Transform startPosition;
    public Transform targetPosition;

    private PathGrid grid;

    #region Unity Methods
    private void Awake()
    {
        grid = GetComponent<PathGrid>();
    }

    private void Update()
    {
        FindPath(startPosition.position, targetPosition.position);
    }
    #endregion

    #region Path Finding Methods
    public void FindPath(Vector3 _startPosition, Vector3 _targetPosition)
    {
        Node startingNode = grid.GetNodeFromWorldPoint(_startPosition);
        Node targetNode = grid.GetNodeFromWorldPoint(_targetPosition);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closeSet = new HashSet<Node>();

        openSet.Add(startingNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closeSet.Add(currentNode);

            if(currentNode == targetNode)
            {
                RetracePath(startingNode, targetNode);
                return;
            }

            foreach (Node neighbourNode in grid.GetNodeNeighbours(currentNode))
            {
                if (!neighbourNode.isWalkable || closeSet.Contains(neighbourNode)) continue;

                int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbourNode);
                if (newCostToNeighbour < neighbourNode.gCost || !openSet.Contains(neighbourNode))
                {
                    neighbourNode.gCost = newCostToNeighbour;
                    neighbourNode.hCost = GetDistance(neighbourNode, targetNode);

                    neighbourNode.parentNode = currentNode;

                    if (!openSet.Contains(neighbourNode))
                    {
                        openSet.Add(neighbourNode);
                    }
                }
            }
        }
    }

    public void RetracePath(Node _startNode, Node _targetNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = _targetNode;

        while (currentNode != _startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
        path.Reverse();

        grid.currentPath = path;
    }

    public int GetDistance(Node _nodeA, Node _nodeB)
    {
        int dstX = Mathf.Abs(_nodeA.gridX - _nodeB.gridX);
        int dstZ = Mathf.Abs(_nodeA.gridZ - _nodeB.gridZ);

        if (dstX > dstZ) return 14 * dstZ + 10 * (dstX - dstZ);
        else return 14 * dstX + 10 * (dstZ - dstX);
    }
    #endregion
}
