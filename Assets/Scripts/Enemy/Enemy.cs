using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public abstract EnemyType GetEnemyType();
    public int currentGoalIndex = -1;

    public void SetGoalFinished()
    {
        currentGoalIndex = -1;
    }
}
