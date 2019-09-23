using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    //Singleton
    public static Attack instance;
    private void Awake()
    {
        instance = this;
    }


    public void UnitAttack ()
    {
        MouseControl.instance.currentState = MouseControl.MouseState.ATTACKING;
        ButtonManager.instance.ToggleActionMenuOff();
    }
}
