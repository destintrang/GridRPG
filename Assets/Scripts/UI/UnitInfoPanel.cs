using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoPanel : MonoBehaviour {

    float index;
    GridLayout myGrid;

    GameObject focus;
    Vector3Int focusCoord = new Vector3Int();

    public Vector3 originalPos;

    public GameObject panel;
    public Text unitName;
    public Text currentHealth;
    public Text maxHealth;
    public Image healthBar;
    public Text weapon;
    public Text armor;

    public Text attack;
    public Text defense;
    public Text evasion;
    public Text accuracy;
    public Text critChance;
    public Text critEvade;
    public Text speed;

    //Singleton
    public static UnitInfoPanel instance;
    private void Awake()
    {
        instance = this;
    }


    // Use this for initialization
    void Start () {
        //index = 0;
        originalPos = panel.transform.position;
        myGrid = GameObject.Find("Grid").GetComponent<GridLayout>();
    }
	
	// Update is called once per frame
	void Update () {
        if (MouseControl.instance.currentState != MouseControl.MouseState.DISABLED && TurnManager.instance.currentState == TurnManager.TurnState.BLUE)
        {
            Vector3 clicked = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clicked.z = 0;
            Vector3Int coord = myGrid.LocalToCell(clicked);

            if (TileManager.instance.allTiles.ContainsKey(coord) && TileManager.instance.allUnits.ContainsKey(coord))
            {
                SetUnitInfo(TileManager.instance.allUnits[coord].GetComponent<CharacterStats>());
                MoveIn(coord);

                if (focus == null && TileManager.instance.allUnits[coord].tag == "Blue")
                {
                    focus = TileManager.instance.allUnits[coord];
                    focus.GetComponent<Animator>().SetTrigger("Focus");
                    focusCoord = coord;
                } 
            }
            else
            {
                MoveOut();
            }

            if (focus != null && coord != focusCoord)
            {
                focus.GetComponent<Animator>().SetTrigger("Idle");
                focus = null;
            }
        } else
        {
            MoveOut();

            //if (focus != null)
            //{
            //    focus.GetComponent<Animator>().SetBool("Focus", false);
            //    focus = null;
            //}
        }
    }


    void SetUnitInfo (CharacterStats unit)
    {
        unitName.text = unit.name;
        currentHealth.text = unit.GetHealth().ToString();
        maxHealth.text = unit.GetMaxHealth().ToString();

        //Setting the size of the health bar
        //Default, full width is 100
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(((float)unit.GetHealth()) / ((float)unit.GetMaxHealth()) * 100, 100);

        if (unit.GetWeapon() != null) { weapon.text = unit.GetWeapon().name; }
        else { weapon.text = "None"; }
        if (unit.GetArmor() != null) { armor.text = unit.GetArmor().name; }
        else { armor.text = "None"; }

        attack.text = unit.GetCombatAttack().ToString();
        defense.text = unit.GetCombatDefense().ToString();
        evasion.text = unit.GetCombatEvasion().ToString();
        accuracy.text = unit.GetAccuracy().ToString();
        critChance.text = unit.GetCritDamage().ToString();
        critEvade.text = unit.GetCritDefense().ToString();
        speed.text = unit.GetCombatSpeed().ToString();
}

    void MoveIn (Vector3Int tile)
    {
        //if (index <= moveDistance)
        //{
        //    index += interval;
        //    panel.transform.Translate(Vector3.right * interval);
        //}
        Vector3 temp = tile + new Vector3(0.5f, 0.5f, 0);
        temp = Camera.main.WorldToScreenPoint(myGrid.CellToLocal(tile) + new Vector3(0.5f, 0.5f, 0));
        panel.GetComponent<RectTransform>().position = temp;
        panel.GetComponent<Animator>().Play("Fade in");
    }
    void MoveOut()
    {
        //if (index > 0)
        //{
        //    index -= interval;
        //    panel.transform.Translate(Vector3.left * interval);
        //}
        panel.GetComponent<Animator>().Play("Fade out");
        //panel.transform.position = originalPos;
    }
}
