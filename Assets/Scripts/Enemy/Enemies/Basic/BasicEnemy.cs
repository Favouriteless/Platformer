using UnityEngine;

[RequireComponent(typeof(EnemyMotor))]
public class BasicEnemy : Enemy, IEnemyMotor
{
    [Header("References")]
    public EnemyMotor motor;

    public EnemyMotor Motor => motor;

    public override EnemyType GetEnemyType()
    {
        return Enemies.BASIC;
    }
}
