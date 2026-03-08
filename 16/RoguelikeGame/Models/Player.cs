using RoguelikeGame.Models.Items;

namespace RoguelikeGame.Models
{
    public class Player
    {
        public int Hp { get; set; }
        public int MaxHp { get; private set; }
        public Weapon Weapon { get; set; }
        public Armor Armor { get; set; }
        public bool IsFrozen { get; set; }

        public Player()
        {
            MaxHp = 100;
            Hp = MaxHp;
            Weapon = new Weapon("Кулак", 5);
            Armor = new Armor("Одежда", 2);
            IsFrozen = false;
        }

        public bool IsAlive => Hp > 0;

        public double HpPercent => (double)Hp / MaxHp;
    }
}
