using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {


    public GameObject wait;
    public GameObject attack;
    public GameObject items;
    public GameObject cancel;

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


    public void ToggleActionMenu(bool toggle)
    {
        if (toggle)
        {
            wait.SetActive(true);
            items.SetActive(true);
            cancel.SetActive(true);
            if (MouseControl.instance.CanAttack()) { attack.SetActive(true); }
        }
        else
        {
            wait.SetActive(false);
            items.SetActive(false);
            cancel.SetActive(false);
            attack.SetActive(false);
        }
    }
}
