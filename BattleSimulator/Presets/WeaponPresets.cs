using BattleSimulator.Models.Entities;
using BattleSimulator.Models.Enums;

namespace BattleSimulator.Presets
{
    public static class WeaponPresets
    {
        public static Weapon CreateSpear()
        {
            return new Weapon(
                name: "Spear",
                weaponType: WeaponType.Spear,
                maxDamage: 15,
                minDamage: 5)
            { Id = 1 };
        }

        public static Weapon CreateAxe()
        {
            return new Weapon(
                name: "Axe",
                weaponType: WeaponType.Axe,
                maxDamage: 20,
                minDamage: 10)
            { Id = 2 };
        }
    }
}
