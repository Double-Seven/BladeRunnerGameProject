﻿using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class SpawnEnemyManager : MonoBehaviour
{
    public SpawnEnemyManager instance;
    public List<Monster> allMonsters;
    public float spawnRate = 3f;
    public float startTime = 0f;

    private Player player;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Debug.LogError("Duplicate spawn manager, removing this one", gameObject);
        }
        player = GameFlowManager.instance.getPlayer();

        InvokeRepeating("Spawn", startTime, spawnRate);
    }

    void Spawn()
    {
        List<Monster> monsters = GameFlowManager.instance.monsters;
        Monster monsterToSpawn = nextSpawn(monsters);
        Vector2 positionSpawn = positionNearPlayer(player.transform.position);

        if (monsters.Count < 3)
        {
            Instantiate(monsterToSpawn, positionSpawn, transform.rotation);
        }
    }

    Vector2 positionNearPlayer(Vector2 positionPlayer)
    {
        float xVal = Random.Range(positionPlayer.x - 5, positionPlayer.x + 6);
        float yVal = Random.Range(positionPlayer.y, positionPlayer.y + 1);

        return new Vector2(xVal, yVal);
    }

    Monster nextSpawn(List<Monster> curMonsters)
    {
        return allMonsters[Random.Range(0, allMonsters.Count)];
    }
}