using System.ComponentModel;
using RoguelikeGame.Models.Enemies;

namespace RoguelikeGame.Views;

public class EnemyCardViewModel : INotifyPropertyChanged
{
    private readonly Enemy _enemy;

    public event PropertyChangedEventHandler? PropertyChanged;

    public EnemyCardViewModel(Enemy enemy)
    {
        _enemy = enemy;
    }

    public string Name => _enemy.Name;

    public string HpText => $"{Math.Max(0, _enemy.Hp)} / {_enemy.MaxHp}";

    public double HpBarWidth => Math.Max(0, _enemy.HpPercent) * 100.0;

    public string EnemyEmoji => _enemy switch
    {
        Boss => "👑",
        Mage => "🧙",
        Skeleton => "💀",
        _ => "👺"
    };

    public void Refresh()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HpText)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HpBarWidth)));
    }
}
