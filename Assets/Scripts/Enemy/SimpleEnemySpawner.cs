using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemySpawner : MonoBehaviour
{
    public EnemyManager manager;
    public GameObject enemyPrefab;
    private Enemy enemy;

    void Start()
    {
        enemy = manager.SpawnEnemy(enemyPrefab, transform.position, transform.rotation);
    }
}
