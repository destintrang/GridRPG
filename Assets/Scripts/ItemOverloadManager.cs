using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOverloadManager : MonoBehaviour
{


    //The parent gameObject that holds the other buttons
    public GameObject inventory;

    public GameObject weapon;
    public GameObject armor;
    public GameObject itemOne;
    public GameObject itemTwo;
    public GameObject itemThree;
    //Overloaded item
    public GameObject overloadedItem;


    public GameObject equip;
    public GameObject use;
    public GameObject drop;

    public List<Transform> buttonLocations;


    private List<Item> toGive;
    private Item currentOverloadedItem;
    private Item currentItem;


    //Singleton
    public static ItemOverloadManager instance;
    private void Awake()
    {
        instance = this;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { EscapeButton(); }
    }


    public void StartOverload (List<Item> items)
    {
        //Disable the mouse
        MouseControl.instance.currentState = MouseControl.MouseState.DISABLED;

        toGive = items;

        LoadNext();
    }
    void LoadNext ()
    {
        LoadUI(toGive[0]);
        toGive.RemoveAt(0);
    }

    private void LoadUI (Item overload)
    {
        currentOverloadedItem = overload;
        EnableInventoryButtons();
    }


    public void EnableInventoryButtons()
    {
        //active = true;
        weapon.GetComponent<Button>().interactable = true;
        armor.GetComponent<Button>().interactable = true;
        itemOne.GetComponent<Button>().interactable = true;
        itemTwo.GetComponent<Button>().interactable = true;
        itemThree.GetComponent<Button>().interactable = true;

        inventory.SetActive(true);

        SetItemButtons();
    }

    private void SetItemButtons()
    {
        CharacterInventory i = MouseControl.instance.unitOfInterest.GetComponent<CharacterInventory>();
        //active = true;

        overloadedItem.GetComponentInChildren<Text>().text = currentOverloadedItem.GetName();

        weapon.SetActive(true);
        //weapon.GetComponent<InventorySlot>().InitializeSlot(i.equippedWeapon, 1);
        weapon.GetComponentInChildren<Text>().text = i.equippedWeapon.GetName();
        armor.SetActive(true);
        //armor.GetComponent<InventorySlot>().InitializeSlot(i.equippedArmor, 2);
        armor.GetComponentInChildren<Text>().text = i.equippedArmor.GetName();
        itemOne.SetActive(true);
        //itemOne.GetComponent<InventorySlot>().InitializeSlot(i.inventory[0], 3);
        itemOne.GetComponentInChildren<Text>().text = i.inventory[0].GetName();
        itemTwo.SetActive(true);
        //itemTwo.GetComponent<InventorySlot>().InitializeSlot(i.inventory[1], 4);
        itemTwo.GetComponentInChildren<Text>().text = i.inventory[1].GetName();
        itemThree.SetActive(true);
        //itemThree.GetComponent<InventorySlot>().InitializeSlot(i.inventory[2], 5);
        itemThree.GetComponentInChildren<Text>().text = i.inventory[2].GetName();
    }
    public void DisableInventoryButtons()
    {
        //active = false;
        weapon.GetComponent<Button>().interactable = false;
        armor.GetComponent<Button>().interactable = false;
        itemOne.GetComponent<Button>().interactable = false;
        itemTwo.GetComponent<Button>().interactable = false;
        itemThree.GetComponent<Button>().interactable = false;
        overloadedItem.GetComponent<Button>().interactable = false;
    }


    public void OnButtonPress (int index)
    {
        currentItem = MouseControl.instance.unitOfInterest.GetComponent<CharacterInventory>().inventory[index];
        ActivateButtons(currentItem);
        DisableInventoryButtons();
    }


    public void ActivateButtons(Item i)
    {
        currentItem = i;
        List<GameObject> buttons = new List<GameObject>();

        if (currentItem is Weapon || currentItem is Armor)
        {
            buttons.Add(equip);
        }

        if (currentItem.usable)
        {
            buttons.Add(use);
        }

        buttons.Add(drop);

        int counter = 0;
        foreach (GameObject button in buttons)
        {
            button.transform.position = buttonLocations[counter].position;
            button.SetActive(true);
            counter++;
        }
    }


    public void DeactivateActionButtons()
    {
        equip.SetActive(false);
        use.SetActive(false);
        drop.SetActive(false);
    }


    public void EquipButton()
    {
        CharacterInventory inventory = MouseControl.instance.unitOfInterest.GetComponent<CharacterInventory>();

        inventory.RemoveItemByID(currentItem.GetInstanceID());
        Item oldEquip = inventory.EquipNewItem(currentItem);
        inventory.AddItem(oldEquip);

        //Update the items
        ResetButtons();
    }

    public void UseButton()
    {
        //MouseControl.instance.unitOfInterest.GetComponent<CharacterInventory>().UseItem(itemIndex);
        (currentItem as Consumable).OnUse(MouseControl.instance.unitOfInterest);

        CheckFinished();

        //Update the items
        ResetButtons();
    }

    public void DropButton()
    {
        MouseControl.instance.unitOfInterest.GetComponent<CharacterInventory>().RemoveItemByID(currentItem.GetInstanceID());
        CheckFinished();
    }


    private void CheckFinished ()
    {
        if (!MouseControl.instance.unitOfInterest.GetComponent<CharacterInventory>().IsFull())
        {

            MouseControl.instance.unitOfInterest.GetComponent<CharacterInventory>().AddItem(currentOverloadedItem);

            if (toGive.Count > 0)
            {
                ResetButtons();
                LoadNext();
            }
            else
            {
                Finish();
            }

        }
    }

    void Finish()
    {
        inventory.SetActive(false);
        MouseControl.instance.Reset();

        //As of right now, a unit's turn will end after any item action
        MouseControl.instance.currentState = MouseControl.MouseState.FINISHED;
    }

    void EscapeButton ()
    {
        DeactivateActionButtons();

        weapon.GetComponent<Button>().interactable = true;
        armor.GetComponent<Button>().interactable = true;
        itemOne.GetComponent<Button>().interactable = true;
        itemTwo.GetComponent<Button>().interactable = true;
        itemThree.GetComponent<Button>().interactable = true;
        overloadedItem.GetComponent<Button>().interactable = true;
    }


    void ResetButtons ()
    {
        SetItemButtons();

        weapon.GetComponent<Button>().interactable = true;
        armor.GetComponent<Button>().interactable = true;
        itemOne.GetComponent<Button>().interactable = true;
        itemTwo.GetComponent<Button>().interactable = true;
        itemThree.GetComponent<Button>().interactable = true;
        overloadedItem.GetComponent<Button>().interactable = true;

        DeactivateActionButtons();
    }

}
