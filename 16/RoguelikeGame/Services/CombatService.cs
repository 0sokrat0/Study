using RoguelikeGame.Models;
using RoguelikeGame.Models.Enemies;

namespace RoguelikeGame.Services
{
    public class CombatService
    {
        private readonly Random _rng;

        public CombatService(Random rng)
        {
            _rng = rng;
        }

        public int PlayerAttack(Player player, Enemy enemy)
        {
            int damage = Math.Max(1, player.Weapon.Attack - enemy.Defense);
            enemy.Hp -= damage;
            return damage;
        }

        public void ApplyDamageToPlayer(Player player, int damage)
        {
            player.Hp = Math.Max(0, player.Hp - damage);
        }

        public void HealPlayer(Player player)
        {
            player.Hp = player.MaxHp;
        }

        public List<EnemyAttackResult> ProcessEnemyAttacks(Player player, List<Enemy> enemies, bool isDefending)
        {
            var results = new List<EnemyAttackResult>();

            foreach (var enemy in enemies)
            {
                if (!enemy.IsAlive) continue;

                var result = enemy.PerformAttack(player, _rng);

                if (isDefending)
                {
                    if (_rng.NextDouble() < 0.40)
                    {
                        result.DamageDealt = 0;
                        result.WasDodged = true;
                    }
                    else
                    {
                        double blockFactor = 0.70 + _rng.NextDouble() * 0.30;
                        int armorBlock = (int)(player.Armor.Defense * blockFactor);
                        result.DamageDealt = Math.Max(1, result.DamageDealt - armorBlock);
                        result.WasBlocked = true;
                    }
                }

                if (!result.WasDodged)
                    ApplyDamageToPlayer(player, result.DamageDealt);

                if (result.IsFreeze)
                    player.IsFrozen = true;

                results.Add(result);
            }

            return results;
        }
    }
}
