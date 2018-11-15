using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Initiator", menuName = "Special Weapon/Initiator")]
public class Initiator : Weapon {

    public int bonusDamage;

    //Initiator: Increased damage when initiating combat
    public override int GetBonusDamage(BattleConditions battle, GameObject owner)
    {
        if (owner.GetInstanceID() == battle.GetAttacker().GetInstanceID())
        {
            return bonusDamage;
        }

        return 0;
    }

}
