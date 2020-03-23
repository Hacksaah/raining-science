using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;

    public int spawnCount = 10;

    //Custom Components
    private Room_Grid roomGrid;


    private void Awake()
    {
        roomGrid = GetComponent<Room_Grid>();
    }

    // Start is called before the first frame update
    void Start()
    {        
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for(int i = 0; i < spawnCount; i++)
        {            
            EnemyActor newEnemy = Instantiate(EnemyPrefab).GetComponent<EnemyActor>();
            newEnemy.gameObject.SetActive(false);
            newEnemy.roomKey = roomGrid.RoomKey;
            newEnemy.AttackTarget = roomGrid.PlayerTransform;
            newEnemy.SpawnActor(roomGrid.AnOpenSpot(), newEnemy.AttackTarget.position);
            newEnemy.gameObject.SetActive(true);
        }
    }
}
