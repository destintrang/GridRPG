using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {


    public GameObject weapon;
    public GameObject armor;
    public GameObject itemOne;
    public GameObject itemTwo;
    public GameObject itemThree;

    public bool active;

    //Singleton
    public static InventoryUI instance;
    private void Awake()
    {
        instance = this;
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (active && Input.GetKeyDown(KeyCode.Escape))
        {
            ButtonManager.instance.ToggleActionMenu(true);
            gameObject.SetActive(false);
            MouseControl.instance.currentState = MouseControl.MouseState.SELECTACTION;
        }
	}

    private void OnEnable()
    {
        CharacterInventory i = MouseControl.instance.unitOfInterest.GetComponent<CharacterInventory>();
        active = true;

        weapon.SetActive(true);
        weapon.GetComponent<InventorySlot>().InitializeSlot(i.equippedWeapon, 1);
        armor.SetActive(true);
        armor.GetComponent<InventorySlot>().InitializeSlot(i.equippedArmor, 2);
        itemOne.SetActive(true);
        itemOne.GetComponent<InventorySlot>().InitializeSlot(i.inventory[0], 3);
        itemTwo.SetActive(true);
        itemTwo.GetComponent<InventorySlot>().InitializeSlot(i.inventory[1], 4);
        itemThree.SetActive(true);
        itemThree.GetComponent<InventorySlot>().InitializeSlot(i.inventory[2], 5);
    }


    public void EnableInventoryButtons()
    {
        active = true;
        weapon.GetComponent<Button>().interactable = true;
        armor.GetComponent<Button>().interactable = true;
        itemOne.GetComponent<Button>().interactable = true;
        itemTwo.GetComponent<Button>().interactable = true;
        itemThree.GetComponent<Button>().interactable = true;
    }

    public void DisableInventoryButtons ()
    {
        active = false;
        weapon.GetComponent<Button>().interactable = false;
        armor.GetComponent<Button>().interactable = false;
        itemOne.GetComponent<Button>().interactable = false;
        itemTwo.GetComponent<Button>().interactable = false;
        itemThree.GetComponent<Button>().interactable = false;
    }
}
