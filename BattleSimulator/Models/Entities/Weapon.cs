using BattleSimulator.Models.Enums;

namespace BattleSimulator.Models.Entities
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; }

        public WeaponType WeaponType { get; }

        public DamageType DamageType { get; }

        public int MaxDamage { get; }

        public int MinDamage { get; }

        public string ImagePath { get; set; } = string.Empty;

        private const int DamageCap = 1_000;

        public Weapon(string name, WeaponType weaponType, int maxDamage, int minDamage)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
            }
            else if (name.Length < 3 || name.Length > 50)
            {
                throw new ArgumentException("Name must be between 3 and 50 characters long.", nameof(name));
            }
            Name = name;

            WeaponType = weaponType;

            DamageType = weaponType switch
            {
                WeaponType.Sword or WeaponType.Axe => DamageType.Slashing,
                WeaponType.Spear => DamageType.Piercing,
                WeaponType.Hammer or WeaponType.Mace => DamageType.Blunt,
                _ => throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, "Choose a supported weapon type.")
            };

            if (maxDamage <= 0 || maxDamage > DamageCap)
            {
                throw new ArgumentOutOfRangeException(nameof(maxDamage), $"Maximum damage must be between 1 and {DamageCap}.");
            }
            MaxDamage = maxDamage;

            if (minDamage <= 0 || minDamage > maxDamage)
            {
                throw new ArgumentOutOfRangeException(nameof(minDamage), $"Minimum damage must be between 1 and {maxDamage}.");
            }
            MinDamage = minDamage;
        }
    }
}
