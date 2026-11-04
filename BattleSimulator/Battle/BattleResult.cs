using BattleSimulator.Battle.Enums;

namespace BattleSimulator.Battle
{
    public class BattleResult
    {
        public int FighterAId { get; }

        public int FighterBId { get; }

        public int? WinnerId { get; }

        public IReadOnlyList<RoundResult> RoundResults { get; }

        public int TotalRounds => RoundResults.Count;

        public int FighterARemainingHealth { get; }

        public int FighterBRemainingHealth { get; }

        public BattleOutcome BattleOutcome { get; }

        public EndingReason EndingReason { get; }

        public BattleResult(int fighterAId, int fighterBId, int? winnerId, IReadOnlyList<RoundResult> roundResults, int fighterARemainingHealth, int fighterBRemainingHealth, BattleOutcome battleOutcome, EndingReason endingReason)
        {
            FighterAId = fighterAId;
            FighterBId = fighterBId;

            if (!Enum.IsDefined(battleOutcome))
            {
                throw new ArgumentOutOfRangeException(nameof(battleOutcome), battleOutcome, "Invalid battle outcome.");
            }

            BattleOutcome = battleOutcome;

            if (!Enum.IsDefined(endingReason))
            {
                throw new ArgumentOutOfRangeException(nameof(endingReason), endingReason, "Invalid ending reason.");
            }

            EndingReason = endingReason;

            bool winnerMatchesOutcome = battleOutcome switch
            {
                BattleOutcome.FighterAWin => winnerId == fighterAId,
                BattleOutcome.FighterBWin => winnerId == fighterBId,
                BattleOutcome.Draw => winnerId == null,
                _ => false
            };

            if (!winnerMatchesOutcome)
            {
                throw new ArgumentException("WinnerId does not match the BattleOutcome.", nameof(winnerId));
            }

            WinnerId = winnerId;

            if (roundResults is null)
            {
                throw new ArgumentNullException(nameof(roundResults), "Round results cannot be null.");
            }

            if (roundResults.Count == 0 || roundResults.Count > 50)
            {
                throw new ArgumentException("Round list must contain between 1 and 50 rounds.", nameof(roundResults));
            }

            if (roundResults.Any(r => r == null))
            {
                throw new ArgumentException("Round results list cannot contain null elements.", nameof(roundResults));
            }
            RoundResults = Array.AsReadOnly(roundResults.ToArray());

            FighterARemainingHealth = ValidateRemainingHealth(fighterARemainingHealth);
            FighterBRemainingHealth = ValidateRemainingHealth(fighterBRemainingHealth);
        }

        private static int ValidateRemainingHealth(int remainingHealth)
        {
            if (remainingHealth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(remainingHealth), remainingHealth, "Remaining health cannot be negative.");
            }

            return remainingHealth;
        }
    }
}
