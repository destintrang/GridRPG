using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoDisplay : MonoBehaviour
{


    //This holds everything
    public GameObject everything;

    public Text unitName;
    public Text unitRank;

    public Text unitMaxHealth;
    public Text unitHealth;
    public Text unitStrength;
    public Text unitMagic;
    public Text unitDexterity;
    public Text unitSpeed;
    public Text unitDefense;
    public Text unitFaith;
    public Text unitLuck;
    public Text unitBuild;
    public Text unitMove;

    public Text combatRange;
    public Text combatAttack;
    public Text combatCritAttack;
    public Text combatCritDefense;
    public Text combatSlashDefense;
    public Text combatStabDefense;
    public Text combatSmashDefense;
    public Text combatMagicDefense;
    public Text combatSpeed;
    public Text combatAccuracy;
    public Text combatEvasion;


    public void LoadUnitInfo (GameObject unit)
    {

        //Activate the parent obj
        everything.SetActive(true);

        CharacterStats c = unit.GetComponent<CharacterStats>();

        LoadText(unitName, c.GetName());
        LoadText(unitRank, c.GetRank().ToString());

        LoadText(unitMaxHealth, c.GetMaxHealth().ToString());
        LoadText(unitHealth, c.GetHealth().ToString());
        LoadText(unitStrength, c.GetStrength().ToString());
        LoadText(unitMagic, c.GetMagic().ToString());
        LoadText(unitDexterity, c.GetDexterity().ToString());
        LoadText(unitSpeed, c.GetSpeed().ToString());
        LoadText(unitDefense, c.GetDefense().ToString());
        LoadText(unitFaith, c.GetFaith().ToString());
        LoadText(unitLuck, c.GetLuck().ToString());
        LoadText(unitBuild, c.GetBuild().ToString());
        LoadText(unitMove, c.GetMovement().ToString());

        //LoadText(combatRange, c.GetRank().ToString());
        LoadText(combatAttack, c.GetCombatAttack().ToString());
        LoadText(combatCritAttack, c.GetCritDamage().ToString());
        LoadText(combatSlashDefense, c.GetCombatSlashDefense().ToString());
        LoadText(combatStabDefense, c.GetCombatStabDefense().ToString());
        LoadText(combatSmashDefense, c.GetCombatSmashDefense().ToString());
        LoadText(combatCritDefense, c.GetCritDefense().ToString());
        //LoadText(combatMagicDefense, c.GetCombatAttack().ToString());
        LoadText(combatSpeed, c.GetCombatSpeed().ToString());
        LoadText(combatAccuracy, c.GetAccuracy().ToString());
        LoadText(combatEvasion, c.GetCombatEvasion().ToString());

    }


    public void Deactivate()
    {
        everything.SetActive(false);
    }


    private void LoadText (Text t, string info)
    {
        //If this display doesn't need this info, just skip it
        if (t == null) { return; }

        t.text = info;
    }

}
