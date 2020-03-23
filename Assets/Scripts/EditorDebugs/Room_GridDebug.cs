using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Room_GridDebug : MonoBehaviour
{
    public bool showIfSelected = true;

    Room_Grid roomGrid;

    private void Awake()
    {
        roomGrid = GetComponent<Room_Grid>();
    }

    private void OnDrawGizmos()
    {
        if (showIfSelected)
        {
            if (Selection.Contains(gameObject))
            {
                Gizmos.DrawWireCube(transform.position, new Vector3(roomGrid.gridWorldSize.x, 1, roomGrid.gridWorldSize.y));
                if (roomGrid != null)
                {
                    Vector3 nodeBox = Vector3.one * 0.6f * (roomGrid.nodeRadius * 2);
                    nodeBox.y /= 6;
                    foreach (PathNode n in roomGrid.Grid)
                    {
                        Gizmos.color = (n.isWalkable) ? Color.cyan : Color.red;
                        Gizmos.DrawCube(n.worldPos, nodeBox);
                    }
                }
            }
        }
        else
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(roomGrid.gridWorldSize.x, 1, roomGrid.gridWorldSize.y));
            if (roomGrid != null)
            {
                Vector3 nodeBox = Vector3.one * 0.6f * (roomGrid.nodeRadius * 2);
                nodeBox.y /= 6;
                foreach (PathNode n in roomGrid.Grid)
                {
                    Gizmos.color = (n.isWalkable) ? Color.cyan : Color.red;
                    Gizmos.DrawCube(n.worldPos, nodeBox);
                }
            }
        }
        
    }
}
