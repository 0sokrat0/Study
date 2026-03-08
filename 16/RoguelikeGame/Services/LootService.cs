using RoguelikeGame.Models.Items;

namespace RoguelikeGame.Services
{
    public class LootService
    {
        private readonly Random _rng;

        private readonly List<Weapon> _weapons = new()
        {
            new Weapon("Кинжал", 8),
            new Weapon("Меч", 14),
            new Weapon("Топор", 18),
            new Weapon("Молот", 22),
            new Weapon("Магический посох", 16)
        };

        private readonly List<Armor> _armors = new()
        {
            new Armor("Кожаный доспех", 5),
            new Armor("Кольчуга", 9),
            new Armor("Латный доспех", 14),
            new Armor("Мантия мага", 7)
        };

        public LootService(Random rng)
        {
            _rng = rng;
        }

        public Item GenerateRandomItem()
        {
            int roll = _rng.Next(3);
            return roll switch
            {
                0 => GetRandomWeapon(),
                1 => GetRandomArmor(),
                _ => new Potion()
            };
        }

        public Weapon GetRandomWeapon()
        {
            var w = _weapons[_rng.Next(_weapons.Count)];
            return new Weapon(w.Name, w.Attack);
        }

        public Armor GetRandomArmor()
        {
            var a = _armors[_rng.Next(_armors.Count)];
            return new Armor(a.Name, a.Defense);
        }
    }
}
