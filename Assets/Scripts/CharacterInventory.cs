using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour {

    public Weapon equippedWeapon;
    public Armor equippedArmor;
    public Item[] inventory = new Item[3];

    private int loadWeight = 0;
    public int GetLoadWeight () { return loadWeight; }
    public void AddWeight (int w) { loadWeight += w; }


    private void Start()
    {
        //Do this to initialize the character load weight
        EquipNewItem(equippedWeapon);
        EquipNewItem(equippedArmor);
    }


    public void EquipItem (int index)
    {
        if (inventory[index - 3] is Weapon)
        {
            Weapon newEquipment = (Weapon) inventory[index - 3];
            if (equippedWeapon != null) { inventory[index - 3] = equippedWeapon; }
            else { inventory[index - 3] = null; }
            equippedWeapon = newEquipment;
        } else if (inventory[index - 3] is Armor)
        {
            Armor newEquipment = (Armor) inventory[index - 3];
            if (equippedArmor != null) { inventory[index - 3] = equippedArmor; }
            else { inventory[index - 3] = null; }
            equippedArmor = newEquipment;
        }
    }

    //Equips a new item, and then returns the previous item that was just unequipped
    public Item EquipNewItem (Item e)
    {
        if (e is Weapon)
        {
            Item oldItem = equippedWeapon;
            equippedWeapon = e as Weapon;
            loadWeight += (e as Weapon).weight;
            return oldItem;
        }
        else if (e is Armor)
        {
            Item oldItem = equippedArmor;
            equippedArmor = e as Armor;
            loadWeight += (e as Armor).weight;
            return oldItem;
        }

        return null;
    }

    public void UseItem (int index)
    {
        if (index == 1)
        {
            equippedWeapon.OnUse(gameObject);
        }
        else if (index == 2)
        {
            equippedArmor.OnUse(gameObject);
        }
        else
        {
            inventory[index - 3].OnUse(gameObject);
        }
    }


    public bool AddItem (Item i)
    {
        for (int n = 0; n < inventory.Length; n++)
        {
            if (inventory[n] == null)
            {
                inventory[n] = i;

                //If this item was an accessory, add the weight and bonuses to the character's stats
                if (i is Accessory)
                {
                    Accessory a = i as Accessory;
                    a.OnAdd(this.gameObject);
                }

                return true;
            }
        }

        return false;
    }

    public void RemoveItemByIndex (int index)
    {
        Item i = inventory[index];

        //If this item was an accessory, remove the weight and bonuses from the character's stats
        if (i is Accessory)
        {
            Accessory a = i as Accessory;
            a.OnRemove(this.gameObject);
        }

        inventory[index] = null;

        ReformatInventory();
    }

    public void RemoveItemByID (int id)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (id == inventory[i].GetInstanceID())
            {
                //If this item was an accessory, remove the weight and bonuses from the character's stats
                if (inventory[i] is Accessory)
                {
                    Accessory a = inventory[i] as Accessory;
                    a.OnRemove(this.gameObject);
                }

                inventory[i] = null;
                break;
            }
        }

        ReformatInventory();
    }


    private void ReformatInventory ()
    {
        for (int i = 0; i < inventory.Length - 1; i++)
        {
            if (inventory[i] == null && inventory [i + 1] != null)
            {
                inventory[i] = inventory[i + 1];
                inventory[i + 1] = null;
            }
        }
    }


    public bool IsFull ()
    {
        foreach (Item item in inventory)
        {
            if (item == null)
            {
                //Space was found
                return false;
            }
        }


        //If it makes it here, then inventory is full
        return true;
    }
}
