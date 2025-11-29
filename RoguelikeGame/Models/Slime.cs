namespace RoguelikeGame.Models;

public class Slime : Enemy
{
    private const int DamageReduction = 2;

    public Slime() : base("Слизень", 45, 12, 5)
    {
    }

    public override void TakeDamage(int damage)
    {
        int reducedDamage = Math.Max(0, damage - DamageReduction);
        if (reducedDamage < damage)
        {
            Console.WriteLine($"{Name} уменьшает входящий урон на {DamageReduction}! Урон: {damage} → {reducedDamage}");
        }
        base.TakeDamage(reducedDamage);
    }

    public override int AttackPlayer(Player player, Random random)
    {
        int damage = Attack;
        Console.WriteLine($"{Name} атакует с уроном: {damage}");
        return damage;
    }
}

