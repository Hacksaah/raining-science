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

    void genLevel()
    {
        rooms.Enqueue(room);
        GameObject currentRoom;
        while (roomsSpawned < numberOfRooms)
        {
            currentRoom = rooms.Dequeue();
            while (rooms.Count == 0 && roomsSpawned < numberOfRooms)
            {
                if (roomsSpawned - 1 == numberOfRooms && !bossRoomSpawned)
                    bossRoomSpawnChance = 100;
                spawnRooms(currentRoom);
            }
            if (checkConnectedRooms(currentRoom, 1, 1))
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

                if (checkConnectedRooms(currentRoom, 1, 1))
                {
                    if(!roomMap.ContainsKey(face))
                        roomMap.Add(face, currentRoom);
                    positions.AddLast(face);
                    continue;
                }

                if (roomsSpawned < numberOfRooms && !positions.Contains(face))
                {
                    positions.AddLast(face);
                    GameObject newRoom = GameObject.Instantiate(room, face, Quaternion.Euler(Vector3.left));
                    if (!bossRoomSpawned && random(bossRoomSpawnChance))
                    {
                        newRoom.tag = "BossRoom";
                        bossRoomSpawned = true;
                    }
                    else
                        newRoom.tag = "GenRoom";
                    if(!roomMap.ContainsKey(face))
                        roomMap.Add(face, newRoom);
                    newRoom.transform.position = face;
                    newRoom.transform.rotation = room.transform.rotation;
                    newRoom.GetComponent<MeshFilter>().mesh = room.GetComponent<MeshFilter>().mesh;
                    rooms.Enqueue(newRoom);
                    roomsSpawned++;
                }
            }
        }
    }

    Boolean checkConnectedRooms(GameObject currentRoom, int startDegree, int endDegree)
    {
        Boolean tooManyRooms = false;

        int numOfConnRooms = 0;

        Vector3 frontFace = currentRoom.transform.position + currentRoom.transform.forward * currentRoom.GetComponent<MeshRenderer>().bounds.size.z;
        Vector3 backFace = currentRoom.transform.position - currentRoom.transform.forward * currentRoom.GetComponent<MeshRenderer>().bounds.size.z;
        Vector3 rightFace = currentRoom.transform.position + currentRoom.transform.right * currentRoom.GetComponent<MeshRenderer>().bounds.size.x;
        Vector3 leftFace = currentRoom.transform.position - currentRoom.transform.right * currentRoom.GetComponent<MeshRenderer>().bounds.size.x;

        if (startDegree < endDegree)
        {
            if(roomMap.ContainsKey(frontFace))
                tooManyRooms = checkConnectedRooms(roomMap[frontFace], startDegree+1, endDegree);
            if (tooManyRooms)
                return tooManyRooms;
            if (roomMap.ContainsKey(backFace))
                tooManyRooms = checkConnectedRooms(roomMap[backFace], startDegree+1, endDegree);
            if (tooManyRooms)
                return tooManyRooms;
            if (roomMap.ContainsKey(rightFace))
                tooManyRooms = checkConnectedRooms(roomMap[rightFace], startDegree+1, endDegree);
            if (tooManyRooms)
                return tooManyRooms;
            if (roomMap.ContainsKey(leftFace))
                tooManyRooms = checkConnectedRooms(roomMap[leftFace], startDegree+1, endDegree);
            if (tooManyRooms)
                return tooManyRooms;
        }

        if (positions.Contains(frontFace))
            numOfConnRooms++;
        if (positions.Contains(backFace))
            numOfConnRooms++;
        if (positions.Contains(rightFace))
            numOfConnRooms++;
        if (positions.Contains(leftFace))
            numOfConnRooms++;

        if (numOfConnRooms >= maxConnectingRooms)
            tooManyRooms = true;

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
