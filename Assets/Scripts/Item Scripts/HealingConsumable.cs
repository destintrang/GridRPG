using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingConsumable : Consumable
{

    public int healAmount;


    public override void OnUse(GameObject user, bool fromConvoy = false)
    {
        user.GetComponent<CharacterStats>().Heal(healAmount);
        base.OnUse(user, fromConvoy);
    }


    //If the user is at max health already, we won't let them spend the healing item
    public override bool CanUse (GameObject user)
    {
        if (user.GetComponent<CharacterStats>().GetMaxHealth() <= user.GetComponent<CharacterStats>().GetHealth())
        {
            return false;
        }

        return true;
    }
}
