using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager INSTANCE;

    [Header("Enemy Settings")]
    [Range(0, 60)] public int updateRate;

    private List<Enemy> enemies = new List<Enemy>();
    private float timeSinceUpdate;

    public Enemy SpawnEnemy(GameObject enemyPrefab, Vector2 position, Quaternion rotation)
    {
        Enemy enemy = Instantiate(enemyPrefab, position, rotation).GetComponent<Enemy>();
        enemy.GetEnemyType().AddEnemy(enemy);
        enemies.Add(enemy);
        return enemy;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemy.GetEnemyType().RemoveEnemy(enemy);
        enemies.Remove(enemy);
        Destroy(enemy);
    }

    private void Awake()
    {
        INSTANCE = this;
    }

    private void Update()
    {
        if (updateRate != 0)
        {
            if (timeSinceUpdate >= 1 / updateRate)
            {
                UpdateEnemies();
                timeSinceUpdate = 0f;
            }
            else
            {
                timeSinceUpdate += Time.deltaTime;
            }
        }
    }

    private void UpdateEnemies()
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.GetEnemyType().ProcessGoals(enemy);
        }
    }

    private void OnDestroy()
    {
        INSTANCE = null;
    }

}
