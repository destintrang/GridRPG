using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour {


    public GameObject inventory;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void ButtonPress ()
    {
        ButtonManager.instance.ToggleActionMenu(false);
        inventory.SetActive(true);
        InventoryUI.instance.EnableInventoryButtons();
        MouseControl.instance.currentState = MouseControl.MouseState.INVENTORY;
    }
}
