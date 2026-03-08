namespace RoguelikeGame.Models.Items
{
    public class Weapon : Item
    {
        public int Attack { get; private set; }

        public Weapon(string name, int attack) : base(name)
        {
            Attack = attack;
        }

        public override string GetDescription() => $"{Name} (ATK: {Attack})";
    }
}
