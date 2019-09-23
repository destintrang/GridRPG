using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemActions : MonoBehaviour {


    public int itemIndex;
    public Item item;
    public bool isEquipped;

    public GameObject equip;
    public GameObject use;
    public GameObject drop;

    public List<Transform> buttonLocations;


    //Singleton
    public static ItemActions instance;
    private void Awake()
    {
        instance = this;
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    InventoryUI.instance.EnableInventoryButtons();
        //    gameObject.SetActive(false);
        //}
    }

    private void NotUsedAnymore()
    {
        //Drop should almost always be enabled, since a unit should always be able to drop an item

        if ((item is Weapon || item is Armor) && !isEquipped)
        {
            //Display the Equip action; this item can be equipped
            equip.SetActive(true);
        } else
        {
            equip.SetActive(false);
        }

        if (item.usable)
        {
            //Display the Use action; this item can be used
            use.SetActive(true);
        } else
        {
            use.SetActive(false);
        }
    }


    public void ActivateButtons (Item i)
    {
        item = i;
        List<GameObject> buttons = new List<GameObject>();

        if (item is Weapon || item is Armor)
        {
            buttons.Add(equip);
        }

        if (item.usable)
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


    public void DeactivateButtons ()
    {
        equip.SetActive(false);
        use.SetActive(false);
        drop.SetActive(false);
    }


    public void EquipButton()
    {
        CharacterInventory inventory = MouseControl.instance.unitOfInterest.GetComponent<CharacterInventory>();

        inventory.RemoveItemByID(item.GetInstanceID());
        Item oldEquip = inventory.EquipNewItem(item);
        inventory.AddItem(oldEquip);

        Finish();
    }

    public void UseButton ()
    {
        //MouseControl.instance.unitOfInterest.GetComponent<CharacterInventory>().UseItem(itemIndex);
        (item as Consumable).OnUse(MouseControl.instance.unitOfInterest);
        Finish();
    }

    public void DropButton ()
    {
        MouseControl.instance.unitOfInterest.GetComponent<CharacterInventory>().RemoveItemByID(item.GetInstanceID());
        Finish();
    }

    void Finish ()
    {
        InventoryUI.instance.inventory.SetActive(false);
        MouseControl.instance.Reset();
        gameObject.SetActive(false);
        //As of right now, a unit's turn will end after any item action
        MouseControl.instance.currentState = MouseControl.MouseState.FINISHED;
    }
}
