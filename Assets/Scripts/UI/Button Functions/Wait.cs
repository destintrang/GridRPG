using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : MonoBehaviour {

    
    GridLayout myGrid;


    // Use this for initialization
    void Start ()
    {
        myGrid = GameObject.Find("Grid").GetComponent<GridLayout>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void UnitWait ()
    {
        GameObject unit = MouseControl.instance.unitOfInterest;
        unit.GetComponent<UnitTracker>().MoveTo(myGrid.LocalToCell(unit.transform.position));
        ButtonManager.instance.ToggleActionMenu(false);
        MouseControl.instance.currentState = MouseControl.MouseState.FINISHED;
    }
}
