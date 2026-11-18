using BattleSimulator.Battle.Enums;
using BattleSimulator.Models.Entities;

namespace BattleSimulator.Battle.Services
{
    public class BattleService : IBattleService
    {
        private const double MinHitChance = 0.20;
        private const double MaxHitChance = 0.75;
        private const double MinDodgeChance = 0.05;
        private const double MaxDodgeChance = 0.40;
        private const double PercentageScale = 100.0;
        private const int MaxRounds = 50;

        public BattleResult SimulateBattle(Character fighterA, Character fighterB)
        {
            var battleState = new BattleState(fighterA, fighterB);

            if (fighterA.Id == fighterB.Id)
            {
                throw new ArgumentException("Fighters must have distinct IDs.", nameof(fighterB));
            }

            var roundsResults = new List<RoundResult>();

            var fighterAHitChance = CalculateHitChance(fighterA);
            var fighterADodgeChance = CalculateDodgeChance(fighterA);

            var fighterBHitChance = CalculateHitChance(fighterB);
            var fighterBDodgeChance = CalculateDodgeChance(fighterB);

            while (CanContinueBattle(battleState))
            {
                var (attacker, defender) = RollAttacker(fighterA, fighterB);

                bool aAttacksFirst = ReferenceEquals(attacker, battleState.FighterA);

                double firstHitChance = aAttacksFirst ? fighterAHitChance : fighterBHitChance;
                double firstDodgeChance = aAttacksFirst ? fighterBDodgeChance : fighterADodgeChance;

                double returnHitChance = aAttacksFirst ? fighterBHitChance : fighterAHitChance;
                double returnDodgeChance = aAttacksFirst ? fighterADodgeChance : fighterBDodgeChance;
                var attackResults = new List<AttackResult>();

                var firstAttack = ResolveAttack(battleState, attacker, firstHitChance, defender, firstDodgeChance);
                attackResults.Add(firstAttack);

                if (firstAttack.DefenderRemainingHealth > 0)
                {
                    attackResults.Add(ResolveAttack(battleState, defender, returnHitChance, attacker, returnDodgeChance));
                }

                roundsResults.Add(new RoundResult(battleState.CurrentRound, attackResults));

                if (battleState.FighterACurrentHealth == 0 ||
                    battleState.FighterBCurrentHealth == 0 ||
                    battleState.CurrentRound >= MaxRounds)
                {
                    break;
                }

                battleState.AdvanceRound();
            }

            bool someoneDied = battleState.FighterACurrentHealth == 0 || battleState.FighterBCurrentHealth == 0;
            var endingReason = someoneDied ? EndingReason.Death : EndingReason.RoundLimitReached;
            var battleOutcome = BattleOutcome.Draw;
            int? winnerId = null;


            if (endingReason == EndingReason.RoundLimitReached)
            {
                double fighterARemainingHealthPercentage = CalculateRemainingHealthPercentage(battleState.FighterACurrentHealth, fighterA.MaxHealth);
                double fighterBRemainingHealthPercentage = CalculateRemainingHealthPercentage(battleState.FighterBCurrentHealth, fighterB.MaxHealth);

                if (fighterARemainingHealthPercentage > fighterBRemainingHealthPercentage)
                {
                    winnerId = fighterA.Id;
                    battleOutcome = BattleOutcome.FighterAWin;
                }
                else if (fighterBRemainingHealthPercentage > fighterARemainingHealthPercentage)
                {
                    winnerId = fighterB.Id;
                    battleOutcome = BattleOutcome.FighterBWin;
                }
                else
                {
                    winnerId = null;
                    battleOutcome = BattleOutcome.Draw;
                }
            }
            else
            {
                bool fighterAIsDead = battleState.FighterACurrentHealth == 0;
                winnerId = fighterAIsDead ? fighterB.Id : fighterA.Id;
                battleOutcome = fighterAIsDead ? BattleOutcome.FighterBWin : BattleOutcome.FighterAWin;
            }


            BattleResult result = new BattleResult(battleState.FighterA.Id, battleState.FighterB.Id, winnerId, roundsResults, battleState.FighterACurrentHealth, battleState.FighterBCurrentHealth, battleOutcome, endingReason);

            return result;
        }


        private static double CalculateHitChance(Character character)
        {
            var hitChance = MinHitChance + (character.Accuracy / PercentageScale) * (MaxHitChance - MinHitChance);

            return hitChance;
        }

        private static double CalculateDodgeChance(Character character)
        {
            var dodgeChance = MinDodgeChance + (character.Evasion / PercentageScale) * (MaxDodgeChance - MinDodgeChance);

            return dodgeChance;
        }

        private static double CalculateRemainingHealthPercentage(int currentHealth, int maxHealth)
        {
            if (maxHealth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxHealth), maxHealth, "Max health must be greater than zero.");
            }

            return currentHealth * PercentageScale / maxHealth;
        }

        private static bool CanContinueBattle(BattleState state)
        {
            return state.FighterACurrentHealth > 0 && state.FighterBCurrentHealth > 0 && state.CurrentRound <= MaxRounds;
        }

        private static (Character attacker, Character defender) RollAttacker(Character fighterA, Character fighterB)
        {
            int roll = Random.Shared.Next(2);

            if (roll == 0)
            {
                return (fighterA, fighterB);
            }
            
            return (fighterB, fighterA);
        }

        private static AttackResult ResolveAttack(BattleState battleState, Character attacker, double attackerHitChance, Character defender, double defenderDodgeChance)
        {
            int defenderHealth = CalculateDefenderHealth(battleState, defender);

            bool passedAccuracyRoll = Random.Shared.NextDouble() < attackerHitChance;

            if (!passedAccuracyRoll)
            {
                return new AttackResult(attacker, defender, AttackOutcome.Miss, 0, 0, defenderHealth);
            }

            bool dodged = Random.Shared.NextDouble() < defenderDodgeChance;

            if (dodged)
            {
                return new AttackResult(attacker, defender, AttackOutcome.Dodge, 0, 0, defenderHealth);
            }

            // The upper bound is exclusive. Use long arithmetic to include int.MaxValue safely.
            int baseDamage = (int)Random.Shared.NextInt64(
                attacker.EquippedWeapon.MinDamage,
                (long)attacker.EquippedWeapon.MaxDamage + 1);
            int finalDamage = baseDamage;

            if (ReferenceEquals(defender, battleState.FighterA))
            {
                battleState.ApplyDamageToFighterA(finalDamage);
            }
            else
            {
                battleState.ApplyDamageToFighterB(finalDamage);
            }

            defenderHealth = CalculateDefenderHealth(battleState, defender);

            return new AttackResult(attacker, defender, AttackOutcome.Hit, baseDamage, finalDamage, defenderHealth);
        }

        private static int CalculateDefenderHealth(BattleState battleState, Character defender)
        {
            return ReferenceEquals(defender, battleState.FighterA) ? battleState.FighterACurrentHealth : battleState.FighterBCurrentHealth;
        }
    }
}
