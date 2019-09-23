using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessory : Item
{

    public int weight;

    public int strengthBonus;
    public int magicBonus;
    public int dexterityBonus;
    public int speedBonus;
    public int defenseBonus;
    public int faithBonus;
    public int luckBonus;
    public int buildBonus;
    public int moveBonus;


    public virtual void OnAdd(GameObject user)
    {
        user.GetComponent<CharacterInventory>().AddWeight(weight);

        if (strengthBonus != 0) { user.GetComponent<CharacterStats>().ModifyStrength(strengthBonus); }
        if (magicBonus != 0) { user.GetComponent<CharacterStats>().ModifyMagic(magicBonus); }
        if (dexterityBonus != 0) { user.GetComponent<CharacterStats>().ModifyDexterity(dexterityBonus); }
        if (speedBonus != 0) { user.GetComponent<CharacterStats>().ModifySpeed(speedBonus); }
        if (defenseBonus != 0) { user.GetComponent<CharacterStats>().ModifyDefense(defenseBonus); }
        if (faithBonus != 0) { user.GetComponent<CharacterStats>().ModifyFaith(faithBonus); }
        if (luckBonus != 0) { user.GetComponent<CharacterStats>().ModifyLuck(luckBonus); }
        if (buildBonus != 0) { user.GetComponent<CharacterStats>().ModifyBuild(buildBonus); }
        if (moveBonus != 0) { user.GetComponent<CharacterStats>().ModifyMove(moveBonus); }
        return;
    }

    public virtual void OnRemove(GameObject user)
    {
        user.GetComponent<CharacterInventory>().AddWeight(-weight);

        if (strengthBonus != 0) { user.GetComponent<CharacterStats>().ModifyStrength(-strengthBonus); }
        if (magicBonus != 0) { user.GetComponent<CharacterStats>().ModifyMagic(-magicBonus); }
        if (dexterityBonus != 0) { user.GetComponent<CharacterStats>().ModifyDexterity(-dexterityBonus); }
        if (speedBonus != 0) { user.GetComponent<CharacterStats>().ModifySpeed(-speedBonus); }
        if (defenseBonus != 0) { user.GetComponent<CharacterStats>().ModifyDefense(-defenseBonus); }
        if (faithBonus != 0) { user.GetComponent<CharacterStats>().ModifyFaith(-faithBonus); }
        if (luckBonus != 0) { user.GetComponent<CharacterStats>().ModifyLuck(-luckBonus); }
        if (buildBonus != 0) { user.GetComponent<CharacterStats>().ModifyBuild(-buildBonus); }
        if (moveBonus != 0) { user.GetComponent<CharacterStats>().ModifyMove(-moveBonus); }
        return;
    }
}
