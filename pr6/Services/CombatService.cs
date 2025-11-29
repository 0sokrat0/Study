using RoguelikeGame.Models;
using RoguelikeGame.Utils;

namespace RoguelikeGame.Services;

public class CombatService
{
    private readonly Random _random;
    private bool _isDefending = false;

    public CombatService(Random random)
    {
        _random = random;
    }

    public bool FightEnemy(Player player, Enemy enemy)
    {
        Console.WriteLine($"Встречен враг: {enemy.Name}");
        Console.WriteLine($"Здоровье врага: {enemy.CurrentHp}");
        Console.WriteLine();

        while (player.IsAlive() && enemy.IsAlive())
        {
            if (player.IsFrozen)
            {
                Console.WriteLine("Вы заморожены и не можете действовать!");
                player.IsFrozen = false;
            }
            else
            {
                PlayerTurn(player, enemy);
            }

            if (!enemy.IsAlive())
            {
                Console.WriteLine($"Вы победили {enemy.Name}!");
                return true;
            }

            EnemyTurn(player, enemy);
        }

        return player.IsAlive();
    }

    public bool FightBoss(Player player, Enemy boss)
    {
        Console.WriteLine($"ВСТРЕЧА С БОССОМ: {boss.Name}");
        Console.WriteLine($"Здоровье босса: {boss.CurrentHp}");
        Console.WriteLine();

        while (player.IsAlive() && boss.IsAlive())
        {
            if (player.IsFrozen)
            {
                Console.WriteLine("Вы заморожены и не можете действовать!");
                player.IsFrozen = false;
            }
            else
            {
                PlayerTurn(player, boss);
            }

            if (!boss.IsAlive())
            {
                Console.WriteLine($"ВЫ ПОБЕДИЛИ БОССА {boss.Name}!");
                return true;
            }

            EnemyTurn(player, boss);
        }

        return player.IsAlive();
    }

    private void PlayerTurn(Player player, Enemy enemy)
    {
        Console.WriteLine("Выберите действие:");
        Console.WriteLine("1. Атаковать");
        Console.WriteLine("2. Защищаться");

        while (true)
        {
            var choice = Console.ReadLine();
            if (choice == "1")
            {
                AttackEnemy(player, enemy);
                _isDefending = false;
                break;
            }
            else if (choice == "2")
            {
                Defend();
                break;
            }
            else
            {
                Console.WriteLine("Введите 1 или 2");
            }
        }
    }

    private void AttackEnemy(Player player, Enemy enemy)
    {
        int damage = player.GetAttack();
        Console.WriteLine($"Вы атакуете на {damage} урона!");
        enemy.TakeDamage(damage);
        Console.WriteLine($"У врага осталось {enemy.CurrentHp} здоровья");
    }

    private void Defend()
    {
        Console.WriteLine("Вы принимаете защитную стойку!");
        _isDefending = true;
    }

    private void EnemyTurn(Player player, Enemy enemy)
    {
        int damage = enemy.AttackPlayer(player, _random);
        
        if (player.IsFrozen)
        {
            return;
        }

        int finalDamage = CalculateDamage(damage, enemy, player);
        if (finalDamage > 0)
        {
            player.TakeDamage(finalDamage);
            Console.WriteLine($"Вы получаете {finalDamage} урона!");
            Console.WriteLine($"У вас осталось {player.CurrentHp} здоровья");
        }
        else
        {
            Console.WriteLine("Вы уклонились от атаки!");
        }
    }

    private int CalculateDamage(int baseDamage, Enemy enemy, Player player)
    {
        if (_isDefending)
        {
            if (RandomChoice.Chance(_random, 40))
            {
                Console.WriteLine("Вы полностью уклонились от атаки!");
                _isDefending = false;
                return 0;
            }
            else
            {
                Console.WriteLine("Уклонение не удалось, но защита сработала!");
                if (enemy.IgnoresDefense)
                {
                    Console.WriteLine("Но враг игнорирует защиту!");
                    _isDefending = false;
                    return baseDamage;
                }
                int defense = player.GetDefense();
                int blockChanceDefend = _random.Next(70, 101);
                int blockAmountDefend = (int)(defense * blockChanceDefend / 100.0);
                _isDefending = false;
                return Math.Max(0, baseDamage - blockAmountDefend);
            }
        }

        if (enemy.IgnoresDefense)
        {
            return baseDamage;
        }

        int playerDefense = player.GetDefense();
        int blockChance = _random.Next(70, 101);
        int blockAmount = (int)(playerDefense * blockChance / 100.0);
        
        return Math.Max(0, baseDamage - blockAmount);
    }
}

