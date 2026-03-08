namespace RoguelikeGame.Models
{
    public class EnemyAttackResult
    {
        public int DamageDealt { get; set; }
        public bool IsCrit { get; set; }
        public bool IsFreeze { get; set; }
        public bool WasDodged { get; set; }
        public bool WasBlocked { get; set; }
    }
}
