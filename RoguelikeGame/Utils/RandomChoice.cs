namespace RoguelikeGame.Utils;

public static class RandomChoice
{
    public static int NextInt(Random random, int min, int max)
    {
        return random.Next(min, max);
    }

    public static T ChooseFromList<T>(Random random, IList<T> items)
    {
        if (items == null || items.Count == 0)
        {
            throw new ArgumentException("Список не может быть пустым", nameof(items));
        }
        return items[random.Next(items.Count)];
    }

    public static bool NextBool(Random random)
    {
        return random.Next(2) == 1;
    }

    public static bool Chance(Random random, int percentage)
    {
        return random.Next(1, 101) <= percentage;
    }
}


