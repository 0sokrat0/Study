using RoguelikeGame.Models;

namespace RoguelikeGame.Factories;

public static class MonsterFactory
{
    public static Enemy CreateEnemy(MonsterType type)
    {
        return type switch
        {
            MonsterType.Goblin => new Goblin(),
            MonsterType.Skeleton => new Skeleton(),
            MonsterType.Mage => new Mage(),
            MonsterType.Slime => new Slime(),
            _ => throw new ArgumentException($"Неизвестный тип монстра: {type}")
        };
    }

    public static Enemy CreateBoss(BossType type)
    {
        return type switch
        {
            BossType.VVG => new VVG(),
            BossType.Kovalsky => new Kovalsky(),
            BossType.ArchmageCpp => new ArchmageCpp(),
            BossType.PestovC => new PestovC(),
            _ => throw new ArgumentException($"Неизвестный тип босса: {type}")
        };
    }

    public static Enemy CreateRandomEnemy(Random random)
    {
        var types = Enum.GetValues<MonsterType>();
        var randomType = types[random.Next(types.Length)];
        return CreateEnemy(randomType);
    }

    public static Enemy CreateRandomBoss(Random random)
    {
        var types = Enum.GetValues<BossType>();
        var randomType = types[random.Next(types.Length)];
        return CreateBoss(randomType);
    }
}

public enum MonsterType
{
    Goblin,
    Skeleton,
    Mage,
    Slime
}

public enum BossType
{
    VVG,
    Kovalsky,
    ArchmageCpp,
    PestovC
}


