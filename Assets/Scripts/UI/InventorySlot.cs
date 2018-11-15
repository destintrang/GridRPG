using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {


    public Text itemName;
    public GameObject itemActions;
    public bool equipmentSlot;

    Item i;
    int index;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitializeSlot (Item unitItem, int _index)
    {
        i = unitItem;
        index = _index;

        if (unitItem != null)
        {
            itemName.text = unitItem.name;
        } else
        {
            itemName.transform.parent.gameObject.SetActive(false);
        }
    }


    public void ButtonPress ()
    {
        itemActions.GetComponent<ItemActions>().item = i;
        itemActions.GetComponent<ItemActions>().itemIndex = index;
        itemActions.GetComponent<ItemActions>().isEquipped = equipmentSlot;
        itemActions.SetActive(true);
        itemActions.transform.position = Input.mousePosition;
        InventoryUI.instance.DisableInventoryButtons();
    }
}
