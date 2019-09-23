using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {


    //The buttons
    public GameObject wait;
    public GameObject attack;
    public GameObject items;
    public GameObject cancel;
    public GameObject interact;

    public List<Transform> buttonPositions;


    private Interactable currentInteractable;


    //Singleton
    public static ButtonManager instance;
    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void ToggleActionMenuOn(Vector3Int tile)
    {
        TurnOnButtons(tile);
    }

    private void TurnOnButtons (Vector3Int tile)
    {
        List<GameObject> possibleActions = new List<GameObject>();

        //Turn on the attack button if the current unit can make an attack
        if (MouseControl.instance.CanAttack()) { possibleActions.Add(attack); }

        //Check if this tile is an interactable tile
        Interactable i = TileManager.instance.allTiles[tile].GetComponent<TileAttributes>().GetInteractable();
        if (i != null)
        {
            currentInteractable = i;
            interact.GetComponentInChildren<Text>().text = i.actionName;
            possibleActions.Add(interact);
        }

        //These options will always be available
        possibleActions.Add(items);
        possibleActions.Add(wait);
        possibleActions.Add(cancel);

        int counter = 0;
        foreach (GameObject b in possibleActions)
        {
            b.transform.position = buttonPositions[counter].position;
            b.SetActive(true);
            counter++;
        }
    }

    public void ToggleActionMenuOff ()
    {
        wait.SetActive(false);
        items.SetActive(false);
        cancel.SetActive(false);
        attack.SetActive(false);
        interact.SetActive(false);
    }


    //BUTTON FUNCTIONS
    public void OnAttackButton()
    {
        MouseControl.instance.currentState = MouseControl.MouseState.ATTACKING;
        ToggleActionMenuOff();
    }

    public void OnInteractButton ()
    {
        currentInteractable.OnInteract(MouseControl.instance.unitOfInterest);
        //OnWaitButton();
        ToggleActionMenuOff();
        MouseControl.instance.currentState = MouseControl.MouseState.DISABLED;
    }
    public void OnItemButton()
    {
        ButtonManager.instance.ToggleActionMenuOff();
        //inventory.SetActive(true);
        InventoryUI.instance.EnableInventoryButtons();
        MouseControl.instance.currentState = MouseControl.MouseState.INVENTORY;
    }

    public void OnWaitButton()
    {
        GridLayout myGrid = GameObject.Find("Grid").GetComponent<GridLayout>();
        GameObject unit = MouseControl.instance.unitOfInterest;
        unit.GetComponent<UnitTracker>().MoveTo(myGrid.LocalToCell(unit.transform.position));
        ToggleActionMenuOff();
        MouseControl.instance.currentState = MouseControl.MouseState.FINISHED;
    }

    public void OnCancelButton()
    {
        GameObject unit = MouseControl.instance.unitOfInterest;
        unit.transform.position = unit.GetComponent<UnitTracker>().GetLocation() + new Vector3(0.5f, 0.5f, 0);
        ToggleActionMenuOff();
        ReturnToMoving();
    }


    void ReturnToMoving()
    {
        MouseControl.instance.ResetUnit();
        MouseControl.instance.currentState = MouseControl.MouseState.MOVING;
        MouseControl.instance.ToggleRedTilesSmall(false);
        MouseControl.instance.ToggleActionTiles(true);
    }

}
