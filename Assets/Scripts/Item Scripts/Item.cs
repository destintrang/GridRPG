using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour {

    new public string name = "New Item";
    public bool usable;
    public string description;

    public virtual void OnUse(GameObject user, bool fromConvoy = false)
    {
        return;
    }

    public virtual bool CanUse (GameObject user)
    {
        return true;
    }

    public virtual string GetName ()
    {
        return name;
    }

}
