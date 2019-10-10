using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseConvoyManager : BaseMenu
{


    private GameObject currentUnit;
    private Item currentItem;

    //Index if we're looking at the unit's items
    private int currentItemIndex;
    //Otherwise, we're looking at an item in the convoy, so we need a different index
    private int currentConvoyItemIndex;
    private CharacterInventory currentInventory;

    public Text unitName;

    public Button weapon;
    public Button armor;
    public Button itemA;
    public Button itemB;
    public Button itemC;

    public Text itemCategoryText;
    private List<string> categoryNames = new List<string> { "SWORDS", "AXES", "LANCES", "BLUDGEONS", "KNIVES", "BOWS", "STAVES", "ARMOR", "ACCESORIES", "CONSUMABLES" };

    public List<Button> convoyButtons;
    private List<Button> activeButtons;

    private List<List<Item>> convoy;
    private int convoyIndex;


    public List<Transform> optionButtonLocations;
    public List<Transform> optionConvoyButtonLocations;
    public Button storeButton;
    public Button useButton;
    public Button equipButton;
    public Button takeButton;
    public Button useConvoyButton;
    public Button equipConvoyButton;


    //Canvas object that displays item info
    public ItemInfoDisplay infoDisplay;


    //Singleton
    public static BaseConvoyManager instance;
    private void Awake()
    {
        instance = this;
        activeButtons = new List<Button>();
    }


    public override void A()
    {
        if (currentItem == null)
        {
            //First change the convoy list we're looking at
            if (convoyIndex == 0)
            {
                convoyIndex = convoy.Count - 1;
            }
            else
            {
                convoyIndex--;
            }


            //Then update the buttons
            LoadConvoySlots(convoyIndex);
        }
    }

    public override void D()
    {
        if (currentItem == null)
        {
            //First change the convoy list we're looking at
            if (convoyIndex == convoy.Count - 1)
            {
                convoyIndex = 0;
            }
            else
            {
                convoyIndex++;
            }


            //Then update the buttons
            LoadConvoySlots(convoyIndex);
        }
    }

    public override void Escape()
    {
        //Only exit the convoy if we aren't looking at an item
        if (currentItem == null)
        {
            ResetConvoyMenu();
            BaseItemsManager.instance.UpdateItemInfo();
            BaseManager.instance.CancelButton();
            return;
        }

        if (currentItem)
        {
            ResetCurrentItem();
            ResetConvoyItemOptions();
            ResetItemOptions();
            return;
        }
    }


    public void LoadConvoyMenu (GameObject character)
    {
        LoadUnitInfo(character);
        LoadConvoyItems();
    }


    private void LoadConvoyItems ()
    {
        convoy = ConvoyManager.instance.GetItemLists();
        convoyIndex = 0;
        LoadConvoySlots(convoyIndex);
    }

    private void LoadConvoySlots (int index)
    {
        //Clear the previous buttons and reset the active buttons list
        foreach (Button b in activeButtons)
        {
            b.gameObject.SetActive(false);
        }
        activeButtons = new List<Button>();

        //Then update the buttons with the current list
        List<Item> itemList = convoy[index];

        for (int i = 0; i < itemList.Count; i++)
        {
            convoyButtons[i].GetComponentInChildren<Text>().text = itemList[i].GetName();
            convoyButtons[i].gameObject.SetActive(true);
            activeButtons.Add(convoyButtons[i]);
        }

        //Update the category title
        itemCategoryText.text = categoryNames[index];
    }


    private void LoadUnitInfo(GameObject character)
    {
        //Set the unit that we are currently looking at
        currentUnit = character;
        currentInventory = currentUnit.GetComponent<CharacterInventory>();
        
        unitName.text = currentUnit.GetComponent<CharacterStats>().GetName();
        UpdateItemInfo();
    }


    //Call this when you need to correctly display the unit's equipment and items
    private void UpdateItemInfo()
    {
        weapon.GetComponentInChildren<Text>().text = currentInventory.equippedWeapon.GetName();
        weapon.gameObject.SetActive(true);

        armor.GetComponentInChildren<Text>().text = currentInventory.equippedArmor.GetName();
        armor.gameObject.SetActive(true);


        if (currentInventory.inventory[0])
        {
            itemA.gameObject.SetActive(true);
            itemA.GetComponentInChildren<Text>().text = currentInventory.inventory[0].GetName();
        }
        else
        {
            itemA.gameObject.SetActive(false);
        }
        if (currentInventory.inventory[1])
        {
            itemB.gameObject.SetActive(true);
            itemB.GetComponentInChildren<Text>().text = currentInventory.inventory[1].GetName();
        }
        else
        {
            itemB.gameObject.SetActive(false);
        }
        if (currentInventory.inventory[2])
        {
            itemC.gameObject.SetActive(true);
            itemC.GetComponentInChildren<Text>().text = currentInventory.inventory[2].GetName();
        }
        else
        {
            itemC.gameObject.SetActive(false);
        }
    }


    //These button functions are for when the player presses on the UNIT's items
    public void OnItemSlotButton(int i)
    {
        currentItem = currentUnit.GetComponent<CharacterInventory>().inventory[i];
        currentItemIndex = i;

        //Reset the item options before turning them back on
        ResetItemOptions();

        //Load the item info
        infoDisplay.LoadItemInfo(currentItem);


        if (currentItem is Weapon)
        {
            DisplayWeaponOptions();
        }
        else if (currentItem is Armor)
        {
            DisplayArmorOptions();
        }
        else if (currentItem is Consumable)
        {
            DisplayConsumableOptions();
        }
        else if (currentItem is Accessory)
        {
            DisplayAccessoryOptions();
        }
    }

    public void OnWeaponSlot()
    {
        currentItem = currentUnit.GetComponent<CharacterInventory>().equippedWeapon;

        //Load the item info
        infoDisplay.LoadItemInfo(currentItem);

        //Reset the item options before turning them back on
        ResetItemOptions();
    }
    public void OnArmorSlot()
    {
        currentItem = currentUnit.GetComponent<CharacterInventory>().equippedArmor;

        //Load the item info
        infoDisplay.LoadItemInfo(currentItem);

        //Reset the item options before turning them back on
        ResetItemOptions();
    }


    public void DisplayWeaponOptions()
    {
        //Store, Equip
        storeButton.transform.position = optionButtonLocations[0].position;
        storeButton.gameObject.SetActive(true);

        equipButton.transform.position = optionButtonLocations[1].position;
        equipButton.gameObject.SetActive(true);
    }

    public void DisplayArmorOptions()
    {
        //Store, Equip
        storeButton.transform.position = optionButtonLocations[0].position;
        storeButton.gameObject.SetActive(true);

        equipButton.transform.position = optionButtonLocations[1].position;
        equipButton.gameObject.SetActive(true);
    }

    public void DisplayConsumableOptions()
    {
        //Store, Use
        storeButton.transform.position = optionButtonLocations[0].position;
        storeButton.gameObject.SetActive(true);

        useButton.transform.position = optionButtonLocations[1].position;
        useButton.gameObject.SetActive(true);
    }

    public void DisplayAccessoryOptions()
    {
        //Store
        storeButton.transform.position = optionButtonLocations[0].position;
        storeButton.gameObject.SetActive(true);
    }


    //When the player presses on one of the CONVOY's items
    public void OnConvoySlotButton(int i)
    {
        currentItemIndex = i;
        currentItem = ConvoyManager.instance.GetItem(convoyIndex, currentItemIndex);

        //Reset the item options before turning them back on
        ResetItemOptions();

        //Load the item info
        infoDisplay.LoadItemInfo(currentItem);


        if (currentItem is Weapon)
        {
            DisplayConvoyWeaponOptions();
        }
        else if (currentItem is Armor)
        {
            DisplayConvoyArmorOptions();
        }
        else if (currentItem is Consumable)
        {
            DisplayConvoyConsumableOptions();
        }
        else if (currentItem is Accessory)
        {
            DisplayConvoyAccessoryOptions();
        }
    }


    public void DisplayConvoyWeaponOptions()
    {
        //Store, Equip
        takeButton.transform.position = optionConvoyButtonLocations[0].position;
        takeButton.gameObject.SetActive(true);

        equipConvoyButton.transform.position = optionConvoyButtonLocations[1].position;
        equipConvoyButton.gameObject.SetActive(true);
    }

    public void DisplayConvoyArmorOptions()
    {
        //Store, Equip
        takeButton.transform.position = optionConvoyButtonLocations[0].position;
        takeButton.gameObject.SetActive(true);

        equipConvoyButton.transform.position = optionConvoyButtonLocations[1].position;
        equipConvoyButton.gameObject.SetActive(true);
    }

    public void DisplayConvoyConsumableOptions()
    {
        //Store, Use
        takeButton.transform.position = optionConvoyButtonLocations[0].position;
        takeButton.gameObject.SetActive(true);

        useConvoyButton.transform.position = optionConvoyButtonLocations[1].position;
        useConvoyButton.gameObject.SetActive(true);
    }

    public void DisplayConvoyAccessoryOptions()
    {
        //Store
        takeButton.transform.position = optionConvoyButtonLocations[0].position;
        takeButton.gameObject.SetActive(true);
    }


    public void EquipItem()
    {
        CharacterInventory i = currentUnit.GetComponent<CharacterInventory>();

        //Store the old equipment to add back to the inventory
        Item oldEquip = null;
        if (currentItem is Weapon)
        {
            oldEquip = i.equippedWeapon;
        }
        else if (currentItem is Armor)
        {
            oldEquip = i.equippedArmor;
        }

        i.EquipNewItem(currentItem);
        i.RemoveItemByIndex(currentItemIndex);
        i.AddItem(oldEquip);

        ResetItemOptions();
        UpdateItemInfo();
        ResetCurrentItem();
    }
    public void UseItem()
    {
        Consumable c = currentItem as Consumable;
        c.OnUse(currentUnit);

        ResetItemOptions();
        UpdateItemInfo();
        ResetCurrentItem();
    }
    public void StoreItem()
    {
        currentUnit.GetComponent<CharacterInventory>().RemoveItemByIndex(currentItemIndex);
        ConvoyManager.instance.AddItem(currentItem);
        ResetItemOptions();
        UpdateItemInfo();
        ResetCurrentItem();

        //An item was put back into the convoy, so refresh it just in case player was viewing where its stored
        RefreshConvoy();
    }


    //These are item functions for items that are in the CONVOY
    public void EquipConvoyItem()
    {
        CharacterInventory i = currentUnit.GetComponent<CharacterInventory>();

        //Store the old equipment to add to the convoy
        Item oldEquip = null;
        if (currentItem is Weapon)
        {
            oldEquip = i.equippedWeapon;
        }
        else if (currentItem is Armor)
        {
            oldEquip = i.equippedArmor;
        }

        i.EquipNewItem(currentItem);
        ConvoyManager.instance.RemoveItem(convoyIndex, currentItemIndex);
        ConvoyManager.instance.AddItem(oldEquip);

        //Don't need these because the equipped weapon slot is seperate from the inventory
        //i.RemoveItemByIndex(currentItemIndex);
        //i.AddItem(oldEquip);

        ResetConvoyItemOptions();
        UpdateItemInfo();
        ResetCurrentItem();

        RefreshConvoy();
    }
    public void UseConvoyItem()
    {
        Consumable c = currentItem as Consumable;
        //Second argument is TRUE, because we're using the item from the convoy
        c.OnUse(currentUnit, true);

        //If the item was used up from the convoy, get rid of it
        if (c.uses == 0)
        {
            //Remove from convoy
            ConvoyManager.instance.RemoveItem(convoyIndex, currentItemIndex);
            //Destroy item
            Destroy(c.gameObject);
        }

        ResetConvoyItemOptions();
        UpdateItemInfo();
        ResetCurrentItem();
        
        //An item might've been removed if used up
        RefreshConvoy();
    }
    public void TakeItem()
    {
        //Only take the item if the character has space for it
        if (currentUnit.GetComponent<CharacterInventory>().IsFull())
        {
            Debug.Log("INVENTORY FULL");
            return;
        }

        //Remove item from convoy
        Item taken = ConvoyManager.instance.RemoveItem(convoyIndex, currentItemIndex);
        //Add item to character inventory
        currentUnit.GetComponent<CharacterInventory>().AddItem(taken);

        ResetConvoyItemOptions();
        UpdateItemInfo();
        ResetCurrentItem();

        //An item was put back into the convoy, so refresh it just in case player was viewing where its stored
        RefreshConvoy();
    }


    private void RefreshConvoy ()
    {
        LoadConvoySlots(convoyIndex);
    }


    public void ResetItemOptions()
    {

        storeButton.gameObject.SetActive(false);
        useButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);

    }
    public void ResetConvoyItemOptions()
    {

        takeButton.gameObject.SetActive(false);
        useConvoyButton.gameObject.SetActive(false);
        equipConvoyButton.gameObject.SetActive(false);

    }

    public void ResetCurrentItem()
    {
        currentItem = null;
        currentItemIndex = -1;

        //Turn off the item info
        infoDisplay.Deactivate();
    }


    private void ResetConvoyMenu ()
    {

    }
}
