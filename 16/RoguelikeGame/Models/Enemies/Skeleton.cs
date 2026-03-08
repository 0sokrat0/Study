namespace RoguelikeGame.Models.Enemies
{
    public class Skeleton : Enemy
    {
        protected double FreezeChance { get; set; }

        public Skeleton() : base("Скелет", 40, 10, 5, "SkeletonImage")
        {
            FreezeChance = 0.0;
        }

        protected Skeleton(string name, int hp, int attack, int defense, double freezeChance)
            : base(name, hp, attack, defense, "BossImage")
        {
            FreezeChance = freezeChance;
        }

        public override EnemyAttackResult PerformAttack(Player player, Random rng)
        {
            int dmg = Math.Max(1, Attack);
            bool freeze = FreezeChance > 0 && rng.NextDouble() < FreezeChance;

            return new EnemyAttackResult
            {
                DamageDealt = dmg,
                IsFreeze = freeze
            };
        }
    }
}
