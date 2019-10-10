using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinCondition : MonoBehaviour
{


    public Text objective;


    //Singleton
    public static WinCondition instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateObjective();
    }


    public virtual void CheckVictory()
    {
        return;
    }


    protected void OnVictory ()
    {
        StartCoroutine(LevelManager.instance.EndLevel());
    }

    //Update the UI to correctly display this level's objective
    public virtual void UpdateObjective ()
    {

    }
}
