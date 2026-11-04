using BattleSimulator.Models.Entities;

namespace BattleSimulator.Battle
{
    public class BattleState
    {
        public Character FighterA { get; }

        public Character FighterB { get; }

        public int FighterACurrentHealth { get; private set; }

        public int FighterBCurrentHealth { get; private set; }

        public int CurrentRound { get; private set; }

        public BattleState(Character fighterA, Character fighterB)
        {
            FighterA = fighterA ?? throw new ArgumentNullException(nameof(fighterA), "FighterA cannot be null.");
            FighterB = fighterB ?? throw new ArgumentNullException(nameof(fighterB), "FighterB cannot be null.");
            FighterACurrentHealth = FighterA.CurrentHealth;
            FighterBCurrentHealth = FighterB.CurrentHealth;
            CurrentRound = 1;
        }

        public void AdvanceRound()
        {
            CurrentRound++;
        }

        public void ApplyDamageToFighterA(int damage)
        {
            FighterACurrentHealth = CalculateRemainingHealth(FighterACurrentHealth, damage);
        }

        public void ApplyDamageToFighterB(int damage)
        {
            FighterBCurrentHealth = CalculateRemainingHealth(FighterBCurrentHealth, damage);
        }

        private static int CalculateRemainingHealth(int currentHealth, int damage)
        {
            if (damage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(damage), damage, "Damage cannot be negative.");
            }

            return Math.Max(currentHealth - damage, 0);
        }
    }
}
