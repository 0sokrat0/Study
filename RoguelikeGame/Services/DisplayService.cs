using RoguelikeGame.Models;

namespace RoguelikeGame.Services;

public class DisplayService
{
    public void DisplayPlayerStatus(Player player)
    {
        Console.WriteLine($"❤️ Здоровье: {player.CurrentHp}/{player.MaxHp}");
        Console.WriteLine($"⚔️ Атака: {player.GetAttack()}");
        Console.WriteLine($"🛡️ Защита: {player.GetDefense()}");
        Console.WriteLine($"🗡️ Оружие: {(player.Weapon?.Name ?? "Нет")}");
        Console.WriteLine($"🛡️ Доспехи: {(player.Armor?.Name ?? "Нет")}");
    }

    public void DisplayGameOver(int turnCount)
    {
        Console.WriteLine();
        Console.WriteLine("💀 ИГРА ОКОНЧЕНА 💀");
        Console.WriteLine($"Вы продержались {turnCount} ходов");
        Console.WriteLine("Спасибо за игру!");
    }

    public void DisplayWelcome()
    {
        Console.WriteLine("=== ТЕКСТОВАЯ ИГРА-РОГАЛИК ===");
        Console.WriteLine("Добро пожаловать в подземелье!");
        Console.WriteLine("На каждом ходу вас ждет либо сундук с сокровищами, либо опасный враг.");
        Console.WriteLine("Каждые 10 ходов вас ждет встреча с боссом!");
        Console.WriteLine();
    }
}

