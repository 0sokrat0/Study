using RoguelikeGame.Models;
using RoguelikeGame.Utils;

namespace RoguelikeGame.Services;

public class ChestService
{
    private readonly Random _random;

    public ChestService(Random random)
    {
        _random = random;
    }

    public void OpenChest(Player player)
    {
        var item = GetRandomItem();
        
        if (item is HealthPotion potion)
        {
            Console.WriteLine($"Вы получили: {potion.Name}");
            player.FullHeal();
            Console.WriteLine("Ваше здоровье полностью восстановлено!");
        }
        else if (item is Weapon weapon)
        {
            Console.WriteLine($"Вы получили оружие: {weapon.Name}");
            Console.WriteLine($"Атака: {weapon.Attack}, Крит. шанс: {weapon.CriticalChance}%");
            
            if (player.Weapon != null)
            {
                Console.WriteLine($"Текущее оружие: {player.Weapon.Name} (Атака: {player.Weapon.Attack})");
                Console.WriteLine("Заменить оружие? (y/n)");
                
                if (Console.ReadLine()?.ToLower() == "y")
                {
                    player.Weapon = weapon;
                    Console.WriteLine($"Оружие заменено на {weapon.Name}!");
                }
                else
                {
                    Console.WriteLine("Оружие выброшено.");
                }
            }
            else
            {
                player.Weapon = weapon;
                Console.WriteLine($"Оружие экипировано: {weapon.Name}!");
            }
        }
        else if (item is Armor armor)
        {
            Console.WriteLine($"Вы получили доспехи: {armor.Name}");
            Console.WriteLine($"Защита: {armor.Defense}");
            
            if (player.Armor != null)
            {
                Console.WriteLine($"Текущие доспехи: {player.Armor.Name} (Защита: {player.Armor.Defense})");
                Console.WriteLine("Заменить доспехи? (y/n)");
                
                if (Console.ReadLine()?.ToLower() == "y")
                {
                    player.Armor = armor;
                    Console.WriteLine($"Доспехи заменены на {armor.Name}!");
                }
                else
                {
                    Console.WriteLine("Доспехи выброшены.");
                }
            }
            else
            {
                player.Armor = armor;
                Console.WriteLine($"Доспехи экипированы: {armor.Name}!");
            }
        }
    }

    private Item GetRandomItem()
    {
        int itemType = _random.Next(1, 4);
        
        return itemType switch
        {
            1 => new HealthPotion(),
            2 => GetRandomWeapon(),
            3 => GetRandomArmor(),
            _ => new HealthPotion()
        };
    }

    private Weapon GetRandomWeapon()
    {
        var weapons = new List<Weapon>
        {
            new("Ржавый меч", 12, 5),
            new("Боевой топор", 18, 15),
            new("Магический посох", 15, 25),
            new("Кинжал убийцы", 20, 30),
            new("Легендарный клинок", 25, 40)
        };
        
        return RandomChoice.ChooseFromList(_random, weapons);
    }

    private Armor GetRandomArmor()
    {
        var armors = new List<Armor>
        {
            new("Кожаная броня", 8),
            new("Кольчуга", 12),
            new("Латная броня", 18),
            new("Магические доспехи", 15),
            new("Легендарная броня", 25)
        };
        
        return RandomChoice.ChooseFromList(_random, armors);
    }
}


