using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cancel : MonoBehaviour {

    //Singleton
    public static Cancel instance;
    private void Awake()
    {
        instance = this;
    }


    public void UnitCancel ()
    {
        GameObject unit = MouseControl.instance.unitOfInterest;
        unit.transform.position = unit.GetComponent<UnitTracker>().GetLocation() + new Vector3(0.5f, 0.5f, 0);

        ButtonManager.instance.ToggleActionMenu(false);
        ReturnToMoving();
    }

    void ReturnToMoving ()
    {
        MouseControl.instance.ResetUnit();
        MouseControl.instance.currentState = MouseControl.MouseState.MOVING;
        MouseControl.instance.ToggleRedTilesSmall(false);
        MouseControl.instance.ToggleActionTiles(true);
    }
}
