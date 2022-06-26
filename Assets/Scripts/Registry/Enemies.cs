public class Enemies
{
    public static Registry<EnemyType> REGISTRY = new Registry<EnemyType>("enemies");

    public static BasicEnemyType BASIC = REGISTRY.Register("basic", new BasicEnemyType());
}
