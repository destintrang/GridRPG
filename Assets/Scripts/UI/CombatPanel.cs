using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatPanel : MonoBehaviour {

    float index;
    public float moveDistance;
    public float interval;
    public bool right;
    public bool activated = false;

    public Text panelName;
    public Text health;
    public Text weapon;
    public Text armor;
    public Text damage;
    public Text accuracy;
    public Text critChance;

    public List<GameObject> emptyBars;
    public List<GameObject> fullBars;

    int emptyCounter = 0;
    int fullCounter = 0;


    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		if (activated)
        {
            MoveIn();
        }
        else
        {
            MoveOut();
        }
	}


    public void InitializeInfo (string _name, int _maxHealth, int _health, Weapon _weapon, Armor _armor, string _damage, string _accuracy, string _critChance, int _attacks)
    {
        UpdateInfo(_name, panelName);
        UpdateInfo(_health.ToString(), health);
        if (_weapon != null) { UpdateInfo(_weapon.name, weapon); }
        else { UpdateInfo("None", weapon); }
        if (_armor != null) { UpdateInfo(_armor.name, armor); }
        else { UpdateInfo("None", armor); }
        UpdateInfo(_damage, damage);
        UpdateInfo(_accuracy, accuracy);
        UpdateInfo(_critChance, critChance);

        SetEmptyBar(_maxHealth);
        SetFullBar(_health);

        activated = true;
    }

    void SetEmptyBar (int setInt)
    {
        int x = emptyCounter;
        while (x != setInt)
        {
            if (x < setInt)
            {
                emptyBars[x].SetActive(true);
                emptyCounter++;
                x++;
            }
            else
            {
                emptyBars[x].SetActive(false);
                emptyCounter--;
                x--;
            }
        }
    }

    void SetFullBar(int setInt)
    {
        int x = fullCounter;
        while (x != setInt)
        {
            if (x < setInt)
            {
                fullBars[x].SetActive(true);
                fullCounter++;
                x++;
            }
            else
            {
                fullBars[x - 1].SetActive(false);
                fullCounter--;
                x--;
            }
        }
    }

    void UpdateInfo(string newInfo, Text field)
    {
        field.text = newInfo;
        return;
    }

    public void DeductHealth (int deduction, int maxHealth)
    {
        int hp = int.Parse(health.text) - deduction;
        hp = Mathf.Clamp(hp, 0, maxHealth);
        health.text = hp.ToString();
        SetFullBar(hp);
    }


    void MoveIn()
    {
        if (index <= moveDistance)
        {
            index += interval;
            if (right)
            {
                transform.Translate(Vector3.right * interval);
            } else
            {
                transform.Translate(Vector3.left * interval);
            }
        }
    }
    void MoveOut()
    {
        if (index > 0)
        {
            index -= interval;
            if (right)
            {
                transform.Translate(Vector3.left * interval);
            }
            else
            {
                transform.Translate(Vector3.right * interval);
            }
        }
    }
}
