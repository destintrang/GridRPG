using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExpandItem : ExpandInformation, IPointerEnterHandler, IPointerExitHandler
{


    int itemType;  //0: Weapon, 1: Armor, 2: Other
    
    public Text weaponClass;
    public Text damageType;
    public Text weaponMight;
    public Text weaponAccuracy;
    public Text weaponCrit;
    public Text weaponWeight;
    public Text weaponRanges;

    public Text armorDefense;
    public Text weightClass;
    public Text magicDefense;
    public Text cutDefense;
    public Text stabDefense;
    public Text bashDefense;
    public Text armorWeight;
    public Text armorEvasion;


    //Detect if the Cursor starts to pass over the GameObject
    public override void OnPointerEnter(PointerEventData pointerEventData)
    {
        infoField.text = info;
        infoObject.SetActive(true);
        infoField.gameObject.SetActive(true);
        switch (itemType)
        {
            case 0:
                weaponClass.gameObject.SetActive(true);
                damageType.gameObject.SetActive(true);
                weaponMight.gameObject.SetActive(true);
                weaponAccuracy.gameObject.SetActive(true);
                weaponCrit.gameObject.SetActive(true);
                weaponWeight.gameObject.SetActive(true);
                weaponRanges.gameObject.SetActive(true);
                break;
            case 1:
                armorDefense.gameObject.SetActive(true);
                weightClass.gameObject.SetActive(true);
                magicDefense.gameObject.SetActive(true);
                cutDefense.gameObject.SetActive(true);
                stabDefense.gameObject.SetActive(true);
                bashDefense.gameObject.SetActive(true);  
                armorWeight.gameObject.SetActive(true);
                armorEvasion.gameObject.SetActive(true);
                break;
        }
    }

    //Detect when Cursor leaves the GameObject
    public override void OnPointerExit(PointerEventData pointerEventData)
    {
        infoField.gameObject.SetActive(false);
        infoObject.SetActive(false);
        infoField.text = "";
    }


    public void InitializeItem (Item i)
    {
        if (i is Weapon)
        {
            Weapon temp = i as Weapon;

            weaponClass.text = temp.weaponClass.ToString();
            damageType.text = temp.damageType.ToString();
            weaponMight.text = temp.might.ToString();
            weaponAccuracy.text = temp.accuracy.ToString();
            weaponCrit.text = temp.crit.ToString();
            weaponWeight.text = temp.weight.ToString();
            info = temp.description;

            itemType = 0;
        } else if (i is Armor)
        {
            Armor temp = i as Armor;

            armorDefense.text = temp.defense.ToString();
            weightClass.text = temp.weightClass.ToString();
            magicDefense.text = temp.magicDefense.ToString();
            cutDefense.text = temp.cutBonus.ToString();
            stabDefense.text = temp.stabBonus.ToString();
            bashDefense.text = temp.bashBonus.ToString();
            armorWeight.text = temp.weight.ToString();
            armorEvasion.text = temp.evasion.ToString();
            info = temp.description;

            itemType = 1;
        } else
        {
            info = i.description;
            itemType = 2;
        }
    }

}
