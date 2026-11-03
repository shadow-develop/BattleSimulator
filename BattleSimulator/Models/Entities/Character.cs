namespace BattleSimulator.Models.Entities
{
    public class Character
    {
        public int Id { get; set; }

        public int MaxHealth { get; set; }

        public int CurrentHealth { get; set; }

        public int Accuracy { get; set; }

        public int Evasion { get; set; }

        public string ImagePath { get; set; } = string.Empty;

        public required Weapon EquippedWeapon { get; set; }
    }
}
