using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Different Types of Pathfinding That Can Be Chosen.
/// </summary>
public enum PathfindingAlgorithms
{
    AStar = 0,
    GreedyBestFirstSearch = 1,
    UniformCostSearch = 2,
    BreadthFirstSearch = 3,
    DepthFirstSearch = 4,
}

public class PathFinding : MonoBehaviour
{
    [Header("Main")]

    /// <summary>
    /// The Current Chosen Algorithm For Pathfinding.
    /// </summary>
    public static PathfindingAlgorithms currentChosenAlgorithm = PathfindingAlgorithms.AStar;

    [Header("References")]

    /// <summary>
    /// A Reference To The PathRequestManager Instance.
    /// </summary>
    public PathRequestManager pathManager;

    [Header("Debug")]

    /// <summary>
    /// The Container That Holds All The Debug Visuals GameObjects.
    /// </summary>
    private NodeContainer nodeContainer;

    /// <summary>
    /// Triggers if Pathing Debug Messages Should Be Shown In The Console.
    /// </summary>
    [SerializeField] private bool showDebugMessages = false;

    #region Unity Methods
    /// <summary>
    /// Unitys Awake Function.
    /// </summary>
    private void Awake()
    {
        pathManager = GetComponent<PathRequestManager>();
        nodeContainer = GetComponent<NodeContainer>();
    }

    /// <summary>
    /// Unitys Update Function.
    /// </summary>
    private void Update()
    {

    }
    #endregion

    #region Path Finding Methods

    /// <summary>
    /// Starts The Pathfinding Coroutine Depending on The Current Pathfinding Algorithm.
    /// </summary>
    /// <param name="_startPositon">The World Position Where To Start Pathfinding From.</param>
    /// <param name="_targetPosition">The Target, in World Coordinates, Where To Aim For in Pathfinding.</param>
    public void StartFindpath(Vector3 _startPositon, Vector3 _targetPosition)
    {
        switch (currentChosenAlgorithm)
        {
            default:
            case PathfindingAlgorithms.AStar:
                StartCoroutine(FindPath_AStar(_startPositon, _targetPosition));
                break;

            case PathfindingAlgorithms.UniformCostSearch:
                StartCoroutine(FindPath_UniformCostSearch(_startPositon, _targetPosition));
                break;

            case PathfindingAlgorithms.GreedyBestFirstSearch:
                StartCoroutine(FindPath_GreedyBestFirstSearch(_startPositon, _targetPosition));
                break;

            case PathfindingAlgorithms.BreadthFirstSearch:
                StartCoroutine(FindPath_BreadthFirstSearch(_startPositon, _targetPosition));
                break;

            case PathfindingAlgorithms.DepthFirstSearch:
                StartCoroutine(FindPath_DepthFirstSearch(_startPositon, _targetPosition));
                break;
        }
    }

    /// <summary>
    /// The Coroutine For Pathfinding, Performed Using a Weighted A-Star Algorithm.
    /// </summary>
    /// <param name="_startPosition">The World Position Where To Start Pathfinding From.</param>
    /// <param name="_targetPosition">The Target, in World Coordinates, Where To Aim For in Pathfinding.</param>
    /// <returns>Returns a Coroutine.</returns>
    public IEnumerator FindPath_AStar(Vector3 _startPosition, Vector3 _targetPosition)
    {
        // Initializing Needed Variables.
        Vector3[] pathWaypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startingNode = nodeContainer.GetNodeFromWorldPoint(_startPosition); // Getting Nodes From Container of
        Node targetNode = nodeContainer.GetNodeFromWorldPoint(_targetPosition);  // The Start and Target Position.

        if (startingNode.isWalkable && targetNode.isWalkable)
        {
            Stopwatch stopwatch = new Stopwatch(); // Starting A StopWatch For Debug Purposes.
            stopwatch.Start();

            Heap<Node> openSet = new Heap<Node>(nodeContainer.MaxSize); // Initializing The open and closed Set of Nodes.
            HashSet<Node> closeSet = new HashSet<Node>();               // Using a Heap and HashSet For Speed.

            openSet.Add(startingNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst(); // Removing The Node at The Start At The Heap With Lowest Costing.
                closeSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    stopwatch.Stop();
                    if (showDebugMessages) UnityEngine.Debug.Log("Pathfind Time Taken: " + stopwatch.ElapsedMilliseconds + "ms");

                    pathSuccess = true; // Setting The Pathing To a Success.
                    break;
                }

                foreach (Node neighbourNode in nodeContainer.GetNodeNeighbours(currentNode))
                {
                    if (!neighbourNode.isWalkable || closeSet.Contains(neighbourNode)) continue;

                    // Adjusting The Cost Values of the Node To Target, Factoring in the Regions Movement Penalty.
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

        // Traces Back Through The Created Path Through Parent Nodes, if A Path Was Found.
        if (pathSuccess) pathWaypoints = RetracePath(startingNode, targetNode);
        pathManager.FinishedProcessingPath(pathWaypoints, pathSuccess); // Finishes The Current Pathfinding Path in The Queue.
        yield return null;
    }

    /// <summary>
    /// The Coroutine For Pathfinding, Performed Using a Uniform Cost Search Algorithm.
    /// </summary>
    /// <param name="_startPosition">The World Position Where To Start Pathfinding From.</param>
    /// <param name="_targetPosition">The Target, in World Coordinates, Where To Aim For in Pathfinding.</param>
    /// <returns>Returns a Coroutine.</returns>
    ///
    public IEnumerator FindPath_UniformCostSearch(Vector3 _startPosition, Vector3 _targetPosition)
    {
        // Initializing Needed Variables.
        Vector3[] pathWaypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startingNode = nodeContainer.GetNodeFromWorldPoint(_startPosition); // Getting Nodes of The Starting and Target Postions
        Node targetNode = nodeContainer.GetNodeFromWorldPoint(_targetPosition);  // From The NodeContainer.

        if (startingNode.isWalkable && targetNode.isWalkable)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();      

            Heap<Node> openSet = new Heap<Node>(nodeContainer.MaxSize); // Creates a open and closed Set For Searched Nodes.
            HashSet<Node> closeSet = new HashSet<Node>();               // Using a Heap and HashSet For Speed. - HashSet Cannot Contain Duplicate Data.

            openSet.Add(startingNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst(); // Removes The First Node From The Heap With The Lowest Cumulative Cost.
                closeSet.Add(currentNode); // Adds The Node To The Closed Set So Doesn't Be Checked Again.

                if (currentNode == targetNode)
                {
                    stopwatch.Stop();
                    UnityEngine.Debug.Log("Pathfind Time Taken: " + stopwatch.ElapsedMilliseconds + "ms");

                    pathSuccess = true; // Returns True if a Path is Found.
                    break;
                }

                foreach (Node neighbourNode in nodeContainer.GetNodeNeighbours(currentNode))
                {
                    if (!neighbourNode.isWalkable || closeSet.Contains(neighbourNode)) continue;

                    // Adjusts The Cost Value of The Cumulative Distance Cost, Factoring in Regions Movement Penaltys.
                    int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbourNode) + neighbourNode.movementPenalty;
                    if (newCostToNeighbour < neighbourNode.gCost || !openSet.Contains(neighbourNode))
                    {
                        neighbourNode.gCost = newCostToNeighbour;

                        neighbourNode.parentNode = currentNode;

                        if (!openSet.Contains(neighbourNode))
                        {
                            openSet.Add(neighbourNode);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbourNode); // Updates The Node On The Heap With The Current Cumulative Costing Up To That Node.
                        }
                    }
                }
            }
        }

        // Traces Back Through The Created Path Through Parent Nodes, if A Path Was Found.
        if (pathSuccess) pathWaypoints = RetracePath(startingNode, targetNode);
        pathManager.FinishedProcessingPath(pathWaypoints, pathSuccess); // Finishes The Current Pathfinding Path in The Queue.
        yield return null;
    }

    /// <summary>
    /// The Coroutine For Pathfinding, Performed Using a Greedy Best First Search Algorithm.
    /// </summary>
    /// <param name="_startPosition">The World Position Where To Start Pathfinding From.</param>
    /// <param name="_targetPosition">The Target, in World Coordinates, Where To Aim For in Pathfinding.</param>
    /// <returns>Returns a Coroutine.</returns>
    public IEnumerator FindPath_GreedyBestFirstSearch(Vector3 _startPosition, Vector3 _targetPosition)
    {
        // Initializing Needed Variables.
        Vector3[] pathWaypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startingNode = nodeContainer.GetNodeFromWorldPoint(_startPosition);
        Node targetNode = nodeContainer.GetNodeFromWorldPoint(_targetPosition);

        if (startingNode.isWalkable && targetNode.isWalkable)
        {
            Stopwatch stopwatch = new Stopwatch(); // Starting A StopWatch For Debug Purposes.
            stopwatch.Start();

            Heap<Node> openSet = new Heap<Node>(nodeContainer.MaxSize); // Initializing The open and closed Set of Nodes
            HashSet<Node> closedSet = new HashSet<Node>();              // Using a Heap and HashSet For Speed.

            openSet.Add(startingNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst(); // Removing The Node at The Start At The Heap With Lowest Distance Costing.
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    stopwatch.Stop();
                    UnityEngine.Debug.Log("Pathfind Time Taken: " + stopwatch.ElapsedMilliseconds + "ms");

                    pathSuccess = true; // Setting The Pathing To a Success.
                    break;
                }

                foreach (Node neighbourNode in nodeContainer.GetNodeNeighbours(currentNode))
                {
                    if (!neighbourNode.isWalkable || closedSet.Contains(neighbourNode)) continue;

                    if (GetDistance(neighbourNode, targetNode) < neighbourNode.hCost || !openSet.Contains(neighbourNode))
                    {
                        // Adjusting The Cost Values of the Node To Target.
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

        // Traces Back Through The Created Path Through Parent Nodes, if A Path Was Found.
        if (pathSuccess) pathWaypoints = RetracePath(startingNode, targetNode);
        pathManager.FinishedProcessingPath(pathWaypoints, pathSuccess); // Finishes The Current Pathfinding Path in The Queue.
        yield return null;
    }

    /// <summary>
    /// The Coroutine For Pathfinding, Performed Using a Breadth First Search Algorithm.
    /// </summary>
    /// <param name="_startPosition">The World Position Where To Start Pathfinding From.</param>
    /// <param name="_targetPosition">The Target, in World Coordinates, Where To Aim For in Pathfinding.</param>
    /// <returns>Returns a Coroutine.</returns>
    public IEnumerator FindPath_BreadthFirstSearch(Vector3 _startPosition, Vector3 _targetPosition)
    {
        // Initializing Needed Variables.
        Vector3[] pathWaypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startingNode = nodeContainer.GetNodeFromWorldPoint(_startPosition);
        Node targetNode = nodeContainer.GetNodeFromWorldPoint(_targetPosition);

        if (startingNode.isWalkable && targetNode.isWalkable)
        {
            Stopwatch stopwatch = new Stopwatch(); // Starting A StopWatch For Debug Purposes.
            stopwatch.Start();

            Queue<Node> openSet = new Queue<Node>();        // Initializing The open and closed Set of Nodes.
            HashSet<Node> closedSet = new HashSet<Node>();  // Using a Queue For its FIFO Attributes, and HashSet For Speed (Cannot Contain Duplicate Data).

            openSet.Enqueue(startingNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.Dequeue(); // Removing The Node At The Start of The Queue, Which is Closest To The Start Node.
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    stopwatch.Stop();
                    UnityEngine.Debug.Log("Pathfind Time Taken: " + stopwatch.ElapsedMilliseconds + "ms");

                    pathSuccess = true; // Setting The Pathing To a Success.
                    break;
                }

                foreach (Node neighbour in nodeContainer.GetNodeNeighbours(currentNode))
                {
                    if (!neighbour.isWalkable || closedSet.Contains(neighbour)) continue;

                    // Adding The Neighbour Node To The Queue if it has not already been accessed.
                    if (!openSet.Contains(neighbour) && !closedSet.Contains(neighbour))
                    {
                        neighbour.parentNode = currentNode;
                        openSet.Enqueue(neighbour);
                    }
                }
            }
        }

        // Traces Back Through The Created Path Through Parent Nodes, if A Path Was Found.
        if (pathSuccess) pathWaypoints = RetracePath(startingNode, targetNode);
        pathManager.FinishedProcessingPath(pathWaypoints, pathSuccess); // Finishes The Current Pathfinding Path in The Queue.
        yield return null;
    }

    /// <summary>
    /// The Coroutine For Pathfinding, Performed Using a Depth First Search Algorithm.
    /// </summary>
    /// <param name="_startPosition">The World Position Where To Start Pathfinding From.</param>
    /// <param name="_targetPosition">The Target, in World Coordinates, Where To Aim For in Pathfinding.</param>
    /// <returns>Returns a Coroutine.</returns>
    public IEnumerator FindPath_DepthFirstSearch(Vector3 _startPosition, Vector3 _targetPosition)
    {
        // Initializing Needed Variables.
        Vector3[] pathWaypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startingNode = nodeContainer.GetNodeFromWorldPoint(_startPosition);
        Node targetNode = nodeContainer.GetNodeFromWorldPoint(_targetPosition);

        if (startingNode.isWalkable && targetNode.isWalkable)
        {
            Stopwatch stopwatch = new Stopwatch(); // Starting A StopWatch For Debug Purposes.
            stopwatch.Start();

            Stack<Node> openSet = new Stack<Node>();        // Initializing The open and closed Set of Nodes
            HashSet<Node> closedSet = new HashSet<Node>();  // Using a Stack For LIFO Attributes and HashSet For Speed (Cannot Contain Duplicate Elemetns).

            openSet.Push(startingNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.Pop(); // Removing The Node At The Top of The Stack, Which Has Just Been Put In.
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    stopwatch.Stop();
                    UnityEngine.Debug.Log("Pathfind Time Taken: " + stopwatch.ElapsedMilliseconds + "ms");

                    pathSuccess = true; // Setting The Pathing To a Success.
                    break;
                }

                foreach (Node neighbour in nodeContainer.GetNodeNeighbours(currentNode))
                {
                    if (!neighbour.isWalkable || closedSet.Contains(neighbour)) continue;

                    // Adding The Neighbour Node To The Stack if it has not already been accessed.
                    if (!openSet.Contains(neighbour) && !closedSet.Contains(neighbour))
                    {
                        neighbour.parentNode = currentNode;
                        openSet.Push(neighbour);
                    }
                }
            }
        }

        // Traces Back Through The Created Path Through Parent Nodes, if A Path Was Found.
        if (pathSuccess) pathWaypoints = RetracePath(startingNode, targetNode);
        pathManager.FinishedProcessingPath(pathWaypoints, pathSuccess); // Finishes The Current Pathfinding Path in The Queue.
        yield return null;
    }

    /// <summary>
    /// Retraces The Current Path From TargetNode Tor Starting Node, Through Parents.
    /// </summary>
    /// <param name="_startPosition">The World Position Where To Start Pathfinding From.</param>
    /// <param name="_targetPosition">The Target, in World Coordinates, Where To Aim For in Pathfinding.</param>
    /// <returns>An Array of The Pathfinding Nodes, Which Have Been Optimised For Movement.</returns>
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

    /// <summary>
    /// Gets The Distance of The Nodes From Each Other.
    /// </summary>
    /// <param name="_nodeA">The Starting Node To Get The Distance From.</param>
    /// <param name="_nodeB">The Target node To Get The Distance To.</param>
    /// <returns></returns>
    public int GetDistance(Node _nodeA, Node _nodeB)
    {
        int dstX = Mathf.Abs(_nodeA.gridX - _nodeB.gridX);
        int dstZ = Mathf.Abs(_nodeA.gridZ - _nodeB.gridZ);

        if (dstX > dstZ) return 14 * dstZ + 10 * (dstX - dstZ);
        else return 14 * dstX + 10 * (dstZ - dstX);
    }
    #endregion

    #region Path Optimisation

    /// <summary>
    /// Optimized The Path By Removing and Positions That Are In Similar Directions And Don't Collide With Objects.
    /// </summary>
    /// <param name="_pathToOptimise">The Path Of Nodes To Be Optimized.</param>
    /// <returns>An Array of Optimized Positions For Pathing, That Minimise Any Repeats In Direction and Collisions.</returns>
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

            bool collisionCheck = Physics.Linecast(_pathToOptimise[i - 1].worldPosition, _pathToOptimise[i].worldPosition, -1, QueryTriggerInteraction.Ignore);

            if (directionNew != directionOld || collisionCheck || i == _pathToOptimise.Count - 1)
            {
                waypoints.Add(_pathToOptimise[i].worldPosition);

                directionOld = directionNew;
            }
        }

        return waypoints.ToArray();
    }
    #endregion
}