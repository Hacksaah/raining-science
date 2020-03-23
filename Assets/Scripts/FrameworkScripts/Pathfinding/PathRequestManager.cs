using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathResult> results = new Queue<PathResult>();

    static PathRequestManager instance;
    Pathfinding pathfinding;

    PathRequestManager() { instance = this; }

    private void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
        Debug.Log("Path Request Manager made");
    }

    private void Update()
    {
        if(results.Count > 0)
        {
            int itemsInQueue = results.Count;
            lock (results)
            {
                for(int i = 0; i < itemsInQueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success);
                }
            }
        }
    }

    public static void RequestPath(PathRequest request)
    {
        ThreadStart threadStart = delegate
        {
            instance.pathfinding.FindPath(request, instance.FinishedProcessingPath);
        };
        threadStart.Invoke();
    }


    //returns true when a Path has been found;
    public void FinishedProcessingPath(PathResult result)
    {
        lock (results)
        {
            results.Enqueue(result);
        }
    }

    public static Vector3 RequestNewMoveSpot(Vector3 position, int radius, int roomKey)
    {
        return instance.pathfinding.FindOpenNodePosition(position, radius, roomKey);
    }
}

//Data package for making a pathfind request for an AI unit
public struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public int roomKey;
    public Action<Vector3[], bool> callback;

    public PathRequest(Vector3 _start, Vector3 _end, int _roomKey, Action<Vector3[], bool> _callback)
    {
        pathStart = _start;
        pathEnd = _end;
        roomKey = _roomKey;
        callback = _callback;
    }
}

public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;

    public PathResult(Vector3[] _path, bool _success, Action<Vector3[], bool> _callback)
    {
        path = _path;
        success = _success;
        callback = _callback;
    }
}
