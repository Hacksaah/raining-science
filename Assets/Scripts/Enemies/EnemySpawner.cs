using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> EnemyPrefabs;
  

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
        StartCoroutine(SpawnEnemyDelay());
    }

    void SpawnEnemies()
    {
        if (gameObject.transform.parent.gameObject.tag == "GenRoom")
        {
            for (int i = 0; i < spawnCount; i++)
            {

                int rand = Random.Range(0, EnemyPrefabs.Count);
                EnemyActor newEnemy = Instantiate(EnemyPrefabs[rand]).GetComponent<EnemyActor>();
                newEnemy.gameObject.SetActive(false);
                newEnemy.transform.parent = gameObject.transform.parent;
                newEnemy.roomKey = roomGrid.RoomKey;
                //newEnemy.AttackTarget = Level_Grid.Instance.PlayerTransform;
                //newEnemy.SpawnActor(roomGrid.AnOpenSpot(), newEnemy.AttackTarget.position);
                newEnemy.gameObject.SetActive(true);
            }
        }
        if (gameObject.transform.parent.gameObject.tag == "BossRoom")
        {
            
        }
    }

    IEnumerator SpawnEnemyDelay()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        SpawnEnemies();
    }
}
