using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleConditions {

    int combatRange;

    GameObject attacker;
    Weapon aWeapon;
    Armor aArmor;

    GameObject defender;
    Weapon dWeapon;
    Armor dArmor;


	public BattleConditions (GameObject _attacker, GameObject _defender, int _range)
    {
        combatRange = _range;

        attacker = _attacker;
        aWeapon = _attacker.GetComponent<CharacterStats>().GetWeapon();
        aArmor = _attacker.GetComponent<CharacterStats>().GetArmor();

        defender = _defender;
        dWeapon = _defender.GetComponent<CharacterStats>().GetWeapon();
        dArmor = _defender.GetComponent<CharacterStats>().GetArmor();
    }

    public int GetRange()
    {
        return combatRange;
    }

    public GameObject GetAttacker ()
    {
        return attacker;
    }
    public GameObject GetDefender ()
    {
        return defender;
    }

    public Weapon GetAttackerWeapon ()
    {
        return aWeapon;
    }
    public Weapon GetDefenderWeapon()
    {
        return dWeapon;
    }

    public Armor GetAttackerArmor ()
    {
        return aArmor;
    }
    public Armor GetDefenderArmor ()
    {
        return dArmor;
    }
}
