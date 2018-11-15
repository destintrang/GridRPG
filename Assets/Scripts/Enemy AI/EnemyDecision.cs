using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDecision {

    public bool pass;

    List<Vector3Int> movePath;
    Vector3Int attackTarget;


    //This is an empty enemy decision. This basically means the enemy won't do anything this turn.
    public EnemyDecision()
    {
        pass = true;
        movePath = new List<Vector3Int>();
        attackTarget = new Vector3Int(-1, -1, -1);
    }

    //This decision is made when the enemy chooses to move without attacking.
    public EnemyDecision(List<Vector3Int> _movePath)
    {
        pass = false;
        movePath = _movePath;
        attackTarget = new Vector3Int(-1, -1, -1); 
    }

    //This decision is made when the enemy chooses to attack without moving.
    public EnemyDecision(Vector3Int _attackTarget)
    {
        pass = false;
        movePath = new List<Vector3Int>();
        attackTarget = _attackTarget;
    }

    //This decision is made when the enemy choose to move AND attack.
    public EnemyDecision (List<Vector3Int> _movePath, Vector3Int _attackTarget)
    {
        pass = false;
        movePath = _movePath;
        attackTarget = _attackTarget;
    }


    public List<Vector3Int> GetPath ()
    {
        return movePath;
    }
    public Vector3Int GetAttackTarget ()
    {
        return attackTarget;
    }
}
