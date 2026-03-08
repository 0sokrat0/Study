namespace RoguelikeGame.Models.Enemies
{
    public class Mage : Enemy
    {
        protected double FreezeChance { get; set; }

        public Mage() : base("Маг", 25, 15, 2, "MageImage")
        {
            FreezeChance = 0.15;
        }

        protected Mage(string name, int hp, int attack, int defense, double freezeChance)
            : base(name, hp, attack, defense, "BossImage")
        {
            FreezeChance = freezeChance;
        }

        public override EnemyAttackResult PerformAttack(Player player, Random rng)
        {
            bool freeze = rng.NextDouble() < FreezeChance;
            int dmg = Math.Max(1, Attack - player.Armor.Defense);

            return new EnemyAttackResult
            {
                DamageDealt = dmg,
                IsFreeze = freeze
            };
        }
    }
}
