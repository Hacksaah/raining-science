using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTheme : MonoBehaviour
{
    public GameObject[] roomItems;
    public List<GameObject> BossPrefabs;

    private bool playerInRoom = false;
    public Room_Grid roomGrid;

    private void Awake()
    {
        roomGrid = (Room_Grid)gameObject.transform.Find("Pathfinding").GetComponent(typeof(Room_Grid));
    }

    public void spawnBoss()
    {
        if(!playerInRoom)
        {
            int rand = Random.Range(0, BossPrefabs.Count);
            EnemyActor boss = Instantiate(BossPrefabs[rand]).GetComponent<EnemyActor>();
            boss.gameObject.SetActive(false);
            boss.transform.parent = gameObject.transform;
            boss.roomKey = roomGrid.RoomKey;
            boss.AttackTarget = roomGrid.PlayerTransform;
            boss.SpawnActor(roomGrid.AnOpenSpot(), boss.AttackTarget.position);
            boss.gameObject.SetActive(true);
            playerInRoom = true;
        }
    }
}
