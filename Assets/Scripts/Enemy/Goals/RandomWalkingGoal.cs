using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalkingGoal<T> : Goal<T> where T : IEnemyMotor
{
    public override void Execute(Enemy enemy)
    {

    }

    public override int GetPriority(Enemy enemy)
    {
        return 0;
    }

    public override bool IsCancellable()
    {
        return true;
    }
}
