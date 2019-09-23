using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : Interactable
{

    public List<Item> treasureItems;
    public int treasureGold;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void OnInteract(GameObject unit)
    {
        List<Item> overloaded = new List<Item>();

        foreach (Item i in treasureItems)
        {
            if (CanGive(unit))
            {
                unit.GetComponent<CharacterInventory>().AddItem(i);
            }
            else
            {
                overloaded.Add(i);
            }
        }

        if (overloaded.Count > 0)
        {
            ItemOverloadManager.instance.StartOverload(overloaded);
        }

        UnsetInteractableTiles();
    }

}
