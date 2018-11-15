using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour {

    public Weapon equippedWeapon;
    public Armor equippedArmor;
    public Item[] inventory = new Item[3];


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

    public void DropItem (int index)
    {
        if (index == 1)
        {
            equippedWeapon = null;
        }
        else if (index == 2)
        {
            equippedArmor = null;
        }
        else
        {
            inventory[index - 3] = null;
        }
    }
}
