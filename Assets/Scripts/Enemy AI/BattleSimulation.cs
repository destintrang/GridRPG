using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSimulation {


    public static int doubleAttackThreshold = 4;
    
    public BattleConditions conditions;

    public GameObject attacker;
    public GameObject defender;

    public int aDamage;
    public int aAccuracy;
    public int aCritDamage;
    public int aAttacks;

    public int dDamage;
    public int dAccuracy;
    public int dCritDamage;
    public int dAttacks;


    public BattleSimulation (GameObject _attacker, GameObject _defender, Vector3Int attackerPos, Vector3Int defenderPos)
    {
        attacker = _attacker;
        defender = _defender;

        int range = (int)(Mathf.Abs(attackerPos.x - defenderPos.x) + Mathf.Abs(attackerPos.y - defenderPos.y));
        conditions = new BattleConditions(_attacker, _defender, range);

        aDamage = CalculateDamage(_attacker, _defender, attackerPos, defenderPos);
        aAccuracy = CalculateHitChance(_attacker, _defender, attackerPos, defenderPos);
        aCritDamage = CalculateCritDamage(_attacker, _defender);
        aAttacks = 1 + _attacker.GetComponent<CharacterStats>().GetWeapon().GetBonusAttacks(conditions, _attacker);
        if (CalculateDoubleAttack(_attacker, _defender)) { aAttacks++; }


        dAttacks = 0;
        if (CanCounterattack(_defender, range))
        {
            dAttacks++;
            dAttacks += _defender.GetComponent<CharacterStats>().GetWeapon().GetBonusAttacks(conditions, _defender);

            if (CalculateDoubleAttack(_defender, _attacker)) { dAttacks++; }
            dDamage = CalculateDamage(_defender, _attacker, defenderPos, attackerPos);
            dAccuracy = CalculateHitChance(_defender, _attacker, defenderPos, attackerPos);
            dCritDamage = CalculateCritDamage(_defender, _attacker);
        }
        else
        {
            dDamage = 0;
            dAccuracy = 0;
            dCritDamage = 0;
        }
    }


    bool CanCounterattack (GameObject defender, int range)
    {
        if (defender.GetComponent<CharacterInventory>().equippedWeapon.ranges.Contains(range)) { return true; }
        else { return false; }
    }

    int CalculateDamage (GameObject attacker, GameObject defender)
    {
        return CalculateDamage(attacker, defender, new Vector3Int(-1, -1, -1), new Vector3Int(-1, -1, -1));
    }
    int CalculateDamage (GameObject attacker, GameObject defender, Vector3Int attackerPos, Vector3Int defenderPos)
    {
        Weapon aWeapon = attacker.GetComponent<CharacterStats>().GetWeapon();
        Weapon dWeapon = defender.GetComponent<CharacterStats>().GetWeapon();
        Armor dArmor = defender.GetComponent<CharacterStats>().GetArmor();

        int aAttack = attacker.GetComponent<CharacterStats>().GetCombatAttack();
        aAttack += aWeapon.GetBonusDamage(conditions, attacker);

        int dDefense = defender.GetComponent<CharacterStats>().GetCombatDefense();
        dDefense += CalculateDefenseTypeBonus(aWeapon, dArmor);
        dDefense += dWeapon.GetBonusDefense(conditions, defender);
        if (defenderPos.z != -1)
        {
            dDefense += TileManager.instance.allTiles[defenderPos].GetComponent<TileAttributes>().defenseBonus;
        }

        return (aAttack - dDefense);
    }

    int CalculateDefenseTypeBonus (Weapon attack, Armor defend)
    {
        if (defend == null) { return 0; }
        switch (attack.damageType)
        {
            case Weapon.DamageType.BASH:
                return defend.bashBonus;
            case Weapon.DamageType.CUT:
                return defend.cutBonus;
            case Weapon.DamageType.STAB:
                return defend.stabBonus;
            case Weapon.DamageType.BASHSTAB:
                return Mathf.Max(defend.bashBonus, defend.stabBonus);
            case Weapon.DamageType.CUTBASH:
                return Mathf.Max(defend.cutBonus, defend.bashBonus);
            case Weapon.DamageType.STABCUT:
                return Mathf.Max(defend.cutBonus, defend.stabBonus);
            case Weapon.DamageType.MAGIC:
                break;
        }

        return 0;
    }

    int CalculateHitChance(GameObject attacker, GameObject defender)
    {
        return CalculateHitChance(attacker, defender, new Vector3Int(-1, -1, -1), new Vector3Int(-1, -1, -1));
    }
    int CalculateHitChance (GameObject attacker, GameObject defender, Vector3Int attackerPos, Vector3Int defenderPos)
    {
        int chance = attacker.GetComponent<CharacterStats>().GetAccuracy() - defender.GetComponent<CharacterStats>().GetCombatEvasion();
        chance += attacker.GetComponent<CharacterStats>().GetWeapon().GetBonusAccuracy(conditions, attacker);
        chance -= defender.GetComponent<CharacterStats>().GetWeapon().GetBonusEvasion(conditions, defender);
        if (defenderPos.z != -1)
        {
            chance -= TileManager.instance.allTiles[defenderPos].GetComponent<TileAttributes>().evasionBonus;
        }

        if (chance < 0) { chance = 0; }
        else if (chance > 100) { chance = 100; }
        return (chance);
    }

    int CalculateCritDamage (GameObject attacker, GameObject defender)
    {
        int chance = attacker.GetComponent<CharacterStats>().GetCritDamage() - defender.GetComponent<CharacterStats>().GetCritDefense();

        if (chance < 0) { chance = 0; }

        return (chance);
    }
    bool CalculateDoubleAttack (GameObject attacker, GameObject defender)
    {
        if ((attacker.GetComponent<CharacterStats>().GetCombatSpeed() - defender.GetComponent<CharacterStats>().GetCombatSpeed()) >= doubleAttackThreshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public GameObject GetAttacker()
    {
        return attacker;
    }
    public GameObject GetDefender()
    {
        return defender;
    }
    public int GetAttackerDamage()
    {
        return aDamage;
    }
    public int GetAttackerAccuracy()
    {
        return aAccuracy;
    }
    public int GetAttackerCritDamage()
    {
        return aCritDamage;
    }
    public int GetAttackerAttacks()
    {
        return aAttacks;
    }
    public int GetDefenderDamage()
    {
        return dDamage;
    }
    public int GetDefenderAccuracy()
    {
        return dAccuracy;
    }
    public int GetDefenderCritDamage()
    {
        return dCritDamage;
    }
    public int GetDefenderAttacks()
    {
        return dAttacks;
    }
}
