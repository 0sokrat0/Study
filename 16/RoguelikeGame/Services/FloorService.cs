using RoguelikeGame.Models;
using RoguelikeGame.Models.Enemies;

namespace RoguelikeGame.Services
{
    public class FloorService
    {
        private readonly Random _rng;

        public FloorService(Random rng)
        {
            _rng = rng;
        }

        public FloorEvent GetFloorEvent(int floor)
        {
            if (floor % 10 == 0) return FloorEvent.Boss;
            return _rng.NextDouble() < 0.5 ? FloorEvent.Enemy : FloorEvent.Chest;
        }

        public List<Enemy> GenerateEnemies()
        {
            int count = _rng.Next(1, 4);
            var enemies = new List<Enemy>();
            for (int i = 0; i < count; i++)
                enemies.Add(CreateRandomEnemy());
            return enemies;
        }

        public Boss GenerateBoss()
        {
            int index = _rng.Next(4);
            return index switch
            {
                0 => CreateBossVVG(),
                1 => CreateBossKovalsky(),
                2 => CreateBossArchmage(),
                _ => CreateBossPestov()
            };
        }

        private Enemy CreateRandomEnemy()
        {
            int type = _rng.Next(3);
            return type switch
            {
                0 => new Goblin(),
                1 => new Skeleton(),
                _ => new Mage()
            };
        }

        private Boss CreateBossVVG()
        {
            int hp = (int)(30 * 2.0);
            int atk = (int)(12 * 1.5);
            int def = (int)(3 * 1.2);
            return new Boss("ВВГ", hp, atk, def, 0.30, 0.0, false);
        }

        private Boss CreateBossKovalsky()
        {
            int hp = (int)(40 * 2.5);
            int atk = (int)(10 * 1.3);
            int def = (int)(5 * 1.4);
            return new Boss("Ковальский", hp, atk, def, 0.0, 0.0, true);
        }

        private Boss CreateBossArchmage()
        {
            int hp = (int)(25 * 1.8);
            int atk = (int)(15 * 1.6);
            int def = (int)(2 * 1.1);
            return new Boss("Архимаг C++", hp, atk, def, 0.0, 0.25, false);
        }

        private Boss CreateBossPestov()
        {
            int hp = (int)(40 * 1.3);
            int atk = (int)(10 * 1.8);
            int def = (int)(5 * 0.6);
            return new Boss("Пестов С--", hp, atk, def, 0.0, 0.15, true);
        }
    }
}
