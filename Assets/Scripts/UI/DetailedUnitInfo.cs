using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailedUnitInfo : MonoBehaviour {


    GridLayout myGrid;
    float maxBarWidth;
    const KeyCode escape = KeyCode.Escape;

    public GameObject unitDetails;
    public float fadeSpeed;
    public float barSpeed;
    public int statMax;
    
    public Text strength;
    public Image strengthBar;
    public Text magic;
    public Image magicBar;
    public Text agiliy;
    public Image agilityBar;
    public Text dexterity;
    public Image dexterityBar;
    public Text defense;
    public Image defenseBar;
    public Text faith;
    public Image faithBar;
    public Text build;
    public Text move;

    public Text weapon;
    public Text armor;
    public Text itemOne;
    public Text itemTwo;
    public Text itemThree;

    public Text damage;
    public Text damageType;
    public Text accuracy;
    public Text critChance;
    public Text combatSpeed;
    public Text cutResist;
    public Text stabResist;
    public Text bashResist;
    public Text critEvade;
    public Text evasion;

    bool active;
    MouseControl.MouseState temp;


	// Use this for initialization
	void Start ()
    {
        myGrid = GameObject.Find("Grid").GetComponent<GridLayout>();
        maxBarWidth = strengthBar.rectTransform.rect.width;
        ZeroBars();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) &&
            MouseControl.instance.currentState != MouseControl.MouseState.DISABLED && 
            TurnManager.instance.currentState == TurnManager.TurnState.BLUE &&
            !Darkness.instance.fading)
        {
            Vector3 clicked = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clicked.z = 0;
            Vector3Int coord = myGrid.LocalToCell(clicked);

            if (TileManager.instance.allTiles.ContainsKey(coord) && TileManager.instance.allUnits.ContainsKey(coord))
            {
                LoadUnitInfo(TileManager.instance.allUnits[coord]);
                StartCoroutine(DisplayUnitInfo(TileManager.instance.allUnits[coord]));
            }
        }

        if (active && Input.GetKeyDown(escape))
        {
            StartCoroutine(HideUnitInfo());
        }
    }


    IEnumerator DisplayUnitInfo (GameObject unit)
    {
        active = true;
        temp = MouseControl.instance.currentState;
        MouseControl.instance.currentState = MouseControl.MouseState.DISABLED;

        Darkness.instance.FadeIn();
        while (Darkness.instance.fading) { yield return null; }

        //Switch in the info here
        LoadBars(unit);
        unitDetails.SetActive(true);
        
        Darkness.instance.FadeOut();
        while (Darkness.instance.fading) { yield return null; }
    }

    IEnumerator HideUnitInfo()
    {
        Darkness.instance.FadeIn();
        while (Darkness.instance.fading) { yield return null; }

        //Switch out the info here
        unitDetails.SetActive(false);
        ZeroBars();

        Darkness.instance.FadeOut();
        while (Darkness.instance.fading) { yield return null; }

        MouseControl.instance.currentState = temp;
        active = false;
    }

    void LoadUnitInfo (GameObject unit)
    {
        CharacterStats stats = unit.GetComponent<CharacterStats>();
        CharacterInventory items = unit.GetComponent<CharacterInventory>();

        strength.text = stats.GetStrength().ToString();
        magic.text = stats.GetMagic().ToString();
        agiliy.text = stats.GetSpeed().ToString();
        dexterity.text = stats.GetDexterity().ToString();
        defense.text = stats.GetDefense().ToString();
        faith.text = stats.GetFaith().ToString();
        build.text = stats.GetBuild().ToString();
        move.text = stats.GetMovement().ToString();
        
        if (items.equippedWeapon != null) { weapon.text = items.equippedWeapon.name; }
        else { weapon.text = "None"; }
        if (items.equippedArmor != null) { armor.text = items.equippedArmor.name; }
        else { armor.text = "None"; }
        if (items.inventory[0] != null) { itemOne.text = items.inventory[0].name; }
        else { itemOne.text = "None"; }
        if (items.inventory[1] != null) { itemTwo.text = items.inventory[1].name; }
        else { itemTwo.text = "None"; }
        if (items.inventory[2] != null) { itemThree.text = items.inventory[2].name; }
        else { itemThree.text = "None"; }

        damage.text = stats.GetCombatAttack().ToString();
        if (items.equippedWeapon != null) { damageType.text = items.equippedWeapon.damageType.ToString(); }
        else { damageType.text = "None"; }
        accuracy.text = stats.GetAccuracy().ToString();
        critChance.text = stats.GetCritDamage().ToString();
        combatSpeed.text = stats.GetCombatSpeed().ToString();
        if (items.equippedArmor != null)
        {
            cutResist.text = (stats.GetCombatDefense() + items.equippedArmor.cutBonus).ToString();
            stabResist.text = (stats.GetCombatDefense() + items.equippedArmor.stabBonus).ToString();
            bashResist.text = (stats.GetCombatDefense() + items.equippedArmor.bashBonus).ToString();
        } else
        {
            cutResist.text = stats.GetCombatDefense().ToString();
            stabResist.text = stats.GetCombatDefense().ToString();
            bashResist.text = stats.GetCombatDefense().ToString();
        }
        critEvade.text = stats.GetCritDefense().ToString();
        evasion.text = stats.GetCombatEvasion().ToString();
    }

    void LoadBars (GameObject unit)
    {
        CharacterStats stats = unit.GetComponent<CharacterStats>();
        StartCoroutine(StartBar(strengthBar, stats.GetStrength()));
        StartCoroutine(StartBar(magicBar, stats.GetMagic()));
        StartCoroutine(StartBar(agilityBar, stats.GetSpeed()));
        StartCoroutine(StartBar(dexterityBar, stats.GetDexterity()));
        StartCoroutine(StartBar(defenseBar, stats.GetDefense()));
        StartCoroutine(StartBar(faithBar, stats.GetFaith()));
    }

    IEnumerator StartBar (Image bar, int number)
    {
        Vector2 originalSize = bar.rectTransform.rect.size;
        float targetWidth = ((float) number / (float) statMax) * maxBarWidth;
        while (bar.rectTransform.rect.width < targetWidth)
        {
            bar.rectTransform.sizeDelta = Vector2.Lerp(bar.rectTransform.rect.size, new Vector2(((float) number / (float) statMax) * maxBarWidth, bar.rectTransform.rect.height), barSpeed * Time.deltaTime);
            if (Input.GetKeyDown(escape)) { yield break; }
            yield return null;
        }
    }

    void ZeroBars ()
    {
        strengthBar.rectTransform.sizeDelta = new Vector2(0, strengthBar.rectTransform.sizeDelta.y);
        magicBar.rectTransform.sizeDelta = new Vector2(0, magicBar.rectTransform.sizeDelta.y);
        agilityBar.rectTransform.sizeDelta = new Vector2(0, agilityBar.rectTransform.sizeDelta.y);
        dexterityBar.rectTransform.sizeDelta = new Vector2(0, dexterityBar.rectTransform.sizeDelta.y);
        defenseBar.rectTransform.sizeDelta = new Vector2(0, defenseBar.rectTransform.sizeDelta.y);
        faithBar.rectTransform.sizeDelta = new Vector2(0, faithBar.rectTransform.sizeDelta.y);
    }
}
