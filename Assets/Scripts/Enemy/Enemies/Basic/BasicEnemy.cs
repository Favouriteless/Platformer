using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    public override EnemyType GetEnemyType()
    {
        return Enemies.BASIC;
    }
}
