namespace RoguelikeGame.Models.Items
{
    public abstract class Item
    {
        public string Name { get; protected set; }

        protected Item(string name)
        {
            Name = name;
        }

        public abstract string GetDescription();
    }
}
