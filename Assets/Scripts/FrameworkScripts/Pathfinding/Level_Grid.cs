using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Grid : MonoBehaviour
{
    static Level_Grid instance;
    public static Level_Grid Instance {
        get
        {
            if (instance == null)
                new Level_Grid();            
            return instance;
        }
    }

    public Transform PlayerTransform { get; set; }

    private Dictionary<int, Room_Grid> roomDict = new Dictionary<int, Room_Grid>();
    [SerializeField]
    private int roomKey = 0;

    Level_Grid() { instance = this; }

    public int AddRoomToLevel(Room_Grid room)
    {
        roomDict.Add(roomKey, room);
        roomKey++;
        return roomKey - 1;
    }

    public Room_Grid GetRoom(int key)
    {
        return roomDict[key];
    }
}
