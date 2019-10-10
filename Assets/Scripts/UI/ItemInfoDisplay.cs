using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoDisplay : MonoBehaviour
{


    //This holds everything
    public GameObject everything;

    //Item will always use this name, no matter what kind of item
    public Text itemName;

    //In some situations, we want to display the cost of the item
    public Text itemCost;

    //Display is different depending on what the item is: Weapon, Armor, Consumable, Accessory
    public GameObject weaponInfo;
    public Text weaponDamage;
    public Text weaponAcuracy;
    public Text weaponCrit;
    public Text weaponWeight;
    public Text weaponClass;
    public Text weaponDamageType;
    public Text weaponDescription;

    public GameObject armorInfo;
    public Text armorDefense;
    public Text armorSlashDefense;
    public Text armorStabDefense;
    public Text armorSmashDefense;
    public Text armorWeight;
    public Text armorEvasion;
    public Text armorCritDefense;
    public Text armorDescription;

    public GameObject consumableInfo;
    public Text consumableUses;
    public Text consumableDescription;

    public GameObject accessoryInfo;
    public Text accessoryWeight;
    public Text accessoryDescription;


    public void LoadItemInfo (Item item)
    {

        //Activate the parent obj
        everything.SetActive(true);

        itemName.text = item.GetName();
        if (itemCost != null) { itemCost.text = item.goldCost.ToString(); }

        //Clear the item info first
        ResetInfo();

        if (item is Weapon)
        {
            LoadWeaponInfo(item as Weapon);
        }
        else if (item is Armor)
        {
            LoadArmorInfo(item as Armor);
        }
        else if (item is Consumable)
        {
            LoadConsumableInfo(item as Consumable);
        }
        else if (item is Accessory)
        {
            LoadAccessoryInfo(item as Accessory);
        }
    }


    public void Deactivate ()
    {
        everything.SetActive(false);
    }


    private void ResetInfo ()
    {
        weaponInfo.SetActive(false);
        armorInfo.SetActive(false);
        consumableInfo.SetActive(false);
        accessoryInfo.SetActive(false);
    }
    private void LoadWeaponInfo(Weapon item)
    {
        //Weapon damage, weight, crit, accuracy
        //Damage type, weapon class, description

        weaponInfo.SetActive(true);

        weaponDamage.text = item.might.ToString();
        weaponAcuracy.text = item.accuracy.ToString();
        weaponCrit.text = item.crit.ToString();
        weaponWeight.text = item.weight.ToString();
        weaponClass.text = item.weaponClass.ToString();
        weaponDamageType.text = item.damageType.ToString();
        weaponDescription.text = item.description;

    }
    private void LoadArmorInfo(Armor item)
    {
        //Base defense, bonus type defense, weight, avoid, crit avoid, description

        armorInfo.SetActive(true);

        armorDefense.text = item.defense.ToString();
        armorSlashDefense.text = item.cutBonus.ToString();
        armorStabDefense.text = item.stabBonus.ToString();
        armorSmashDefense.text = item.bashBonus.ToString();
        armorWeight.text = item.weight.ToString();
        armorEvasion.text = item.evasion.ToString();
        //armorCritDefense.text = item;
        armorDescription.text = item.description;

    }
    private void LoadConsumableInfo(Consumable item)
    {
        //Description, number of uses

        consumableInfo.SetActive(true);

        consumableUses.text = item.uses.ToString();
        consumableDescription.text = item.description;

    }
    private void LoadAccessoryInfo(Accessory item)
    {
        //Description, weight

        accessoryInfo.SetActive(true);

        accessoryWeight.text = item.weight.ToString();
        accessoryDescription.text = item.description;

    }

}
