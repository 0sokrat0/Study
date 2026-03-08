namespace RoguelikeGame.Models.Enemies
{
    public class Boss : Enemy
    {
        private readonly double _critChance;
        private readonly double _freezeChance;
        private readonly bool _ignoresArmor;

        public Boss(string name, int hp, int attack, int defense,
                    double critChance, double freezeChance, bool ignoresArmor)
            : base(name, hp, attack, defense, "BossImage")
        {
            _critChance = critChance;
            _freezeChance = freezeChance;
            _ignoresArmor = ignoresArmor;
        }

        public override EnemyAttackResult PerformAttack(Player player, Random rng)
        {
            bool isCrit = _critChance > 0 && rng.NextDouble() < _critChance;
            bool isFreeze = _freezeChance > 0 && rng.NextDouble() < _freezeChance;

            int baseDmg;
            if (_ignoresArmor)
            {
                baseDmg = Math.Max(1, Attack);
            }
            else
            {
                baseDmg = Math.Max(1, Attack - player.Armor.Defense);
            }

            if (isCrit) baseDmg *= 2;

            return new EnemyAttackResult
            {
                DamageDealt = baseDmg,
                IsCrit = isCrit,
                IsFreeze = isFreeze
            };
        }
    }
}
