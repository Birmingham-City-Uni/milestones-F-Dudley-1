using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathfindingAlgorithms
{
    ASTAR,
    BFS,
    DFS,
}

public class PathFinding : MonoBehaviour
{
    [Header("Main")]
    public PathfindingAlgorithms currentChosenAlgorithm;

    [Header("References")]
    public PathRequestManager pathManager;

    [Header("Debug")]
    private NodeContainer nodeContainer;

    #region Unity Methods
    private void Awake()
    {
        pathManager = GetComponent<PathRequestManager>();
        nodeContainer = GetComponent<NodeContainer>();
    }

    private void Update()
    {

    }
    #endregion

    #region Path Finding Methods
    public void StartFindpath(Vector3 _startPositon, Vector3 _targetPosition)
    {
        switch (currentChosenAlgorithm)
        {
            
            default:
            case PathfindingAlgorithms.ASTAR:
                StartCoroutine(FindPath_AStar(_startPositon, _targetPosition));
                break;

            case PathfindingAlgorithms.BFS:
                StartCoroutine(FindPath_BFS(_startPositon, _targetPosition));
                break;

            case PathfindingAlgorithms.DFS:
                StartCoroutine(FindPath_DFS(_startPositon, _targetPosition));
                break;
        }
        
    }

    public IEnumerator FindPath_AStar(Vector3 _startPosition, Vector3 _targetPosition)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Vector3[] pathWaypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startingNode = nodeContainer.GetNodeFromWorldPoint(_startPosition);
        Node targetNode = nodeContainer.GetNodeFromWorldPoint(_targetPosition);

        if (startingNode.isWalkable && targetNode.isWalkable)
        {
            Heap<Node> openSet = new Heap<Node>(nodeContainer.MaxSize);
            HashSet<Node> closeSet = new HashSet<Node>();

            openSet.Add(startingNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closeSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    stopwatch.Stop();
                    UnityEngine.Debug.Log("Pathfind Time Taken: " + stopwatch.ElapsedMilliseconds + "ms");

                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbourNode in nodeContainer.GetNodeNeighbours(currentNode))
                {
                    if (!neighbourNode.isWalkable || closeSet.Contains(neighbourNode)) continue;

                    int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbourNode) + neighbourNode.movementPenalty;
                    if (newCostToNeighbour < neighbourNode.gCost || !openSet.Contains(neighbourNode))
                    {
                        neighbourNode.gCost = newCostToNeighbour;
                        neighbourNode.hCost = GetDistance(neighbourNode, targetNode);

                        neighbourNode.parentNode = currentNode;

                        if (!openSet.Contains(neighbourNode))
                        {
                            openSet.Add(neighbourNode);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbourNode);
                        }
                    }
                }
            }
        }

        yield return null;
        if (pathSuccess) pathWaypoints = RetracePath(startingNode, targetNode);
        pathManager.FinishedProcessingPath(pathWaypoints, pathSuccess);
    }

    public IEnumerator FindPath_BFS(Vector3 _startPosition, Vector3 _targetPosition)
    {
        Vector3[] pathWaypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startingNode = nodeContainer.GetNodeFromWorldPoint(_startPosition);
        Node targetNode = nodeContainer.GetNodeFromWorldPoint(_targetPosition);

        if (startingNode.isWalkable && targetNode.isWalkable)
        {
            /*
            while (true)
            {
                if (currentNode == targetNode)
                {
                    stopwatch.Stop();
                    UnityEngine.Debug.Log("Pathfind Time Taken: " + stopwatch.ElapsedMilliseconds + "ms");

                    pathSuccess = true;
                    break;
                }
            }
            */
        }

        yield return null;
        if (pathSuccess) pathWaypoints = RetracePath(startingNode, targetNode);
        pathManager.FinishedProcessingPath(pathWaypoints, pathSuccess);
    }

    public IEnumerator FindPath_DFS(Vector3 _startPosition, Vector3 _targetPosition)
    {
        Vector3[] pathWaypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startingNode = nodeContainer.GetNodeFromWorldPoint(_startPosition);
        Node targetNode = nodeContainer.GetNodeFromWorldPoint(_targetPosition);

        if (startingNode.isWalkable && targetNode.isWalkable)
        {
            /*
            while (true)
            {
                if (currentNode == targetNode)
                {
                    stopwatch.Stop();
                    UnityEngine.Debug.Log("Pathfind Time Taken: " + stopwatch.ElapsedMilliseconds + "ms");

                    pathSuccess = true;
                    break;
                }
            }
            */
        }

        yield return null;
        if (pathSuccess) pathWaypoints = RetracePath(startingNode, targetNode);
        pathManager.FinishedProcessingPath(pathWaypoints, pathSuccess);
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
                _pathToOptimise[i - 1].gridX - _pathToOptimise[i].gridX,
                _pathToOptimise[i - 1].gridZ - _pathToOptimise[i].gridZ
            );

            if (directionNew != directionOld)
            {
                waypoints.Add(_pathToOptimise[i].worldPosition);

                directionOld = directionNew;
            }
        }

        return waypoints.ToArray();
    }
    #endregion
}