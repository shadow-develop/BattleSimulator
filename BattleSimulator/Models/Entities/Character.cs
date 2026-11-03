namespace BattleSimulator.Models.Entities
{
    public class Character
    {
        public int Id { get; set; }

        public string Name { get; }

        public int MaxHealth { get; }

        public int CurrentHealth { get; private set; }

        public int Accuracy { get; }

        public int Evasion { get; }

        public string ImagePath { get; set; } = string.Empty;

        public Weapon EquippedWeapon { get; }

        public Character(string name, int maxHealth, int accuracy, int evasion, Weapon equippedWeapon)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
            } else if (name.Length < 3 || name.Length > 50)
            {
                throw new ArgumentException("Name must be between 3 and 50 characters long.", nameof(name));
            }
            Name = name;

            if(maxHealth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxHealth), "MaxHealth must be greater than zero.");
            }
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;

            if (accuracy <= 0) {
                throw new ArgumentOutOfRangeException(nameof(accuracy), "Accuracy must be greater than zero.");
            }
            Accuracy = accuracy;

            if (evasion <= 0) {
                throw new ArgumentOutOfRangeException(nameof(evasion), "Evasion must be greater than zero.");
            }
            Evasion = evasion;

            EquippedWeapon = equippedWeapon ?? throw new ArgumentNullException(nameof(equippedWeapon), "EquippedWeapon cannot be null.");
        }
    }
}
