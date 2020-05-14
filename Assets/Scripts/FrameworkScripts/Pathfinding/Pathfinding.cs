using System.Collections.Generic;
using UnityEngine;
using System;
using Debug = UnityEngine.Debug;

public class Pathfinding : MonoBehaviour
{
    Level_Grid levelGrid;

    public Transform start, target;

    private void Awake()
    {
        levelGrid = GetComponent<Level_Grid>();
    }

    public void FindPath(PathRequest request, Action<PathResult> callback)
    {
        //Debug.Log("Looking for path :: PATHFINDING");

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Room_Grid roomGrid = levelGrid.GetRoom(request.roomKey);

        PathNode startNode = roomGrid.NodeFromWorldPoint(request.pathStart);
        PathNode targetNode = roomGrid.NodeFromWorldPoint(request.pathEnd);

        Debug.Log("Startnode :: " + startNode.isWalkable + " ,  TargetNode :: " + targetNode.isWalkable);

        if(startNode.isWalkable && targetNode.isWalkable)
        {
            Heap<PathNode> openSet = new Heap<PathNode>(roomGrid.MaxSize);
            HashSet<PathNode> closedSet = new HashSet<PathNode>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                PathNode currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                //if reached the target node END HERE
                if (currentNode == targetNode)
                {
                    //Debug.Log("DONE :: PATHFINDING");
                    pathSuccess = true;
                    break;
                }

                //Debug.Log("Seekin...");

                //----------------Otherwise search neighbors-----------------------
                foreach (PathNode neighbor in roomGrid.GetNeighbours(currentNode))
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
            //Debug.Log("Start or target is not walkable node");
            //Debug.Log("Start: " + startNode.isWalkable);
            //Debug.Log("End: " + targetNode.isWalkable);
        }
        
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
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

    public Vector3 FindOpenNodePosition(Vector3 startPosition, int searchRadius, int roomKey)
    {
        Room_Grid roomGrid = levelGrid.GetRoom(roomKey);
        PathNode startNode = roomGrid.NodeFromWorldPoint(startPosition);
        List<PathNode> openNodes = roomGrid.GetOpenNodes(startNode, searchRadius);
        int randomIndex = UnityEngine.Random.Range(0, openNodes.Count - 1);
        return openNodes[randomIndex].worldPos;
    }

    public Vector3 FindOpenPositionBetween(Vector3 targetPosition, int searchMin, int searchMax, int roomKey)
    {
        Room_Grid room_Grid = levelGrid.GetRoom(roomKey);
        // Return a random open world position from nose
        return room_Grid.FindRandomSpotBetween(targetPosition, searchMin, searchMax);
    }
}