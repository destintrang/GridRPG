using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Swiftness", menuName = "Special Weapon/Swiftness")]
public class Swiftness : Weapon {

    public int bonusAttacks;

    //Swiftness: An extra attack is granted when attacking
    public override int GetBonusAttacks(BattleConditions battle, GameObject owner)
    {
        return bonusAttacks;
    }

}
