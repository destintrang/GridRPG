using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Consumable : Item {

    public int uses;

    public override void OnUse(GameObject user, bool fromConvoy = false)
    {

        uses--;
        //If the consumable has been used up from the unit's inventory, destroy it
        //If the item was used up from the convoy, the convoy will manage the removal of the item
        if (uses == 0 && !fromConvoy)
        {
            user.GetComponent<CharacterInventory>().RemoveItemByID(this.GetInstanceID());

            Destroy(this.gameObject);
        }

    }

    public override string GetName()
    {
        return (name + " (" + uses + ")");
    }
}
