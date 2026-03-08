namespace RoguelikeGame.Models.Items
{
    public class Armor : Item
    {
        public int Defense { get; private set; }

        public Armor(string name, int defense) : base(name)
        {
            Defense = defense;
        }

        public override string GetDescription() => $"{Name} (DEF: {Defense})";
    }
}
