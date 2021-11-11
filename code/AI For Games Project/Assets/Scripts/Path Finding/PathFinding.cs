using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnvironmentStyle
{
    Grid,
    Graph
}

public class PathFinding : MonoBehaviour
{
    [Header("References")]
    public EnvironmentStyle environmentLayout;
    public PathFindingManager pathManager;

    [Header("Debug")]
    private PathGrid grid;
    private PathGraph graph;

    #region Unity Methods
    private void Awake()
    {
        pathManager = GetComponent<PathFindingManager>();
        grid = GetComponent<PathGrid>();
        graph = GetComponent<PathGraph>();

        if (grid != null) environmentLayout = EnvironmentStyle.Grid;
        else if (graph != null) environmentLayout = EnvironmentStyle.Graph;
    }

    private void Update()
    {

    }
    #endregion

    #region Path Finding Methods
    public void StartFindpath(Vector3 _startPositon, Vector3 _targetPosition)
    {
        StartCoroutine(FindPath(_startPositon, _targetPosition));
    }

    public IEnumerator FindPath(Vector3 _startPosition, Vector3 _targetPosition)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Vector3[] pathWaypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startingNode = grid.GetNodeFromWorldPoint(_startPosition);
        Node targetNode = grid.GetNodeFromWorldPoint(_targetPosition);

        if (startingNode.isWalkable && targetNode.isWalkable) {
            PriorityQueue<Node> openSet = new PriorityQueue<Node>(grid.MaxSize);
            HashSet<Node> closeSet = new HashSet<Node>();

            openSet.Enqueue(startingNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.Dequeue();
                closeSet.Add(currentNode);

                if(currentNode == targetNode)
                {
                    stopwatch.Stop();
                    UnityEngine.Debug.Log("Pathfind Time Taken: " + stopwatch.ElapsedMilliseconds + "ms");

                    pathSuccess = true;
                    break;
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
                            openSet.Enqueue(neighbourNode);
                        }
                    }
                }
            }            
        }

        yield return null;
        if (pathSuccess) pathWaypoints = RetracePath(startingNode, targetNode);
        pathManager.FinishedPathProcessing(pathWaypoints, pathSuccess);
    }

    public Vector3[] RetracePath(Node _startNode, Node _targetNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = _targetNode;

        while (currentNode != _startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
        Vector3[] waypoints = OptimisePath(path);
        Array.Reverse(waypoints);

        return waypoints;
    }

    public int GetDistance(Node _nodeA, Node _nodeB)
    {
        int dstX = Mathf.Abs(_nodeA.gridX - _nodeB.gridX);
        int dstZ = Mathf.Abs(_nodeA.gridZ - _nodeB.gridZ);

        if (dstX > dstZ) return 14 * dstZ + 10 * (dstX - dstZ);
        else return 14 * dstX + 10 * (dstZ - dstX);
    }
    #endregion

    #region Path Optimisation
    public Vector3[] OptimisePath(List<Node> _pathToOptimise)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < _pathToOptimise.Count; i++)
        {
            Vector2 directionNew = new Vector2(
                _pathToOptimise[i-1].gridX - _pathToOptimise[i].gridX,
                _pathToOptimise[i-1].gridZ - _pathToOptimise[i].gridZ
            );

            if (directionNew != directionOld) {
                waypoints.Add(_pathToOptimise[i].worldPosition);

                directionOld = directionNew;
            }
        }

        return waypoints.ToArray();
    }
    #endregion
}
