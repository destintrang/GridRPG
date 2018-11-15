using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {


    public float startDelay;

    //Singleton
    public static EnemyManager instance;
    private void Awake()
    {
        instance = this;
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartEnemyTurn ()
    {
        StartCoroutine(EnemyTurn());
    }


    IEnumerator EnemyTurn ()
    {
        yield return new WaitForSeconds(startDelay);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Red");
        for (int i = 0; i < enemies.Length; i++)
        {
            EnemyDecision decision;
            decision = enemies[i].GetComponent<EnemyAI>().GetDecision();
            if (!decision.pass)
            {
                Movement.instance.BeginMoving(enemies[i], decision.GetPath());

                while (Movement.instance.moving)
                {
                    yield return null;
                }
                if (decision.GetPath().Count != 0)
                {
                    enemies[i].GetComponent<UnitTracker>().MoveTo(decision.GetPath()[(decision.GetPath().Count - 1)]);
                }
                
                Vector3Int attackCoord = decision.GetAttackTarget();
                if (attackCoord.z != -1)
                {
                    GameObject attackTarget = TileManager.instance.allUnits[attackCoord];
                    //Get the range of the combat
                    int range = (int)(Mathf.Abs(enemies[i].GetComponent<UnitTracker>().GetLocation().x - attackCoord.x) + Mathf.Abs(enemies[i].GetComponent<UnitTracker>().GetLocation().y - attackCoord.y));

                    BattleManager.instance.DisplayBattleForecast(enemies[i], attackTarget, enemies[i].GetComponent<UnitTracker>().GetLocation(), attackTarget.GetComponent<UnitTracker>().GetLocation());
                    yield return new WaitForSeconds(1);
                    BattleManager.instance.EnemyAttacks(enemies[i], attackTarget, range);
                    while (BattleManager.instance.attacking)
                    {
                        yield return null;
                    }
                    BattleManager.instance.HideBattleForecast();
                } else
                {
                    enemies[i].GetComponent<Animator>().SetTrigger("Idle");
                }
            }

            TurnManager.instance.FinishUnit();
        }
    }
}
