using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Inventory/Armor")]
public class Armor : Item {

    public enum WeightType
    {
        LIGHT,
        HEAVY
    }


    public WeightType weightClass;

    public int defense;
    public int magicDefense;

    public int cutBonus;
    public int stabBonus;
    public int bashBonus;

    public int weight;
    public int evasion;

}
