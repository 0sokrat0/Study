using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using RoguelikeGame.Models;
using RoguelikeGame.Models.Enemies;
using RoguelikeGame.Models.Items;
using RoguelikeGame.Services;

namespace RoguelikeGame.Views;

public partial class GameView : UserControl
{
    public event EventHandler<int>? GameOver;

    private readonly Random _rng = new();
    private readonly Player _player;
    private readonly FloorService _floorService;
    private readonly CombatService _combatService;
    private readonly LootService _lootService;
    private readonly EventLogService _logService;

    private int _currentFloor = 0;
    private List<Enemy> _currentEnemies = new();
    private bool _inCombat = false;
    private bool _playerDefending = false;
    private FloorEvent _currentEvent;

    private readonly ObservableCollection<EnemyCardViewModel> _enemyCards = new();
    private readonly ObservableCollection<string> _logEntries = new();

    public GameView()
    {
        InitializeComponent();

        _player = new Player();
        _floorService = new FloorService(_rng);
        _combatService = new CombatService(_rng);
        _lootService = new LootService(_rng);
        _logService = new EventLogService();

        EnemyCardsPanel.ItemsSource = _enemyCards;
        LogItemsControl.ItemsSource = _logEntries;

        UpdatePlayerUI();

        Loaded += async (_, _) => await AdvanceToNextFloor();
    }

    private async Task AdvanceToNextFloor()
    {
        _currentFloor++;
        FloorText.Text = $"Этаж: {_currentFloor}";
        _currentEvent = _floorService.GetFloorEvent(_currentFloor);
        _playerDefending = false;

        NextFloorButton.IsVisible = false;
        FrozenStatusText.IsVisible = false;

        if (_currentEvent == FloorEvent.Boss)
            StartBossFight();
        else if (_currentEvent == FloorEvent.Enemy)
            StartEnemyFight();
        else
            ShowChest();
    }

    private void StartEnemyFight()
    {
        _currentEnemies = _floorService.GenerateEnemies();
        _inCombat = true;
        _logService.Add($"--- Этаж {_currentFloor}: Враги! ---");

        EventTitleText.Text = $"Этаж {_currentFloor} — Бой!";
        ChestText.IsVisible = false;
        EnemyCardsPanel.IsVisible = true;
        RebuildEnemyCards();
        ShowCombatButtons(true);
        RefreshLog();
    }

    private void StartBossFight()
    {
        var boss = _floorService.GenerateBoss();
        _currentEnemies = new List<Enemy> { boss };
        _inCombat = true;
        _logService.Add($"--- Этаж {_currentFloor}: БОСС — {boss.Name}! ---");

        EventTitleText.Text = $"Этаж {_currentFloor} — БОСС: {boss.Name}!";
        ChestText.IsVisible = false;
        EnemyCardsPanel.IsVisible = true;
        RebuildEnemyCards();
        ShowCombatButtons(true);
        RefreshLog();
    }

    private void ShowChest()
    {
        _inCombat = false;
        _currentEnemies.Clear();
        _enemyCards.Clear();
        _logService.Add($"--- Этаж {_currentFloor}: Сундук! ---");

        EventTitleText.Text = $"Этаж {_currentFloor} — Сундук";
        ChestText.IsVisible = true;
        EnemyCardsPanel.IsVisible = false;
        ShowCombatButtons(false);
        OpenChestButton.IsVisible = true;
        RefreshLog();
    }

    private async void OnOpenChestClick(object? sender, RoutedEventArgs e)
    {
        OpenChestButton.IsVisible = false;

        var item = _lootService.GenerateRandomItem();
        _logService.Add($"В сундуке найдено: {item.GetDescription()}");

        if (item is Potion)
        {
            _combatService.HealPlayer(_player);
            _logService.Add("Вы выпили зелье и полностью восстановили HP!");
            UpdatePlayerUI();
        }
        else if (item is Weapon newWeapon)
        {
            var window = (Window)TopLevel.GetTopLevel(this)!;
            var dialog = new ItemPickupDialog(_player.Weapon, newWeapon);
            bool took = await dialog.ShowDialog<bool>(window);
            if (took)
            {
                _player.Weapon = newWeapon;
                _logService.Add($"Вы взяли {newWeapon.GetDescription()}");
                UpdatePlayerUI();
            }
            else
            {
                _logService.Add("Вы выбросили новое оружие.");
            }
        }
        else if (item is Armor newArmor)
        {
            var window = (Window)TopLevel.GetTopLevel(this)!;
            var dialog = new ItemPickupDialog(_player.Armor, newArmor);
            bool took = await dialog.ShowDialog<bool>(window);
            if (took)
            {
                _player.Armor = newArmor;
                _logService.Add($"Вы взяли {newArmor.GetDescription()}");
                UpdatePlayerUI();
            }
            else
            {
                _logService.Add("Вы выбросили новую броню.");
            }
        }

        NextFloorButton.IsVisible = true;
        RefreshLog();
    }

    private void OnAttackClick(object? sender, RoutedEventArgs e)
    {
        if (!_inCombat) return;
        if (_player.IsFrozen)
        {
            ProcessFrozenTurn();
            return;
        }

        _playerDefending = false;
        PlayerAttackPhase();

        if (CheckAllEnemiesDead()) return;

        EnemyAttackPhase();
    }

    private void OnDefendClick(object? sender, RoutedEventArgs e)
    {
        if (!_inCombat) return;
        if (_player.IsFrozen)
        {
            ProcessFrozenTurn();
            return;
        }

        _playerDefending = true;
        _logService.Add("Вы принимаете защитную стойку.");
        EnemyAttackPhase();
    }

    private void ProcessFrozenTurn()
    {
        _player.IsFrozen = false;
        FrozenStatusText.IsVisible = false;
        _logService.Add("Вы заморожены! Пропускаете свой ход.");
        EnemyAttackPhase();
    }

    private void PlayerAttackPhase()
    {
        var aliveEnemies = _currentEnemies.Where(e => e.IsAlive).ToList();
        if (aliveEnemies.Count == 0) return;

        var target = aliveEnemies[0];
        int dmg = _combatService.PlayerAttack(_player, target);
        _logService.Add($"Вы атакуете {target.Name} и наносите {dmg} урона.");

        if (!target.IsAlive)
            _logService.Add($"{target.Name} повержен!");

        RefreshEnemyCards();
    }

    private void EnemyAttackPhase()
    {
        var results = _combatService.ProcessEnemyAttacks(_player, _currentEnemies, _playerDefending);
        _playerDefending = false;

        var aliveEnemies = _currentEnemies.Where(e => e.IsAlive).ToList();
        for (int i = 0; i < aliveEnemies.Count && i < results.Count; i++)
            BuildEnemyAttackLog(aliveEnemies[i], results[i]);

        UpdatePlayerUI();

        if (_player.IsFrozen)
            FrozenStatusText.IsVisible = true;

        RefreshLog();

        if (!_player.IsAlive)
        {
            EndGame();
            return;
        }

        CheckAllEnemiesDead();
    }

    private void BuildEnemyAttackLog(Enemy enemy, EnemyAttackResult result)
    {
        if (result.WasDodged)
        {
            _logService.Add($"{enemy.Name} атакует, но вы уклонились!");
            return;
        }

        string extra = "";
        if (result.IsCrit) extra += " [КРИТ]";
        if (result.IsFreeze) extra += " [ЗАМОРОЖЕН]";
        if (result.WasBlocked) extra += " [заблокировано]";

        _logService.Add($"{enemy.Name} наносит вам {result.DamageDealt} урона.{extra}");
    }

    private bool CheckAllEnemiesDead()
    {
        if (_currentEnemies.All(e => !e.IsAlive))
        {
            _inCombat = false;
            _logService.Add("Все враги повержены! Путь открыт.");
            RefreshLog();
            ShowCombatButtons(false);
            NextFloorButton.IsVisible = true;
            return true;
        }
        return false;
    }

    private async void OnNextFloorClick(object? sender, RoutedEventArgs e)
    {
        await AdvanceToNextFloor();
    }

    private void EndGame()
    {
        _inCombat = false;
        _logService.Add("Вы погибли...");
        ShowCombatButtons(false);
        RefreshLog();
        GameOver?.Invoke(this, _currentFloor);
    }

    private void RebuildEnemyCards()
    {
        _enemyCards.Clear();
        foreach (var enemy in _currentEnemies)
            _enemyCards.Add(new EnemyCardViewModel(enemy));
    }

    private void RefreshEnemyCards()
    {
        foreach (var card in _enemyCards)
            card.Refresh();
    }

    private void UpdatePlayerUI()
    {
        double pct = _player.HpPercent;
        PlayerHpBar.Width = Math.Max(0, pct * 220);

        if (pct > 0.6)
            PlayerHpBar.Background = new SolidColorBrush(Color.FromRgb(0x22, 0xCC, 0x22));
        else if (pct > 0.3)
            PlayerHpBar.Background = new SolidColorBrush(Color.FromRgb(0xCC, 0xAA, 0x00));
        else
            PlayerHpBar.Background = new SolidColorBrush(Color.FromRgb(0xCC, 0x22, 0x22));

        PlayerHpText.Text = $"{Math.Max(0, _player.Hp)} / {_player.MaxHp}";
        WeaponText.Text = _player.Weapon.GetDescription();
        ArmorText.Text = _player.Armor.GetDescription();
    }

    private void ShowCombatButtons(bool show)
    {
        CombatButtonsPanel.IsVisible = show;
    }

    private void RefreshLog()
    {
        _logEntries.Clear();
        foreach (var entry in _logService.GetLast(20))
            _logEntries.Add(entry);

        Dispatcher.UIThread.Post(() => LogScrollViewer.ScrollToEnd());
    }
}
