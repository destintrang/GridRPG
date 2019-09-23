using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoyManager : MonoBehaviour
{

    List<Item> swords = new List<Item>();
    List<Item> axes = new List<Item>();
    List<Item> lances = new List<Item>();
    List<Item> bludgeons = new List<Item>();
    List<Item> knives = new List<Item>();
    List<Item> bows = new List<Item>();
    List<Item> staves = new List<Item>();
    List<Item> armor = new List<Item>();
    List<Item> accesories = new List<Item>();
    List<Item> consumables = new List<Item>();
    List<List<Item>> convoyItems;

    int storedItemCount = 0;
    const int MAXITEMS = 100;

    public Item a;
    public Item b;
    public Item c;


    //Singleton
    public static ConvoyManager instance;
    private void Awake()
    {
        instance = this;
        convoyItems = new List<List<Item>> { swords, axes, lances, bludgeons, knives, bows, staves, armor, accesories, consumables };
    }


    // Start is called before the first frame update
    void Start()
    {
        AddItem(a);
        AddItem(b);
        AddItem(c);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) { RemoveItem(0,0); }
    }


    public List<List<Item>> GetItemLists()
    {
        return convoyItems;
        //return new List<List<Item>> { swords, axes, lances, bludgeons, knives, bows, staves, armor, accesories, consumables };
    }


    public Item GetItem (int type, int index)
    {
        return convoyItems[type][index];
    }


    public bool AddItem (Item i)
    {
        if (storedItemCount >= MAXITEMS)
        {
            //Hit the convoy's max storage
            return false;
        }

        if (i is Weapon)
        {
            AddWeapon(i);
        }
        else if (i is Armor)
        {
            armor.Add(i);
        }
        else if (i is Accessory)
        {
            accesories.Add(i);
        }
        else if (i is Consumable)
        {
            consumables.Add(i);
        }

        storedItemCount++;
        return true;
    }

    private void AddWeapon (Item i)
    {
        Weapon w = i as Weapon;
        switch (w.weaponClass)
        {
            case Weapon.WeaponType.SWORD: swords.Add(i); break;
            case Weapon.WeaponType.AXE: axes.Add(i); break;
            case Weapon.WeaponType.LANCE: lances.Add(i); break;
            case Weapon.WeaponType.BLUDGEON: bludgeons.Add(i); break;
            case Weapon.WeaponType.KNIFE: knives.Add(i); break;
            case Weapon.WeaponType.BOW: bows.Add(i); break;
            case Weapon.WeaponType.STAFF: staves.Add(i); break;
        }
    }


    public Item RemoveItem (int type, int index)
    {
        Item removed = convoyItems[type][index];
        convoyItems[type].RemoveAt(index);
        return removed;
    }


}
