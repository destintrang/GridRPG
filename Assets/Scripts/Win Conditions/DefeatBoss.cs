using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatBoss : WinCondition
{


    public List<GameObject> targetEnemies;


    public override void CheckVictory()
    {
        foreach (GameObject enemy in targetEnemies)
        {
            if (enemy.activeInHierarchy == true) { return; }
        }

        OnVictory();
    }

    public override void UpdateObjective()
    {
        objective.text = "Defeat boss!";
    }
}
