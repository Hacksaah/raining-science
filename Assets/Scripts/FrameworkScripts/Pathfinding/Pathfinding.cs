using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;

public class Pathfinding : MonoBehaviour
{
    Path_Grid grid;

    public Transform start, target;

    private void Awake()
    {
        grid = GetComponent<Path_Grid>();
    }

    public void FindPath(PathRequest request, Action<PathResult> callback)
    {
        //Debug.Log("Looking for path");
        //Debug.Log(grid.NodeFromWorldPoint(new Vector3(45, 1, 58)).gridX);
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        PathNode startNode = grid.NodeFromWorldPoint(request.pathStart);
        PathNode targetNode = grid.NodeFromWorldPoint(request.pathEnd);

        //Debug.Log("Startnode: " + startNode.worldPos + ", EndNode: " + targetNode.worldPos);

        if(startNode.isWalkable && targetNode.isWalkable)
        {
            Heap<PathNode> openSet = new Heap<PathNode>(grid.MaxSize);
            HashSet<PathNode> closedSet = new HashSet<PathNode>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                //Debug.Log("Open set still open: " + openSet.Count);
                PathNode currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                //if reached the target node END HERE
                if (currentNode == targetNode)
                {
                    //Debug.Log("bingo");
                    pathSuccess = true;
                    break;
                }

                //----------------Otherwise search neighbors-----------------------
                foreach (PathNode neighbor in grid.GetNeighbours(currentNode))
                {
                    if (!neighbor.isWalkable || closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    int newMoveCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor) + neighbor.movementPenalty;
                    if (newMoveCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newMoveCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                        else
                            openSet.UpdateItem(neighbor);
                    }
                }
            }
        }
        else
        {
            Debug.Log("Start or target is not walkable node");
            Debug.Log("Start: " + startNode.isWalkable);
            Debug.Log("End: " + targetNode.isWalkable);
        }
        
        if (pathSuccess)
        {
            //Debug.Log("Found path");
            waypoints = RetracePath(startNode, targetNode);
            callback(new PathResult(waypoints, pathSuccess, request.callback));
        }
        
    }

    int GetDistance(PathNode nodeA, PathNode nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }

    Vector3[] RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;
        
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<PathNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i].gridX - path[i-1].gridX, path[i].gridY - path[i-1].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i-1].worldPos);
            }
            directionOld = directionNew;
        }
        //Debug.Log(waypoints.Count);
        return waypoints.ToArray();
    }
}