using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Grid : MonoBehaviour
{
    static Level_Grid instance;
    private List<Room_Grid> rooms = new List<Room_Grid>();

    private void Awake()
    {
        instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddRoomToLevelGrid(Room_Grid _room)
    {

    }

    public void NodeFromWorldPoint(Vector3 _worldPos)
    {

        return;
    }

    public List<PathNode> GetNeighbors(PathNode centerNode)
    {
        List<PathNode> neighbours = new List<PathNode>();

        return neighbours;
    }
}
