using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainMaskType
{
    public LayerMask maskName;
    public int maskPenalty;
}

[System.Serializable]
public class PathGrid : MonoBehaviour, NodeContainer
{
    [Header("Grid Attributes")]
    public Vector3 gridPositionOffset;
    public Vector3 gridWorldSize;
    public float nodeDiameter;

    [Tooltip("This Gets Assigned at Runtime.")]
    [SerializeField] private LayerMask walkableTerrainMask;
    [SerializeField] private LayerMask unwalkableTerrainMask;
    public TerrainMaskType[] terrainRegions;

    private int penaltyMin = int.MaxValue;
    private int penaltyMax = int.MinValue;

    private Dictionary<int, int> terrainRegionsLog = new Dictionary<int, int>();

    [Header("Debug Referernces")]
    private GameObject pathingVisuals;
    public Material walkableMat;
    public Material nonWalkableMat;

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeZ;
        }
    }

    public Node[,] grid;
    private int gridSizeX, gridSizeZ;

    #region Unity Functions
    private void Awake()
    {
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);

        foreach (TerrainMaskType type in terrainRegions)
        {
            walkableTerrainMask |= type.maskName;
            terrainRegionsLog.Add((int) Mathf.Log(type.maskName.value, 2), type.maskPenalty);
        }

        CreateContainer();
        CreateDebugVisuals();
    }

    private void OnEnable()
    {
        GameManager.enableNodeContainerDrawing += EnablePathingVisuals;
    }

    private void OnDisable()
    {
        GameManager.enableNodeContainerDrawing -= EnablePathingVisuals;
    }

    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube((transform.position + gridPositionOffset) + Vector3.up * (gridWorldSize.y / 2), gridWorldSize);

        if (grid != null && GameManager.instance.DrawNodeContainer)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = node.isWalkable ? Color.gray : Color.red;
                Gizmos.DrawWireCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
    #endregion

    #region Grid Functions
    public void CreateContainer()
    {
        float nodeRadius = nodeDiameter / 2;
        grid = new Node[gridSizeX, gridSizeZ];
        Vector3 worldBottomLeft = (transform.position + gridPositionOffset) - (Vector3.right * (gridSizeX / 2)) - (Vector3.forward * (gridSizeZ / 2)) + Vector3.up * (gridWorldSize.y / 2);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 pointInWorld = worldBottomLeft + (Vector3.right * (x * nodeDiameter + nodeRadius)) + (Vector3.forward * (z * nodeDiameter + nodeRadius));
                bool isWalkable = !(Physics.CheckBox(pointInWorld, Vector3.one * nodeRadius, Quaternion.identity, unwalkableTerrainMask, QueryTriggerInteraction.Ignore));
                
                int movementPenalty = 0;
                if (isWalkable)
                {
                    Ray ray = new Ray(pointInWorld + Vector3.up * 50, Vector3.down);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100, walkableTerrainMask, QueryTriggerInteraction.Collide))
                    {
                        terrainRegionsLog.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                    }
                }

                grid[x, z] = new Node(isWalkable, pointInWorld, x, z, movementPenalty);
            }
        }

        PenaltyMap(2);
    }

    public void CreateDebugVisuals()
    {
        pathingVisuals = new GameObject("DebugVisualsContainer");
        pathingVisuals.transform.parent = transform;
        pathingVisuals.SetActive(false);        

        Vector3 walkScale = new Vector3(0.2f, 0.2f, 0.2f);
        Vector3 nonWalkScale = new Vector3(0.8f, 0.8f, 0.8f);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                GameObject nodeVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Renderer renderer = nodeVisual.GetComponent<Renderer>();
                Destroy(nodeVisual.GetComponent<Collider>());
                nodeVisual.transform.parent = pathingVisuals.transform;
                nodeVisual.name = string.Format("Node-{0}-{1}", x, z);

                if (grid[x, z].isWalkable)
                {
                    nodeVisual.transform.localScale = walkScale;
                    renderer.material = walkableMat;
                }
                else
                {
                    nodeVisual.transform.localScale = nonWalkScale;
                    renderer.material = nonWalkableMat;
                }

                nodeVisual.transform.position = grid[x, z].worldPosition;
            }
        }
    }

    private void PenaltyMap(int _blurSize)
    {
        int kernelSize = _blurSize * 2 + 1;
        int kernelExtents = (kernelSize-1) / 2;

        int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeZ];
        int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeZ];

		for (int z = 0; z < gridSizeZ; z++) {
			for (int x = -kernelExtents; x <= kernelExtents; x++) {
				int sampleX = Mathf.Clamp (x, 0, kernelExtents);
				penaltiesHorizontalPass [0, z] += grid [sampleX, z].movementPenalty;
			}

			for (int x = 1; x < gridSizeX; x++) {
				int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
				int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);

				penaltiesHorizontalPass [x, z] = penaltiesHorizontalPass [x - 1, z] - grid [removeIndex, z].movementPenalty + grid [addIndex, z].movementPenalty;
			}
		}
			
		for (int x = 0; x < gridSizeX; x++) {
			for (int z = -kernelExtents; z <= kernelExtents; z++) {
				int sampleZ = Mathf.Clamp (z, 0, kernelExtents);
				penaltiesVerticalPass [x, 0] += penaltiesHorizontalPass [x, sampleZ];
			}

			int blurredPenalty = Mathf.RoundToInt(penaltiesVerticalPass [x, 0] / Mathf.Pow(kernelSize, 2));
			grid [x, 0].movementPenalty = blurredPenalty;

			for (int z = 1; z < gridSizeZ; z++) {
				int removeIndex = Mathf.Clamp(z - kernelExtents - 1, 0, gridSizeZ);
				int addIndex = Mathf.Clamp(z + kernelExtents, 0, gridSizeZ-1);

				penaltiesVerticalPass [x, z] = penaltiesVerticalPass [x, z - 1] - penaltiesHorizontalPass [x, removeIndex] + penaltiesHorizontalPass [x, addIndex];
				blurredPenalty = Mathf.RoundToInt(penaltiesVerticalPass [x, z] / Mathf.Pow(kernelSize, 2));
				grid [x, z].movementPenalty = blurredPenalty;

				if (blurredPenalty > penaltyMax) {
					penaltyMax = blurredPenalty;
				}
				if (blurredPenalty < penaltyMin) {
					penaltyMin = blurredPenalty;
				}
			}
		}
    }

    public Node GetNodeFromWorldPoint(Vector3 _worldPosition)
    {
        float percentX = Mathf.Clamp01((_worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float percentZ = Mathf.Clamp01((_worldPosition.z + gridWorldSize.z / 2) / gridWorldSize.z);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);

        return grid[x, z];
    }

    public List<Node> GetNodeNeighbours(Node _node)
    {
        List<Node> neighbourNodes = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0) continue;

                int checkX = _node.gridX + x;
                int checkZ = _node.gridZ + z;

                if (checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ)
                {
                    neighbourNodes.Add(grid[checkX, checkZ]);
                }
            }
        }

        return neighbourNodes;
    }

    public void EnablePathingVisuals(bool isEnabled)
    {
        pathingVisuals.SetActive(isEnabled);
    }
    #endregion
}