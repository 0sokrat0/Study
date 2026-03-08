namespace RoguelikeGame.Models.Enemies
{
    public abstract class Enemy
    {
        public string Name { get; protected set; }
        public int Hp { get; set; }
        public int MaxHp { get; protected set; }
        public int Attack { get; protected set; }
        public int Defense { get; protected set; }
        public string ImageKey { get; protected set; }

        protected Enemy(string name, int hp, int attack, int defense, string imageKey)
        {
            Name = name;
            MaxHp = hp;
            Hp = hp;
            Attack = attack;
            Defense = defense;
            ImageKey = imageKey;
        }

        public bool IsAlive => Hp > 0;
        public double HpPercent => (double)Hp / MaxHp;

        public abstract EnemyAttackResult PerformAttack(Player player, Random rng);

        protected int CalculateBaseDamage(int playerDefense)
        {
            return Math.Max(1, Attack - playerDefense);
        }
    }
}
