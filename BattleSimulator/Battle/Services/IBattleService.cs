using BattleSimulator.Models.Entities;

namespace BattleSimulator.Battle.Services
{
    public interface IBattleService
    {
        BattleResult SimulateBattle(Character fighterA, Character fighterB);
    }
}
