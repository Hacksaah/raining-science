using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    public GameObject room;
    public int branchPercent;
    public int numberOfRooms;
    public int maxConnectingRooms;

    private Queue<GameObject> rooms = new Queue<GameObject>();
    private int roomsSpawned = 0;
    private LinkedList<Vector3> positions = new LinkedList<Vector3>();

    private void Awake()
    {
        if (maxConnectingRooms > 4)
            maxConnectingRooms = 4;
        else if (maxConnectingRooms < 2)
            maxConnectingRooms = 2;
        if (branchPercent > 100)
            branchPercent = 100;
        else if (branchPercent < 1)
            branchPercent = 1;
        if (numberOfRooms < 1)
            numberOfRooms = 1;
        positions.AddFirst(new Vector3(room.transform.position.x, room.transform.position.y, room.transform.position.z));
        numberOfRooms--;
        genLevel();
    }

    void genLevel()
    {
        rooms.Enqueue(room);
        GameObject currentRoom;
        while (roomsSpawned < numberOfRooms)
        {
            currentRoom = rooms.Dequeue();
            while (rooms.Count == 0 && roomsSpawned < numberOfRooms)
            {
                spawnRooms(currentRoom);
            }
            if (checkConnectedRooms(currentRoom))
                continue;
            
            spawnRooms(currentRoom);
        }
    }

    void spawnRooms(GameObject currentRoom)
    {
        Vector3 face = new Vector3(0, 0, 0);
        for (int i = 0; i < 4; i++)
        {
            int x = UnityEngine.Random.Range(0,4);
            if (random(branchPercent))
            {
                switch (x)
                {
                    case 0:
                        face = currentRoom.transform.position + currentRoom.transform.up * currentRoom.GetComponent<MeshRenderer>().bounds.size.z;
                        break;
                    case 1:
                        face = currentRoom.transform.position - currentRoom.transform.up * currentRoom.GetComponent<MeshRenderer>().bounds.size.z;
                        break;
                    case 2:
                        face = currentRoom.transform.position - currentRoom.transform.right * currentRoom.GetComponent<MeshRenderer>().bounds.size.z;
                        break;
                    case 3:
                        face = currentRoom.transform.position + currentRoom.transform.right * currentRoom.GetComponent<MeshRenderer>().bounds.size.z;
                        break;
                        
                }

                if (checkConnectedRooms(currentRoom))
                {
                    positions.AddLast(face);
                    continue;
                }

                if (roomsSpawned < numberOfRooms && !positions.Contains(face))
                {
                    positions.AddLast(face);
                    GameObject newRoom = GameObject.Instantiate(room);
                    newRoom.transform.position = face;
                    rooms.Enqueue(newRoom);
                    roomsSpawned++;
                }
            }
        }
    }

    Boolean checkConnectedRooms(GameObject currentRoom)
    {
        int numOfConnRooms = 0;

        Vector3 frontFace = currentRoom.transform.position + currentRoom.transform.up * currentRoom.GetComponent<MeshRenderer>().bounds.size.z;
        Vector3 backFace = currentRoom.transform.position - currentRoom.transform.up * currentRoom.GetComponent<MeshRenderer>().bounds.size.z;
        Vector3 rightFace = currentRoom.transform.position + currentRoom.transform.right * currentRoom.GetComponent<MeshRenderer>().bounds.size.z;
        Vector3 leftFace = currentRoom.transform.position - currentRoom.transform.right * currentRoom.GetComponent<MeshRenderer>().bounds.size.z;

        if (positions.Contains(frontFace))
            numOfConnRooms++;
        if (positions.Contains(backFace))
            numOfConnRooms++;
        if (positions.Contains(rightFace))
            numOfConnRooms++;
        if (positions.Contains(leftFace))
            numOfConnRooms++;

        if (numOfConnRooms >= maxConnectingRooms)
            return true;
        else
            return false;
    }

    Boolean random(int percent)
    {
        if (UnityEngine.Random.Range(0,101) <= percent)
            return true;
        else
            return false;
    }
}
