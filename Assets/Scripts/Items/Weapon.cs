using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic Weapon", menuName = "Inventory/Weapon")]
public class Weapon : Item {

    public enum WeaponType
    {
        SWORD,
        LANCE,
        AXE,
        BLUDGEON,
        KNIFE,
        BOW,
        STAFF
    }

    public enum DamageType
    {
        CUT,
        STAB,
        BASH,
        STABCUT,
        CUTBASH,
        BASHSTAB,
        MAGIC
    }


    public WeaponType weaponClass;
    public DamageType damageType;

    public int might;
    public int magicBonus;
    public int healBonus;

    public int weight;
    public int accuracy;
    public int crit;

    public List<int> ranges;


    public virtual int GetBonusDamage (BattleConditions battle, GameObject owner)
    {
        return 0;
    }
    public virtual int GetBonusDefense(BattleConditions battle, GameObject owner)
    {
        return 0;
    }
    public virtual int GetBonusAccuracy(BattleConditions battle, GameObject owner)
    {
        return 0;
    }
    public virtual int GetBonusEvasion(BattleConditions battle, GameObject owner)
    {
        return 0;
    }
    public virtual int GetBonusCrit(BattleConditions battle, GameObject owner)
    {
        return 0;
    }
    public virtual int GetBonusAttacks(BattleConditions battle, GameObject owner)
    {
        return 0;
    }
}
