using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseItemsManager : BaseMenu
{


    private List<GameObject> party;
    private GameObject currentUnit;

    //References to each unit's button 
    public List<Button> unitButtons;


    public Text unitName;

    public Button weapon;
    public Button armor;
    public Button itemA;
    public Button itemB;
    public Button itemC;


    private Item currentItem;
    private int currentItemIndex;

    public Button storeButton;
    public Button useButton;
    public Button equipButton;

    public List<Transform> optionButtonLocations;

    //Canvas object that displays item info
    public ItemInfoDisplay infoDisplay;


    //Singleton
    public static BaseItemsManager instance;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void SpaceBar()
    {
        if (currentUnit != null)
        {
            ResetItemOptions();
            ResetCurrentItem();
            BaseManager.instance.ConvoyButton(currentUnit);
        }
    }

    public override void Escape()
    {
        if (currentItem != null)
        {
            ResetItemOptions();
            ResetCurrentItem();
        }
        else if (currentUnit != null)
        {
            ResetItemMenu();
            ResetItemOptions();
        }
        else
        {
            BaseManager.instance.CancelButton();
            ResetItemMenu();
        }
    }


    public void LoadPartyUnits()
    {

        party = PartyManager.instance.GetParty();
        int counter = 0;

        foreach (GameObject u in party)
        {
            unitButtons[counter].gameObject.SetActive(true);
            unitButtons[counter].GetComponentInChildren<Text>().text = u.GetComponent<CharacterStats>().GetName();
            counter++;
        }
    }


    public void LoadItemInfo(int i)
    {
        //Set the unit that we are currently looking at
        currentUnit = party[i];

        //If the item options were on, turn them off here
        ResetCurrentItem();
        ResetItemOptions();

        unitName.text = party[i].GetComponent<CharacterStats>().GetName();
        UpdateItemInfo();
    }


    //Call this when you need to correctly display the unit's equipment and items
    public void UpdateItemInfo()
    {
        CharacterInventory items = currentUnit.GetComponent<CharacterInventory>();

        weapon.GetComponentInChildren<Text>().text = items.equippedWeapon.GetName();
        weapon.gameObject.SetActive(true);

        armor.GetComponentInChildren<Text>().text = items.equippedArmor.GetName();
        armor.gameObject.SetActive(true);


        if (items.inventory[0])
        {
            itemA.gameObject.SetActive(true);
            itemA.GetComponentInChildren<Text>().text = items.inventory[0].GetName();
        }
        else
        {
            itemA.gameObject.SetActive(false);
        }
        if (items.inventory[1])
        {
            itemB.gameObject.SetActive(true);
            itemB.GetComponentInChildren<Text>().text = items.inventory[1].GetName();
        }
        else
        {
            itemB.gameObject.SetActive(false);
        }
        if (items.inventory[2])
        {
            itemC.gameObject.SetActive(true);
            itemC.GetComponentInChildren<Text>().text = items.inventory[2].GetName();
        }
        else
        {
            itemC.gameObject.SetActive(false);
        }
    }


    public void OnItemSlotButton (int i)
    {
        currentItem = currentUnit.GetComponent<CharacterInventory>().inventory[i];
        currentItemIndex = i;

        //Load the item info
        infoDisplay.LoadItemInfo(currentItem);

        //Reset the item options before turning them back on
        ResetItemOptions();


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

    public void OnWeaponSlot ()
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


    public void DisplayWeaponOptions ()
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


    public void EquipItem ()
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
    public void StoreItem ()
    {
        currentUnit.GetComponent<CharacterInventory>().RemoveItemByIndex(currentItemIndex);
        ConvoyManager.instance.AddItem(currentItem);

        ResetItemOptions();
        UpdateItemInfo();
        ResetCurrentItem();
    }


    private void ResetItemSlots ()
    {
        weapon.gameObject.SetActive(false);
        armor.gameObject.SetActive(false);
        itemA.gameObject.SetActive(false);
        itemB.gameObject.SetActive(false);
        itemC.gameObject.SetActive(false);
    }


    public void ResetItemOptions ()
    {

        storeButton.gameObject.SetActive(false);
        useButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);

    }

    public void ResetCurrentItem ()
    {
        currentItem = null;
        currentItemIndex = -1;

        //Turn off the item info
        infoDisplay.Deactivate();
    }

    private void ResetItemMenu()
    {
        currentUnit = null;
        unitName.text = "Unit name";
        ResetItemOptions();
        ResetItemSlots();
    }
}
