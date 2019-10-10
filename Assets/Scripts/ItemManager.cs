using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemManager : MonoBehaviour
{



    List<Item> usedItems = new List<Item>();

    List<Item> weaponA = new List<Item>();
    List<Item> weaponB = new List<Item>();
    List<Item> weaponC = new List<Item>();
    List<Item> weaponD = new List<Item>();
    List<Item> weaponE = new List<Item>();

    List<Item> armorA = new List<Item>();
    List<Item> armorB = new List<Item>();
    List<Item> armorC = new List<Item>();
    List<Item> armorD = new List<Item>();
    List<Item> armorE = new List<Item>();

    List<Item> accessoryA = new List<Item>();
    List<Item> accessoryB = new List<Item>();
    List<Item> accessoryC = new List<Item>();
    List<Item> accessoryD = new List<Item>();
    List<Item> accessoryE = new List<Item>();

    List<Item> consumableA = new List<Item>();
    List<Item> consumableB = new List<Item>();
    List<Item> consumableC = new List<Item>();
    List<Item> consumableD = new List<Item>();
    List<Item> consumableE = new List<Item>();


    //Singleton
    public static ItemManager instance;
    private void Awake()
    {
        instance = this;
        LoadAllItems();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public Item GetWeapon(Item.ItemRank rank)
    {
        List<Item> list;

        if (rank == Item.ItemRank.A) { list = weaponA; }
        else if (rank == Item.ItemRank.B) { list = weaponB; }
        else if (rank == Item.ItemRank.C) { list = weaponC; }
        else if (rank == Item.ItemRank.D) { list = weaponD; }
        else { list = weaponE; }

        return ChooseItem(list);
    }
    public Item GetArmor(Item.ItemRank rank)
    {
        List<Item> list;

        if (rank == Item.ItemRank.A) { list = armorA; }
        else if (rank == Item.ItemRank.B) { list = armorB; }
        else if (rank == Item.ItemRank.C) { list = armorC; }
        else if (rank == Item.ItemRank.D) { list = armorD; }
        else { list = armorE; }

        return ChooseItem(list);
    }
    public Item GetAccessory(Item.ItemRank rank)
    {
        List<Item> list;

        if (rank == Item.ItemRank.A) { list = accessoryA; }
        else if (rank == Item.ItemRank.B) { list = accessoryB; }
        else if (rank == Item.ItemRank.C) { list = accessoryC; }
        else if (rank == Item.ItemRank.D) { list = accessoryD; }
        else { list = accessoryE; }


        return ChooseItem(list);
    }
    public Item GetConsumable(Item.ItemRank rank)
    {
        List<Item> list;

        if (rank == Item.ItemRank.A) { list = consumableA; }
        else if (rank == Item.ItemRank.B) { list = consumableB; }
        else if (rank == Item.ItemRank.C) { list = consumableC; }
        else if (rank == Item.ItemRank.D) { list = consumableD; }
        else { list = consumableE; }

        Item c = list[Random.Range(0, list.Count)];
        return c;
    }

    //This chooses an item from the list that hasn't been used yet
    //Recycling might need to be implemented
    private Item ChooseItem (List<Item> items)
    {

        Item c;
        while (true)
        {

            c = items[Random.Range(0, items.Count)];
            if (!usedItems.Contains(c))
            {
                usedItems.Add(c);
                return c;
            }

        }

    }



    void LoadAllItems ()
    {
        foreach(GameObject item in Resources.LoadAll("Items", typeof(GameObject)).Cast<GameObject>())
        {
            Item currentItem = item.GetComponent<Item>();

            if (currentItem is Weapon) { AddWeapon(currentItem); }
            else if (currentItem is Armor) { AddArmor(currentItem); }
            else if (currentItem is Accessory) { AddAccessory(currentItem); }
            else if (currentItem is Consumable) { AddConsumable(currentItem); }
        }
    }


    void AddWeapon (Item item)
    {

        if (item.rank == Item.ItemRank.A) { weaponA.Add(item); }
        else if (item.rank == Item.ItemRank.B) { weaponB.Add(item); }
        else if (item.rank == Item.ItemRank.C) { weaponC.Add(item); }
        else if (item.rank == Item.ItemRank.D) { weaponD.Add(item); }
        else if (item.rank == Item.ItemRank.E) { weaponE.Add(item); }

    }
    void AddArmor(Item item)
    {

        if (item.rank == Item.ItemRank.A) { armorA.Add(item); }
        else if (item.rank == Item.ItemRank.B) { armorB.Add(item); }
        else if (item.rank == Item.ItemRank.C) { armorC.Add(item); }
        else if (item.rank == Item.ItemRank.D) { armorD.Add(item); }
        else if (item.rank == Item.ItemRank.E) { armorE.Add(item); }

    }
    void AddAccessory(Item item)
    {

        if (item.rank == Item.ItemRank.A) { accessoryA.Add(item); }
        else if (item.rank == Item.ItemRank.B) { accessoryB.Add(item); }
        else if (item.rank == Item.ItemRank.C) { accessoryC.Add(item); }
        else if (item.rank == Item.ItemRank.D) { accessoryD.Add(item); }
        else if (item.rank == Item.ItemRank.E) { accessoryE.Add(item); }

    }
    void AddConsumable(Item item)
    {

        if (item.rank == Item.ItemRank.A) { consumableA.Add(item); }
        else if (item.rank == Item.ItemRank.B) { consumableB.Add(item); }
        else if (item.rank == Item.ItemRank.C) { consumableC.Add(item); }
        else if (item.rank == Item.ItemRank.D) { consumableD.Add(item); }
        else if (item.rank == Item.ItemRank.E) { consumableE.Add(item); }

    }


}
