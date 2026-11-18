using BattleSimulator.Battle.Enums;
using BattleSimulator.Models.Entities;

namespace BattleSimulator.Battle
{
    public class AttackResult
    {
        public int AttackerId { get; }

        public int DefenderId { get; }

        public AttackOutcome Outcome { get; }

        public int BaseDamage { get; }

        public int FinalDamage { get; }

        public int DefenderRemainingHealth { get; }

        public AttackResult(Character attacker, Character defender, AttackOutcome outcome, int baseDamage, int finalDamage, int defenderRemainingHealth)
        {
            AttackerId = attacker?.Id ?? throw new ArgumentNullException(nameof(attacker), "Attacker cannot be null.");
            DefenderId = defender?.Id ?? throw new ArgumentNullException(nameof(defender), "Defender cannot be null.");

            if (!Enum.IsDefined(typeof(AttackOutcome), outcome))
            {
                throw new ArgumentException("Invalid attack outcome.", nameof(outcome));
            }
            Outcome = outcome;

            ValidateNumbers(baseDamage, finalDamage, defenderRemainingHealth);
            BaseDamage = baseDamage;
            FinalDamage = finalDamage;
            DefenderRemainingHealth = defenderRemainingHealth;
        }

        private static void ValidateNumbers(int baseDamage, int finalDamage, int defenderRemainingHealth)
        {
            if (baseDamage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(baseDamage), baseDamage, "Base damage cannot be negative.");
            }
            if (finalDamage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(finalDamage), finalDamage, "Final damage cannot be negative.");
            }
            if (defenderRemainingHealth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(defenderRemainingHealth), defenderRemainingHealth, "Defender remaining health cannot be negative.");
            }
        }
    }
}
