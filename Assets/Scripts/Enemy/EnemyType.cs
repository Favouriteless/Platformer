using System.Collections.Generic;

public abstract class EnemyType<T> : EnemyType where T : Enemy
{
    private List<Goal<T>> GOALS = new List<Goal<T>>();

    public EnemyType()
    {
        SetupGoals();
    }

    protected void AddGoal(Goal<T> goal)
    {
        GOALS.Add(goal);
    }

    protected abstract void SetupGoals();

    public override void AddEnemy(Enemy enemy)
    {
        if (enemy is T value)
        {
            GOALS.ForEach((goal) => { goal.Add(enemy, value); });
        }
    }

    public override void RemoveEnemy(Enemy enemy)
    {
        GOALS.ForEach((goal) => { goal.Remove(enemy); });
    }

    public override void ProcessGoals(Enemy enemy)
    {
        if (GOALS.Count > 0)
        {
            if (enemy.currentGoalIndex == -1 || GOALS[enemy.currentGoalIndex].IsCancellable())
            {
                int priorityGoalIndex = 0;
                int highestPriority = -100000;

                int i = 0;
                while (i < GOALS.Count)
                {
                    int priority = GOALS[i].GetPriority(enemy);

                    if (priority > highestPriority)
                    {
                        highestPriority = priority;
                        priorityGoalIndex = i;
                    }
                    i++;
                }

                enemy.currentGoalIndex = priorityGoalIndex;
                GOALS[priorityGoalIndex].Execute(enemy);
            }
        }
        else
        {
            GOALS[enemy.currentGoalIndex].Execute(enemy);
        }
    }

}

// Don't touch this, it's just a non-generic implementation to allow for fake wildcards on the registry
public abstract class EnemyType
{
    public abstract void ProcessGoals(Enemy enemy);
    public abstract void AddEnemy(Enemy enemy);
    public abstract void RemoveEnemy(Enemy enemy);
}
