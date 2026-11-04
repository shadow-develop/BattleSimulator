namespace BattleSimulator.Battle
{
    public class RoundResult
    {
        public int RoundNumber { get; }

        public IReadOnlyList<AttackResult> Attacks { get; }

        public RoundResult(int roundNumber, IReadOnlyList<AttackResult> attacks)
        {
            if (roundNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roundNumber), "Round number must be greater than zero.");
            }

            RoundNumber = roundNumber;

            if(attacks is null)
            {
                throw new ArgumentNullException(nameof(attacks), "Attacks list cannot be null.");
            }

            if(attacks.Count == 0 || attacks.Count > 2)
            {
                throw new ArgumentException("Attacks list must contain between 1 and 2 elements.", nameof(attacks));
            }

            if(attacks.Any(a => a == null))
            {
                throw new ArgumentException("Attacks list cannot contain null elements.", nameof(attacks));
            }

            Attacks = Array.AsReadOnly(attacks.ToArray());
        }
    }
}
