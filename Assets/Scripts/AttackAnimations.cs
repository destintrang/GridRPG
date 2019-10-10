using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimations : MonoBehaviour {
    

    Vector3 enemyTile;
    Vector3 myTile;
    float strikeTime;
    float returnTime;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float step = 0.0f;
        if (BattleManager.instance != null) { step = BattleManager.instance.speed * Time.deltaTime; }

		if (Time.time < strikeTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemyTile, step);
        } else if (Time.time < returnTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, myTile, step);
        } 
	}


    public void Strike(GameObject enemy)
    {
        enemyTile = enemy.GetComponent<UnitTracker>().GetLocation() + new Vector3(0.5f, 0.5f, 0);
        myTile = GetComponent<UnitTracker>().GetLocation() + new Vector3(0.5f, 0.5f, 0);
        enemyTile = ((enemyTile - myTile) / 2) + myTile;
        strikeTime = Time.time + BattleManager.instance.strikeDuration;
        returnTime = strikeTime + BattleManager.instance.strikeDuration;
    }
}
