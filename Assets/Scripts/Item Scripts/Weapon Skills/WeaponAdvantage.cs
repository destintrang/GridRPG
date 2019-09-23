using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponAdvantage", menuName = "Special Weapon/WeaponAdvantage")]
public class WeaponAdvantage : Weapon {

    public Weapon.WeaponType advantage;

    public int bonusDamage;
    public int bonusDefense;
    public int bonusAccuracy;
    public int bonusEvasion;


    public override int GetBonusDamage(BattleConditions battle, GameObject owner)
    {
        if (Advantage(battle, owner)) { return bonusDamage; }

        return 0;
    }

    public override int GetBonusDefense(BattleConditions battle, GameObject owner)
    {
        if (Advantage(battle, owner)) { return bonusDefense; }

        return 0;
    }

    public override int GetBonusAccuracy(BattleConditions battle, GameObject owner)
    {
        if (Advantage(battle, owner)) { return bonusAccuracy; }

        return 0;
    }

    public override int GetBonusEvasion(BattleConditions battle, GameObject owner)
    {
        if (Advantage(battle, owner)) { return bonusEvasion; }

        return 0;
    }


    bool Advantage (BattleConditions battle, GameObject owner)
    {
        if (owner.GetInstanceID() == battle.GetAttacker().GetInstanceID())
        {
            if (battle.GetDefenderWeapon().weaponClass == advantage)
            {
                return true;
            }
        }
        else if (owner.GetInstanceID() == battle.GetDefender().GetInstanceID())
        {
            if (battle.GetAttackerWeapon().weaponClass == advantage)
            {
                return true;
            }
        }

        return false;
    }
}
