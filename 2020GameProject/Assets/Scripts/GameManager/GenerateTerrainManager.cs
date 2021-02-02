using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class GenerateTerrainManager : MonoBehaviour
{
    public List<GameObject> obstacles;
    public GenerateTerrainManager instance;
    public GameObject camera;
    public SpawnEnemyManager EnemyManager;
    float lastObstacle;
    float cameraPos;
    // distance to next generated object from camera
    float distance = 10f;
    float minObstacleDistance = 5f;
    float lastEnemy;
    float minEnemyDistance = 2f;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Duplicate terrain manager, removing this one", gameObject);
        }
        lastObstacle = camera.transform.position.x;

        EnemyManager = SpawnEnemyManager.instance;
    }

    private void FixedUpdate()
    {
       
        // player moved forward
        if (camera.transform.position.x > cameraPos + 1) {
            cameraPos = camera.transform.position.x;
            float dice = Random.Range(0, 20);
            GenerateObstable(dice);
            GenerateEnemy(dice);
        }
    }

    private void GenerateObstable(float dice)
    {
        
        if (dice < 1 && cameraPos + distance > lastObstacle + minObstacleDistance)
        {
            Instantiate(obstacles[Random.Range(0, obstacles.Count)], new Vector2(cameraPos + distance, -1f), transform.rotation);
            lastObstacle = cameraPos + distance;
   
        }
    }

    private void GenerateEnemy(float dice)
    {
        float spawnChance = 2f;
        spawnChance += Mathf.Max(3f, cameraPos / 100f);

        if (dice > 20 - spawnChance && cameraPos + distance > lastEnemy + minEnemyDistance)
        {
            EnemyManager.Spawn(new Vector2(cameraPos + distance, 1f));
            lastEnemy = cameraPos + distance;
        }
       
    }
}
