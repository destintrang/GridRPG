using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUnitManager : BaseMenu
{

    private List<GameObject> party;
    private GameObject currentUnit;

    //References to each unit's button 
    public List<Button> unitButtons;

    //Deactivate this to hide ALL stats UI (when no unit is being looked at)
    public GameObject statsUI;

    public Text unitName;
    public Text maxhp;
    public Text hp;
    public Text str;
    public Text mag;
    public Text dex;
    public Text spd;
    public Text def;
    public Text fth;
    public Text lck;
    public Text mov;

    const KeyCode LEVELKEY = KeyCode.E;
    const KeyCode HEALKEY = KeyCode.Space;


    //Singleton
    public static BaseUnitManager instance;
    private void Awake()
    {
        instance = this;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public override void E()
    {
        if (currentUnit != null)
        {
            currentUnit.GetComponent<CharacterStats>().RankUp();
            UpdateUnitInfo();
        }
    }

    public override void SpaceBar()
    {
        if (currentUnit != null)
        {
            currentUnit.GetComponent<CharacterStats>().Heal(10);
            UpdateUnitInfo();
        }
    }

    public override void Escape()
    {
        BaseManager.instance.CancelButton();
        ResetUnitMenu();
    }

    public override void D()
    {
        if (currentUnit != null)
        {
            if (BaseRepositionManager.instance.CanDeploy())
            {
                Debug.Log("DEPLOY!");
            }
        }
    }



    public void LoadPartyUnits ()
    {

        party = PartyManager.instance.GetParty();
        int counter = 0;

        foreach (GameObject u in party)
        {
            unitButtons[counter].gameObject.SetActive(true);
            unitButtons[counter].GetComponentInChildren<Text>().text = u.GetComponent<CharacterStats>().GetName();
            counter++;
        }
    }


    public void LoadUnitInfo (int i)
    {
        //Set the unit that we are currently looking at
        currentUnit = party[i];

        CharacterStats stats = currentUnit.GetComponent<CharacterStats>();

        unitName.text = party[i].GetComponent<CharacterStats>().GetName();
        UpdateUnitInfo();
    }

    private void UpdateUnitInfo ()
    {
        CharacterStats c = currentUnit.GetComponent<CharacterStats>();
        maxhp.text = c.GetMaxHealth().ToString();
        hp.text = c.GetHealth().ToString();
        str.text = c.GetStrength().ToString();
        mag.text = c.GetMagic().ToString();
        dex.text = c.GetDexterity().ToString();
        spd.text = c.GetSpeed().ToString();
        def.text = c.GetDefense().ToString();
        fth.text = c.GetFaith().ToString();
        lck.text = c.GetLuck().ToString();
        mov.text = c.GetMovement().ToString();

        statsUI.SetActive(true);
    }



    private void ResetUnitMenu ()
    {
        //Turn off the stats UI
        statsUI.SetActive(false);

        currentUnit = null;
        unitName.text = "Unit name";
    }

}
