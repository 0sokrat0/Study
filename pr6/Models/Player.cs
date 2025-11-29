namespace RoguelikeGame.Models;

public class Player
{
    public int MaxHp { get; set; } = 100;
    public int CurrentHp { get; set; } = 100;
    public int BaseAttack { get; set; } = 10;
    public int BaseDefense { get; set; } = 5;
    public bool IsFrozen { get; set; } = false;
    public Weapon? Weapon { get; set; }
    public Armor? Armor { get; set; }

    public bool IsAlive()
    {
        return CurrentHp > 0;
    }

    public int GetAttack()
    {
        return BaseAttack + (Weapon?.Attack ?? 0);
    }

    public int GetDefense()
    {
        return BaseDefense + (Armor?.Defense ?? 0);
    }

    public void TakeDamage(int damage)
    {
        CurrentHp = Math.Max(0, CurrentHp - damage);
    }

    public void FullHeal()
    {
        CurrentHp = MaxHp;
    }
}

