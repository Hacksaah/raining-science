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
    public int bossRoomSpawnChance = 5;

    private Queue<GameObject> rooms = new Queue<GameObject>();
    private int roomsSpawned = 0;
    private LinkedList<Vector3> positions = new LinkedList<Vector3>();
    private LinkedList<Vector3> spawnedRooms = new LinkedList<Vector3>();
    private Dictionary<Vector3, GameObject> roomMap = new Dictionary<Vector3, GameObject>();
    private Boolean bossRoomSpawned = false;


    private void Start()
    {
        CombineMesh meshScript = (CombineMesh)room.GetComponent(typeof(CombineMesh));
        meshScript.combine();
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

    void unlockDoors()
    {
        foreach(Vector3 pos in spawnedRooms)
        {
            DoorScript doorScript;
            GameObject door;
            GameObject currentRoom = roomMap[pos];

            if (roomMap.ContainsKey(new Vector3(pos.x, pos.y, pos.z+96)))
            {
                door = currentRoom.transform.Find("NorthDoor").gameObject;
                for (int z = 0; z < door.transform.childCount; z++)
                {
                    doorScript = (DoorScript)door.transform.GetChild(z).gameObject.GetComponent(typeof(DoorScript));
                    doorScript.Lock(true);
                }
            }
            if (roomMap.ContainsKey(new Vector3(pos.x, pos.y, pos.z - 96)))
            {
                door = currentRoom.transform.Find("SouthDoor").gameObject;
                for (int z = 0; z < door.transform.childCount; z++)
                {
                    doorScript = (DoorScript)door.transform.GetChild(z).gameObject.GetComponent(typeof(DoorScript));
                    doorScript.Lock(true);
                }
            }
            if (roomMap.ContainsKey(new Vector3(pos.x + 96, pos.y, pos.z)))
            {
                door = currentRoom.transform.Find("EastDoor").gameObject;
                for (int z = 0; z < door.transform.childCount; z++)
                {
                    doorScript = (DoorScript)door.transform.GetChild(z).gameObject.GetComponent(typeof(DoorScript));
                    doorScript.Lock(true);
                }
            }
            if (roomMap.ContainsKey(new Vector3(pos.x - 96, pos.y, pos.z)))
            {
                door = currentRoom.transform.Find("WestDoor").gameObject;
                for (int z = 0; z < door.transform.childCount; z++)
                {
                    doorScript = (DoorScript)door.transform.GetChild(z).gameObject.GetComponent(typeof(DoorScript));
                    doorScript.Lock(true);
                }
            }
        }
        print("Doors unlocked");
    }
    void genLevel()
    {
        rooms.Enqueue(room);
        spawnedRooms.AddLast(room.transform.position);
        positions.AddLast(room.transform.position);
        roomMap.Add(room.transform.position, room);
        GameObject currentRoom;
        int iterationThreshold = 0;
        Boolean nextThreshold = false;
        int maxThreshold = 0;
        while (roomsSpawned < numberOfRooms)
        {
            currentRoom = rooms.Dequeue();
            if (roomsSpawned - 1 == numberOfRooms && !bossRoomSpawned)
                bossRoomSpawnChance = 100;
            while (rooms.Count == 0 && !spawnRooms(currentRoom))
            {
                iterationThreshold++;
                if (maxThreshold == 3)
                {
                    print("Final Threshold hit with " + (roomsSpawned+1) + " number of rooms spawned out of " + (numberOfRooms+1));
                    return;
                }
                if (iterationThreshold == 3 && !nextThreshold)
                {
                    print("Threshold 1 hit with " + (roomsSpawned + 1) + " number of rooms spawned out of " + (numberOfRooms + 1));
                    print("Trying to spawn rooms from existing rooms");
                    nextThreshold = true;
                    iterationThreshold = 0;
                    positions.Clear();
                    foreach (Vector3 r in spawnedRooms)
                    {
                        positions.AddLast(r);
                        rooms.Enqueue(roomMap[r]);
                    }
                }
                else if (iterationThreshold == 3 && nextThreshold)
                {
                    print("Threshold 2 hit with " + (roomsSpawned + 1) + " number of rooms spawned out of " + (numberOfRooms + 1));
                    print("Trying to spawn rooms from existing rooms");
                    if (maxConnectingRooms < 4)
                    {
                        print("Increasing max connections from " + maxConnectingRooms + " to " + (maxConnectingRooms + 1));
                        maxConnectingRooms++;
                    }
                    iterationThreshold = 0;
                    nextThreshold = false;
                    maxThreshold++;
                    foreach (Vector3 r in spawnedRooms)
                    {
                        rooms.Enqueue(roomMap[r]);
                    }
                }
            }            
            spawnRooms(currentRoom);
        }
        print("Spawned all " + (roomsSpawned + 1) + " out of " + (numberOfRooms + 1));
        unlockDoors();
    }

    Boolean spawnRooms(GameObject currentRoom)
    {
        Boolean spawnedRoom = false;
        Vector3 face = new Vector3(0, 0, 0);
        for (int i = 0; i < 4; i++)
        {
            if (random(branchPercent))
            {
                switch (i)
                {
                    case 0:
                        face = currentRoom.transform.position + currentRoom.transform.forward * currentRoom.GetComponent<MeshRenderer>().bounds.size.z;
                        break;
                    case 1:
                        face = currentRoom.transform.position - currentRoom.transform.forward * currentRoom.GetComponent<MeshRenderer>().bounds.size.z;
                        break;
                    case 2:
                        face = currentRoom.transform.position - currentRoom.transform.right * currentRoom.GetComponent<MeshRenderer>().bounds.size.x;
                        break;
                    case 3:
                        face = currentRoom.transform.position + currentRoom.transform.right * currentRoom.GetComponent<MeshRenderer>().bounds.size.x;
                        break;
                        
                }
                face.y = currentRoom.transform.position.y;
                face.x = Mathf.RoundToInt(face.x);
                face.z = Mathf.RoundToInt(face.z);

                if (checkConnectedRooms(currentRoom.transform.position, 1, 2, i))
                {
                    if(!positions.Contains(face))
                        positions.AddLast(face);
                    continue;
                }

                if (roomsSpawned < numberOfRooms && !positions.Contains(face))
                {
                    if(!positions.Contains(face))
                        positions.AddLast(face);

                    GameObject newRoom = GameObject.Instantiate(room, face, Quaternion.Euler(Vector3.left));

                    if (!bossRoomSpawned && random(bossRoomSpawnChance))
                    {
                        newRoom.tag = "BossRoom";
                        bossRoomSpawned = true;
                    }
                    else
                        newRoom.tag = "GenRoom";

                    if (!spawnedRooms.Contains(face))
                        spawnedRooms.AddLast(face);

                    if (!roomMap.ContainsKey(face))
                        roomMap.Add(face, newRoom);

                    newRoom.transform.position = face;
                    newRoom.transform.rotation = room.transform.rotation;
                    newRoom.GetComponent<MeshFilter>().mesh = room.GetComponent<MeshFilter>().mesh;
                    rooms.Enqueue(newRoom);
                    roomsSpawned++;
                    spawnedRoom = true;
                }
            }
        }
        return spawnedRoom;
    }

    Boolean checkConnectedRooms(Vector3 currentRoom, int startDegree, int endDegree, int face)
    {
        Boolean tooManyRooms = false;

        int numOfConnRooms = 0;

        Vector3 frontFace = new Vector3(currentRoom.x, currentRoom.y, currentRoom.z + 96);
        Vector3 backFace = new Vector3(currentRoom.x, currentRoom.y, currentRoom.z - 96);
        Vector3 rightFace = new Vector3(currentRoom.x + 96, currentRoom.y, currentRoom.z);
        Vector3 leftFace = new Vector3(currentRoom.x - 96, currentRoom.y, currentRoom.z);

        if (startDegree < endDegree)
        {
            if((positions.Contains(frontFace) || spawnedRooms.Contains(frontFace)) && (face == 0 || face == -1))
                tooManyRooms = checkConnectedRooms(frontFace, startDegree+1, endDegree, -1);
            if (tooManyRooms)
                return tooManyRooms;
            if ((positions.Contains(backFace) || spawnedRooms.Contains(backFace)) && (face == 1 || face == -1))
                tooManyRooms = checkConnectedRooms(backFace, startDegree+1, endDegree, -1);
            if (tooManyRooms)
                return tooManyRooms;
            if ((positions.Contains(rightFace) || spawnedRooms.Contains(rightFace)) && (face == 3 || face == -1))
                tooManyRooms = checkConnectedRooms(rightFace, startDegree+1, endDegree, -1);
            if (tooManyRooms)
                return tooManyRooms;
            if ((positions.Contains(leftFace) || spawnedRooms.Contains(leftFace)) && (face == 2 || face == -1))
                tooManyRooms = checkConnectedRooms(leftFace, startDegree+1, endDegree, -1);
            if (tooManyRooms)
                return tooManyRooms;
        }

        if (positions.Contains(frontFace) || spawnedRooms.Contains(frontFace))
            numOfConnRooms++;
        if (positions.Contains(backFace) || spawnedRooms.Contains(backFace))
            numOfConnRooms++;
        if (positions.Contains(rightFace) || spawnedRooms.Contains(rightFace))
            numOfConnRooms++;
        if (positions.Contains(leftFace) || spawnedRooms.Contains(leftFace))
            numOfConnRooms++;

        /*print("");
        print("Room being checked" + currentRoom);
        print("Degree " + startDegree);
        print("Face " + face);
        print(frontFace);
        print(backFace);
        print(rightFace);
        print(leftFace);*/

        if (numOfConnRooms >= maxConnectingRooms)
            tooManyRooms = true;

        //print("Too many rooms: " + tooManyRooms);

        return tooManyRooms;
    }

    Boolean random(int percent)
    {
        if (UnityEngine.Random.Range(0,101) <= percent)
            return true;
        else
            return false;
    }
}
