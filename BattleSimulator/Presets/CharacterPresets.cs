using BattleSimulator.Models.Entities;

namespace BattleSimulator.Presets
{
    public static class CharacterPresets
    {
        public static Character CreateLeonidas()
        {
            return new Character(
                name: "Leonidas",
                maxHealth: 100,
                accuracy: 75,
                evasion: 50,
                equippedWeapon: WeaponPresets.CreateSpear())
            { Id = 1 };
        }

        public static Character CreateHarald()
        {
            return new Character(
                name: "Harald",
                maxHealth: 110,
                accuracy: 60,
                evasion: 40,
                equippedWeapon: WeaponPresets.CreateAxe())
            { Id = 2 };
        }
    }
}
