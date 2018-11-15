using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {


    [SerializeField] int maxHealth;
    [SerializeField] int health;
    [SerializeField] int strength;
    [SerializeField] int magic;
    [SerializeField] int dexterity;
    [SerializeField] int speed;
    [SerializeField] int defense;
    [SerializeField] int faith;
    [SerializeField] int build;
    [SerializeField] int move;

    CharacterInventory equipment;


    // Use this for initialization
    void Start () {
        equipment = GetComponent<CharacterInventory>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage (int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    public List<int> GetRange ()
    {
        if (GetComponent<CharacterInventory>().equippedWeapon)
        {
            return equipment.equippedWeapon.ranges;
        } else
        {
            return new List<int> { 0 };
        }
    }

    public int GetCombatAttack ()
    {
        if (equipment.equippedWeapon == null) { return 0; }
        int returnedInt;
        returnedInt = strength;
        if (equipment.equippedWeapon != null) { returnedInt += equipment.equippedWeapon.might; }
        return (returnedInt);
    }
    public int GetCombatDefense()
    {
        int returnedInt;
        returnedInt = defense;
        if (equipment.equippedArmor != null) { returnedInt += equipment.equippedArmor.defense; }
        return (returnedInt);
    }
    public int GetCombatSpeed ()
    {
        int weight = 0;
        if (equipment.equippedWeapon != null) { weight += equipment.equippedWeapon.weight; }
        if (equipment.equippedArmor != null) { weight += equipment.equippedArmor.weight; }
        if (weight > build)
        {
            return (speed - (weight - build));
        } else
        {
            return speed;
        }
    }
    public int GetAccuracy ()
    {
        if (equipment.equippedWeapon == null) { return 0; }
        int returnedInt = 0;
        if (equipment.equippedWeapon != null) { returnedInt += equipment.equippedWeapon.accuracy; }
        else returnedInt += 100;
        return (returnedInt + dexterity * 2 + faith);
    }
    public int GetCombatEvasion ()
    {
        return (GetCombatSpeed() * 2 + faith);
    }
    public int GetCritRate ()
    {
        if (equipment.equippedWeapon == null) { return 0; }
        int returnedInt = 0;
        if (equipment.equippedWeapon != null) { returnedInt += equipment.equippedWeapon.crit; }
        return (returnedInt + (dexterity / 2));
    }
    public int GetCritEvade ()
    {
        return (faith + (dexterity / 2));
    }

    public int GetMaxHealth ()
    {
        return maxHealth;
    }
    public int GetHealth ()
    {
        return health;
    }
    public int GetStrength ()
    {
        return strength;
    }
    public int GetMagic ()
    {
        return magic;
    }
    public int GetDexterity ()
    {
        return dexterity;
    }
    public int GetSpeed ()
    {
        return speed;
    }
    public int GetDefense ()
    {
        return defense;
    }
    public int GetFaith ()
    {
        return faith;
    }
    public int GetBuild ()
    {
        return build;
    }
    public int GetMovement ()
    {
        return move;
    }
    public Weapon GetWeapon ()
    {
        return equipment.equippedWeapon;
    }
    public Armor GetArmor ()
    {
        return equipment.equippedArmor;
    }
    
}
