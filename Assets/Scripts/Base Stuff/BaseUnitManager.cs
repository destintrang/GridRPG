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

    public Button deployButton;
    public Button undeployButton;

    //Deactivate this to hide ALL stats UI (when no unit is being looked at)
    public UnitInfoDisplay infoDisplay;

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

    public Text deployCount;
    public Text deployMax;

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
        if (currentUnit != null)
        {
            ResetUnitMenu();
        }
        else
        {
            BaseManager.instance.CancelButton();
            ResetUnitMenu();
        }
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

        UpdateDeployNumber();

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

        UpdateUnitInfo();
    }

    private void UpdateUnitInfo ()
    {
        UpdateDeployButton();

        CharacterStats c = currentUnit.GetComponent<CharacterStats>();

        //unitName.text = c.GetName();
        //maxhp.text = c.GetMaxHealth().ToString();
        //hp.text = c.GetHealth().ToString();
        //str.text = c.GetStrength().ToString();
        //mag.text = c.GetMagic().ToString();
        //dex.text = c.GetDexterity().ToString();
        //spd.text = c.GetSpeed().ToString();
        //def.text = c.GetDefense().ToString();
        //fth.text = c.GetFaith().ToString();
        //lck.text = c.GetLuck().ToString();
        //mov.text = c.GetMovement().ToString();

        infoDisplay.LoadUnitInfo(currentUnit);
    }


    private void UpdateDeployButton ()
    {
        //Deploy button can be DEPLOY, UNDEPLOY, or (INACTIVE) DEPLOY

        UpdateDeployNumber();


        if (currentUnit == null)
        {
            deployButton.gameObject.SetActive(false);
            undeployButton.gameObject.SetActive(false);
            return;
        }

        if (BaseRepositionManager.instance.IsDeployed(currentUnit))
        {
            deployButton.gameObject.SetActive(false);
            undeployButton.gameObject.SetActive(true);

            if (BaseRepositionManager.instance.CanUndeploy())
            {
                undeployButton.interactable = true;
            }
            else
            {
                //If the unit can't be undeployed, you can't press the button
                undeployButton.interactable = false;
            }
        }
        else
        {
            deployButton.gameObject.SetActive(true);
            undeployButton.gameObject.SetActive(false);

            if (BaseRepositionManager.instance.CanDeploy())
            {
                deployButton.interactable = true;
            }
            else
            {
                //If the unit can't be deployed, you can't press the button
                deployButton.interactable = false;
            }
        }

    }

    private void UpdateDeployNumber()
    {
        //Update the deploy numbers
        deployCount.text = BaseRepositionManager.instance.GetDeployedCount().ToString();
        deployMax.text = BaseRepositionManager.instance.GetDeployedMax().ToString();
    }


    public void DeployCurrent ()
    {
        BaseRepositionManager.instance.DeployUnit(currentUnit);
        UpdateDeployButton();
    }
    public void UndeployCurrent()
    {
        BaseRepositionManager.instance.UndeployUnit(currentUnit);
        UpdateDeployButton();
    }



    private void ResetUnitMenu ()
    {
        //Turn off the stats UI
        infoDisplay.Deactivate();

        currentUnit = null;
        unitName.text = "Unit name";

        //Update the deploy button
        UpdateDeployButton();
    }

}
