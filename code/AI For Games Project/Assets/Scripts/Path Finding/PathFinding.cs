<<<<<<< HEAD
﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PathingTechniques
{
	ASTAR,
	BFS,
	DFS,

}

public class PathFinding : MonoBehaviour {

	public PathingTechniques currentPathingTechnique = PathingTechniques.ASTAR;

	private NodeContainer nodeContainer;
	private PathRequestManager requestManager;

	void Awake() {
		nodeContainer = GetComponent<NodeContainer> ();
		requestManager = GetComponent<PathRequestManager>();
	}

	public void StartFindPath(Vector3 startPosition, Vector3 endPosition)
	{
		switch (currentPathingTechnique)
		{
			default:
			case PathingTechniques.ASTAR:
				StartCoroutine(FindPathAStar(startPosition, endPosition));		
				break;

			case PathingTechniques.BFS:
				StartCoroutine(FindPathBFS(startPosition, endPosition));
				break;

			case PathingTechniques.DFS:
				StartCoroutine(FindPathDFS(startPosition, endPosition));
				break;
		}	
	}

	IEnumerator FindPathAStar(Vector3 startPos, Vector3 targetPos) {
		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		Node startNode = nodeContainer.GetNodeFromWorldPoint(startPos);
		Node targetNode = nodeContainer.GetNodeFromWorldPoint(targetPos);

		if (startNode.isWalkable && targetNode.isWalkable)
		{
			Heap<Node> openSet = new Heap<Node>(nodeContainer.MaxSize);
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				if (currentNode == targetNode)
				{
					pathSuccess = true;
					break;
				}

				foreach (Node neighbour in nodeContainer.GetNodeNeighbours(currentNode))
				{
					if (!neighbour.isWalkable || closedSet.Contains(neighbour))
					{
						continue;
					}

					int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
					if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parentNode = currentNode;

						if (!openSet.Contains(neighbour))
						{
							openSet.Add(neighbour);							
						}
					}
				}					
			}			
		}

		if (pathSuccess) 
		{
			waypoints = RetracePath(startNode, targetNode);
		}
		requestManager.FinishedProcessingPath(waypoints, pathSuccess);

		yield return null;
	}

	IEnumerator FindPathBFS(Vector3 startPos, Vector3 targetPos) 
	{
		Node startNode = nodeContainer.GetNodeFromWorldPoint(startPos);
		Node targetNode = nodeContainer.GetNodeFromWorldPoint(targetPos);

		Queue<Node> frontier = new Queue<Node>();
		List<Node> explored = new List<Node>();

		frontier.Enqueue(startNode);

		while (frontier.Count > 0)
		{
			Node leafNode = frontier.Dequeue();
			explored.Add(leafNode);

			if (leafNode == targetNode) RetracePath(startNode, targetNode);

			foreach (Node neighbour in leafNode.neighbours)
			{
				if (!neighbour.isWalkable || explored.Contains(neighbour)) continue;

				if (!frontier.Contains(neighbour)) 
				{
					neighbour.parentNode = leafNode;
					frontier.Enqueue(neighbour);
				}
			}
		}

		yield return null;
	}

	IEnumerator FindPathDFS(Vector3 startPos, Vector3 targetPos) 
	{
		Node startNode = nodeContainer.GetNodeFromWorldPoint(startPos);
		Node targetNode = nodeContainer.GetNodeFromWorldPoint(targetPos);

		Stack<Node> frontier = new Stack<Node>();
		List<Node> explored = new List<Node>();

		frontier.Push(startNode);

		while (frontier.Count > 0)
		{
			Node leafNode = frontier.Pop();
			explored.Add(leafNode);

			if (leafNode == targetNode) RetracePath(startNode, targetNode);

			foreach (Node neighbour in leafNode.neighbours)
			{
				if (!neighbour.isWalkable || explored.Contains(neighbour)) continue;

				if (!frontier.Contains(neighbour)) 
				{
					neighbour.parentNode = leafNode;
					frontier.Push(neighbour);
				}
			}
		}

		yield return null;
	}

	Vector3[] RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parentNode;
		}
		Vector3[] waypoints = OptimizePath(path);
		Array.Reverse(waypoints);

		return waypoints;
	}

	Vector3[] OptimizePath(List<Node> path)
	{
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 oldDirection = Vector2.zero;

		for (int i = 1; i < path.Count; i++)
		{
			Vector2 newDirection = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridZ - path[i].gridZ);

			if (newDirection != oldDirection)
			{
				waypoints.Add(path[i].worldPosition);
			}
			oldDirection = newDirection;
		}

		return waypoints.ToArray();
	}

	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}
=======
using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [Header("References")]
    public PathFindingManager pathManager;

    [Header("Debug")]
    private PathGrid grid;

    #region Unity Methods
    private void Awake()
    {
        pathManager = GetComponent<PathFindingManager>();
        grid = GetComponent<PathGrid>();
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

        if (startingNode.isWalkable && targetNode.isWalkable)
        {
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

                if (currentNode == targetNode)
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
                            openSet.Add(neighbourNode);
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
>>>>>>> main
}
