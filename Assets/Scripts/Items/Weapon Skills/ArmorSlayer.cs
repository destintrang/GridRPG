using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ArmorSlayer", menuName = "Special Weapon/Armor Slayer")]
public class ArmorSlayer : Weapon {

    public int armorSlayerDamageBonus;
    public Armor.WeightType effective;

    //Armor Slayer: Increased damage against heavy armor
    public override int GetBonusDamage(BattleConditions battle, GameObject owner)
    {
        if (battle.GetDefenderArmor() == null) { return 0; }
        if (owner.GetInstanceID() == battle.GetAttacker().GetInstanceID())
        {
            if (battle.GetDefenderArmor().weightClass == effective)
            {
                return armorSlayerDamageBonus;
            }
        } else if (owner.GetInstanceID() == battle.GetDefender().GetInstanceID())
        {
            if (battle.GetAttackerArmor().weightClass == effective)
            {
                return armorSlayerDamageBonus;
            }
        }

        return 0;
    }

}
