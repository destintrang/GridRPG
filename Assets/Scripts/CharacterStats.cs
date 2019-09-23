using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {


    [SerializeField] string race;

    private string name;
    public string GetName () { return name; }
    public void SetName (string n) { name = n; }

    [SerializeField] int rank = 1;

    [SerializeField] int maxHealth;
    [SerializeField] int health;
    [SerializeField] int strength;
    [SerializeField] int magic;
    [SerializeField] int dexterity;
    [SerializeField] int speed;
    [SerializeField] int defense;
    [SerializeField] int faith;
    [SerializeField] int luck;
    [SerializeField] int build;
    [SerializeField] int move;

    [SerializeField] float healthGrowth;
    [SerializeField] float strengthGrowth;
    [SerializeField] float magicGrowth;
    [SerializeField] float dexterityGrowth;
    [SerializeField] float speedGrowth;
    [SerializeField] float defenseGrowth;
    [SerializeField] float faithGrowth;
    [SerializeField] float luckGrowth;

    CharacterInventory equipment;


    int strengthBonus = 0;
    public void ModifyStrength (int b) { strengthBonus += b; }
    int magicBonus = 0;
    public void ModifyMagic (int b) { magicBonus += b; }
    int dexterityBonus = 0;
    public void ModifyDexterity (int b) { dexterityBonus += b; }
    int speedBonus = 0;
    public void ModifySpeed (int b) { speedBonus += b; }
    int defenseBonus = 0;
    public void ModifyDefense(int b) { defenseBonus += b; }
    int faithBonus = 0;
    public void ModifyFaith(int b) { faithBonus += b; }
    int luckBonus = 0;
    public void ModifyLuck(int b) { luckBonus += b; }
    int buildBonus = 0;
    public void ModifyBuild(int b) { buildBonus += b; }
    int moveBonus = 0;
    public void ModifyMove (int b) { moveBonus += b; }



    // Use this for initialization
    void Start () {
        equipment = GetComponent<CharacterInventory>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void RankUp ()
    {
        rank++;

        maxHealth += CalculateGrowth(healthGrowth);
        strength += CalculateGrowth(strengthGrowth);
        magic += CalculateGrowth(magicGrowth);
        dexterity += CalculateGrowth(dexterityGrowth);
        speed += CalculateGrowth(speedGrowth);
        defense += CalculateGrowth(defenseGrowth);
        faith += CalculateGrowth(faithGrowth);
        luck += CalculateGrowth(luckGrowth);
    }

    private int CalculateGrowth (float g)
    {
        int total = 0;

        float growth = g;
        while (growth > 0)
        {
            if (growth >= 1)
            {
                total++;
                growth -= 1;
            }
            else
            {
                float rng = Random.Range(0.01f, 1.0f);
                if (rng <= growth)
                {
                    total++;
                }
                growth -= 1;
            }
        }

        return total;
    }


    public void TakeDamage (int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    public void Heal (int heal)
    {
        health += heal;
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
        returnedInt = GetStrength();
        if (equipment.equippedWeapon != null) { returnedInt += equipment.equippedWeapon.might; }
        return (returnedInt);
    }
    public int GetCombatDefense()
    {
        int returnedInt;
        returnedInt = GetDefense();
        if (equipment.equippedArmor != null) { returnedInt += equipment.equippedArmor.defense; }
        return (returnedInt);
    }
    public int GetCombatSpeed ()
    {
        int weight = 0;
        if (equipment.equippedWeapon != null) { weight += equipment.equippedWeapon.weight; }
        if (equipment.equippedArmor != null) { weight += equipment.equippedArmor.weight; }
        if (weight > GetBuild())
        {
            return (GetSpeed() - (weight - GetBuild()));
        } else
        {
            return GetSpeed();
        }
    }
    public int GetAccuracy ()
    {
        if (equipment.equippedWeapon == null) { return 0; }
        int returnedInt = 0;
        if (equipment.equippedWeapon != null) { returnedInt += equipment.equippedWeapon.accuracy; }
        else returnedInt += 100;
        return (returnedInt + GetDexterity() * 2 + GetFaith());
    }
    public int GetCombatEvasion ()
    {
        return (GetCombatSpeed() * 2 + GetFaith()) ;
    }
    public int GetCritRate ()
    {
        if (equipment.equippedWeapon == null) { return 0; }
        int returnedInt = 0;
        if (equipment.equippedWeapon != null) { returnedInt += equipment.equippedWeapon.crit; }
        return (returnedInt + (GetDexterity() / 2));
    }
    public int GetCritEvade ()
    {
        return (GetFaith() + (GetDexterity() / 2));
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
        return strength + strengthBonus;
    }
    public int GetMagic ()
    {
        return magic + magicBonus;
    }
    public int GetDexterity ()
    {
        return dexterity + dexterityBonus;
    }
    public int GetSpeed ()
    {
        return speed + speedBonus;
    }
    public int GetDefense ()
    {
        return defense + defenseBonus;
    }
    public int GetFaith()
    {
        return faith + faithBonus;
    }
    public int GetLuck()
    {
        return luck + luckBonus;
    }
    public int GetBuild ()
    {
        return build + buildBonus;
    }
    public int GetMovement ()
    {
        return move + moveBonus;
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
