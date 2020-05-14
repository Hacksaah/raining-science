using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelManager : MonoBehaviour
{
    public GameObject PlayerPrefab;

    static GameLevelManager instance;
    public static GameLevelManager Instance
    {
        get
        {
            if (instance == null)
                new GameLevelManager();
            return instance;
        }
    }

    public GameObject Player { get; set; }

    GameLevelManager() { instance = this; }

    private void Awake()
    {
        if(PlayerPrefab != null)
            Player = Instantiate(PlayerPrefab);
        else
        {
            Debug.Log("GAME LEVEL MANAGER ERROR :: No player prefab given");
            return;
        }
        
        Level_Grid.Instance.PlayerTransform = Player.transform;
        InteractManager.Instance.PlayerTransform = Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
