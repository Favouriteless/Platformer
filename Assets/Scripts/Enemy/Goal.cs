using System.Collections.Generic;

public abstract class Goal<T>
{
    protected Dictionary<Enemy, T> enemies = new Dictionary<Enemy, T>();

    public void Add(Enemy enemy, T value)
    {
        enemies.Add(enemy, value);
    }

    public void Remove(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    public abstract int GetPriority(Enemy enemy);
    public abstract void Execute(Enemy enemy);
    public abstract bool IsCancellable();
}   
