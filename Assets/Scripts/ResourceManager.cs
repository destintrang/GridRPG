 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{


    private int gold = 100;
    public int GetGold() { return gold; }


    //Singleton
    public static ResourceManager instance;
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


    public bool SpendGold (int cost)
    {
        if (cost <= gold)
        {
            gold -= cost;
            return true;
        }
        else if (cost > gold)
        {
            //Can't afford
            return false;
        }

        return false;
    }

    public void AddGold (int g)
    {
        gold += g;
    }

}
