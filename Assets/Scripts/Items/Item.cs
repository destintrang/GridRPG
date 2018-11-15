using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

    new public string name = "New Item";
    public bool usable;
    public string description;

    public virtual void OnUse (GameObject user)
    {
        Debug.Log(user.name + " used " + name);
        return;
    }

}
