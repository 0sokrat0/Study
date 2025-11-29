using RoguelikeGame.Models;
using RoguelikeGame.Factories;
using RoguelikeGame.Utils;

namespace RoguelikeGame.Services;

public class GameService
{
    private readonly Random _random;
    private readonly Player _player;
    private readonly CombatService _combatService;
    private readonly ChestService _chestService;
    private readonly DisplayService _displayService;
    private int _turnCount;

    public GameService()
    {
        _random = new Random();
        _player = new Player();
        _combatService = new CombatService(_random);
        _chestService = new ChestService(_random);
        _displayService = new DisplayService();
        _turnCount = 0;
    }

    public void StartGame()
    {
        _displayService.DisplayWelcome();

        while (_player.IsAlive())
        {
            _turnCount++;
            Console.WriteLine($"ХОД {_turnCount}");
            _displayService.DisplayPlayerStatus(_player);
            Console.WriteLine();

            if (_turnCount % 10 == 0)
            {
                Console.WriteLine("ВСТРЕЧА С БОССОМ!");
                var boss = MonsterFactory.CreateRandomBoss(_random);
                if (!_combatService.FightBoss(_player, boss))
                {
                    _displayService.DisplayGameOver(_turnCount);
                    return;
                }
            }
            else
            {
                if (RandomChoice.NextInt(_random, 1, 3) == 1)
                {
                    Console.WriteLine("Вы нашли сундук!");
                    _chestService.OpenChest(_player);
                }
                else
                {
                    Console.WriteLine("Встреча с врагом!");
                    var enemy = MonsterFactory.CreateRandomEnemy(_random);
                    if (!_combatService.FightEnemy(_player, enemy))
                    {
                        _displayService.DisplayGameOver(_turnCount);
                        return;
                    }
                }
            }

            if (_player.IsFrozen)
            {
                Console.WriteLine("Вы заморожены и пропускаете ход!");
                _player.IsFrozen = false;
            }

            Console.WriteLine();
            Console.WriteLine("Нажмите Enter для продолжения...");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
