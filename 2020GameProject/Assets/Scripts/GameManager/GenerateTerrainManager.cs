using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class GenerateTerrainManager : MonoBehaviour
{
    public List<GameObject> obstacles;
    public GameObject camera;
    float lastObstacle;
    float cameraPos;
    // distance to next generated object from camera
    float distance = 20f;
    float minObstacleDistance = 5f;

    private void Start()
    {
        lastObstacle = camera.transform.position.x;
    }

    private void FixedUpdate()
    {
       
        // player moved forward
        if (camera.transform.position.x > cameraPos + 1) {
            cameraPos = camera.transform.position.x;
            float dice = Random.Range(0, 20);
            GenerateObstable(dice);
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
}
