using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Room_Grid : MonoBehaviour
{
    public Transform PlayerTransform;

    public LayerMask unwalkableLayer;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    PathNode[,] grid;
    public PathNode[,] Grid { get { return grid; } }

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    [SerializeField]
    private int roomKey;
    public int RoomKey { get { return roomKey; } }

    // Start is called before the first frame update
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        grid = new PathNode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * (gridWorldSize.x / 2) - Vector3.forward * (gridWorldSize.y / 2);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                Vector3 nodeBox = Vector3.one * nodeRadius;
                bool walkable = !(Physics.CheckBox(worldPoint, nodeBox, Quaternion.identity, unwalkableLayer));

                int movementPenalty = 0;
                grid[x, y] = new PathNode(walkable, worldPoint, x, y, movementPenalty);
            }
        }
        Debug.Log("Room pathfinding grid made");
        // Add this room to the Level and retreive the dictionary key value
        roomKey = Level_Grid.Instance.AddRoomToLevel(this);
    }

    public PathNode NodeFromWorldPoint(Vector3 _worldPos)
    {
        float percentX = (_worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (_worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<PathNode> GetNeighbours(PathNode centerNode)
    {
        List<PathNode> neighbours = new List<PathNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = centerNode.gridX + x;
                int checkY = centerNode.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public List<PathNode> GetOpenNodes(PathNode startNode, int searchRadius)
    {
        List<PathNode> openNodes = new List<PathNode>();
        int x = startNode.gridX - searchRadius;
        int maxX = x + 2 * searchRadius;
        int y = startNode.gridY - searchRadius;
        int maxY = y + 2 * searchRadius;
        x = x < 0 ? 0 : x;
        maxX = maxX >= gridSizeX ? gridSizeX : maxX;
        y = y < 0 ? 0 : y;
        maxY = maxY >= gridSizeY ? gridSizeY : maxY;
        for (; x < maxX; x++)
        {
            for (; y < maxY; y++)
            {
                PathNode curNode = grid[x, y];
                if (curNode.isWalkable)
                {
                    openNodes.Add(curNode);
                }
            }
        }
        return openNodes;
    }

    public Vector3 FindRandomSpotBetween(Vector3 target, int searchMin, int searchMax)
    {
        PathNode targetNode = NodeFromWorldPoint(target);
        int x = targetNode.gridX - searchMax;        
        int y = targetNode.gridY - searchMax;        
        int xMax = x + 2*searchMax;
        int yMax = y + 2*searchMax;

        int xBound1 = targetNode.gridX - searchMin;
        int xBound2 = targetNode.gridX + searchMin;
        int yBound1 = targetNode.gridY - searchMin;
        int yBound2 = targetNode.gridY + searchMin;

        x = x < 0 ? 0 : x;
        y = y < 0 ? 0 : y;
        xMax = xMax >= gridSizeX ? gridSizeX : xMax;
        yMax = yMax >= gridSizeY ? gridSizeY : yMax;

        xBound1 = xBound1 < 0 ? 0 : xBound1;
        yBound1 = yBound1 < 0 ? 0 : yBound1;
        xBound2 = xBound2 >= gridSizeX ? gridSizeX : xBound2;
        yBound2 = yBound2 >= gridSizeY ? gridSizeY : yBound2;        

        List<PathNode> nodes = new List<PathNode>();

        for(; x < xMax; x++)
        {
            for(; y < yMax; y++)
            {
                if (x >= xBound1 && y >= yBound1 && x <= xBound2 && y <= yBound2)
                    continue;
                else if (grid[x, y].isWalkable)
                    nodes.Add(grid[x, y]);
            }
        }

        if (nodes.Count != 0)
            return nodes[Random.Range(0, nodes.Count)].worldPos;

        return Vector3.zero;
    }

    //returns a random walkable spot in the room
    public Vector3 AnOpenSpot()
    {
        Vector3 openSpot;

        int x = Random.Range(0, gridSizeX-1);
        int y = Random.Range(0, gridSizeY-1);

        while (!grid[x, y].isWalkable)
        {
            x++;            
            if (x == gridSizeX)
            {
                x = 0;
                y++;
                if (y == gridSizeY)
                    y = 0;
            }
        }
        openSpot = grid[x, y].worldPos;

        return openSpot;
    }    
}
