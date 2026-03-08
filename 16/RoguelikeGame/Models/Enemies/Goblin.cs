namespace RoguelikeGame.Models.Enemies
{
    public class Goblin : Enemy
    {
        protected double CritChance { get; set; }

        public Goblin() : base("Гоблин", 30, 12, 3, "GoblinImage")
        {
            CritChance = 0.20;
        }

        protected Goblin(string name, int hp, int attack, int defense, double critChance)
            : base(name, hp, attack, defense, "BossImage")
        {
            CritChance = critChance;
        }

        public override EnemyAttackResult PerformAttack(Player player, Random rng)
        {
            bool isCrit = rng.NextDouble() < CritChance;
            int dmg = Math.Max(1, Attack - player.Armor.Defense);
            if (isCrit) dmg *= 2;

            return new EnemyAttackResult
            {
                DamageDealt = dmg,
                IsCrit = isCrit
            };
        }
    }
}
