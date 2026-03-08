namespace RoguelikeGame.Models.Items
{
    public class Potion : Item
    {
        public Potion() : base("Зелье исцеления")
        {
        }

        public override string GetDescription() => "Зелье исцеления (Полностью восстанавливает HP)";
    }
}
