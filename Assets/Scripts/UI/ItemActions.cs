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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InventoryUI.instance.EnableInventoryButtons();
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
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


    public void EquipButton()
    {
        MouseControl.instance.unitOfInterest.GetComponent<CharacterInventory>().EquipItem(itemIndex);
        Finish();
    }

    public void UseButton ()
    {
        MouseControl.instance.unitOfInterest.GetComponent<CharacterInventory>().UseItem(itemIndex);
        Finish();
    }

    public void DropButton ()
    {
        MouseControl.instance.unitOfInterest.GetComponent<CharacterInventory>().DropItem(itemIndex);
        Finish();
    }

    void Finish ()
    {
        InventoryUI.instance.gameObject.SetActive(false);
        MouseControl.instance.Reset();
        gameObject.SetActive(false);
        //As of right now, a unit's turn will end after any item action
        MouseControl.instance.currentState = MouseControl.MouseState.FINISHED;
    }
}
