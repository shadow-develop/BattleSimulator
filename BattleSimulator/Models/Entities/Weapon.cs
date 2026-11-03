namespace BattleSimulator.Models.Entities
{
    public class Weapon
    {
        public required string Name { get; set; }

        public required string WeaponType { get; set; }

        public required string DamageType { get; set; }

        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        public string ImagePath { get; set; } = string.Empty;
    }
}
