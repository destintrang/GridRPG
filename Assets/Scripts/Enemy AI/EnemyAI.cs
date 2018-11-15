using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {


    public enum EnemyBehaviour
    {
        IDLE,
        HOLD,
        WAIT,
        SEEK
    }

    public EnemyBehaviour behaviour;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public EnemyDecision GetDecision ()
    {
        UnitTracker ut = GetComponent<UnitTracker>();
        
        switch (behaviour)
        {
            case EnemyBehaviour.IDLE:
                //In this state, enemies will simply stand still, and will not attack (will still counterattack)
                return new EnemyDecision();
            case EnemyBehaviour.HOLD:
                //In this state, enemies will stay in place, and will not move. They will attack anything in their immediate range,
                //but will not move to attack a unit
                return FindBestAttackAt(ut.GetLocation());
            case EnemyBehaviour.WAIT:
                //In this state, enemies will stay in place, but will move to attack any enemies they can move to and attack
                return FindBestMoveAttack(ut.PossibleMovement());
            case EnemyBehaviour.SEEK:
                //In this state, enemies will stay in place, but will move to attack any enemies they can move to and attack
                //If there are no enemies to attack, enemies will move towards their goal
                EnemyDecision bestCombat = FindBestMoveAttack(ut.PossibleMovement());
                if (bestCombat.pass)
                {
                    return FindBestMove(new Vector3Int(0,0,0));
                } else
                {
                    return bestCombat;
                }
        }

        print("???");
        return new EnemyDecision();
    }


    EnemyDecision FindBestMove (Vector3Int goal)
    {
        int closestDistance = int.MaxValue;
        Vector3Int bestMove = new Vector3Int();
        Vector3Int currentLocation = GetComponent<UnitTracker>().GetLocation();

        foreach (Vector3Int move in GetComponent<UnitTracker>().PossibleMovement())
        {
            int tempDistance = ShortestPath.instance.FindPath(move, goal, "Red").Count;
            if (tempDistance < closestDistance)
            {
                closestDistance = tempDistance;
                bestMove = move;
            }
        }
        
        return new EnemyDecision(ShortestPath.instance.FindPath(currentLocation, bestMove, "Red"));
    }


    EnemyDecision FindBestMoveAttack (List<Vector3Int> possibleMoves)
    {
        UnitTracker ut = GetComponent<UnitTracker>();
        if (possibleMoves.Count == 0)
        {
            return FindBestAttackAt(ut.GetLocation());
        }

        Vector3Int bestMove = new Vector3Int(-1, -1, -1);
        Vector3Int bestAttack = new Vector3Int(-1, -1, -1);
        float bestRating = 0;
        bool freeAttackFound = false;
        foreach (Vector3Int move in possibleMoves)  //Loop through every possible move a character can make
        {
            foreach (Vector3Int attack in ut.PossibleAttacksAtTile(move))  //Loop through every possible attack at each move
            {
                if (TileManager.instance.allUnits.ContainsKey(attack) && TileManager.instance.allUnits[attack].tag != tag)  //Check to see if there is an attackable unit
                {
                    GameObject target = TileManager.instance.allUnits[attack];
                    BattleSimulation battle = new BattleSimulation(gameObject, target, move, target.GetComponent<UnitTracker>().GetLocation());
                    if (!TileManager.instance.allUnits.ContainsKey(move))
                    {
                        if (!freeAttackFound && battle.GetDefenderAttacks() == 0)
                        {
                            freeAttackFound = true;
                            bestRating = CalculateRating(battle);
                            bestMove = move;
                            bestAttack = attack;
                        }
                        else if (!freeAttackFound && battle.GetDefenderAttacks() != 0)
                        {
                            float tempRating = CalculateRating(battle);
                            if (tempRating > bestRating)
                            {
                                bestRating = tempRating;
                                bestMove = move;
                                bestAttack = attack;
                            }
                        }
                        else if (battle.GetDefenderAttacks() == 0)
                        {
                            float tempRating = CalculateRating(battle);
                            if (tempRating > bestRating)
                            {
                                bestRating = tempRating;
                                bestMove = move;
                                bestAttack = attack;
                            }
                        }
                    }
                }
            }
        }
        
        if (bestMove.z == -1 && bestAttack.z == -1) { return new EnemyDecision(); }
        List<Vector3Int> path = ShortestPath.instance.FindPath(gameObject.GetComponent<UnitTracker>().GetLocation(), bestMove, "Red");
        return new EnemyDecision(path, bestAttack);
    }

    EnemyDecision FindBestAttackAt (Vector3Int move)
    {
        UnitTracker ut = GetComponent<UnitTracker>();
        List<Vector3Int> possibleAttacks = ut.PossibleAttacksAtTile(move);
        Vector3Int bestAttack = new Vector3Int(-1, -1, -1);
        float bestRating = 0;
        bool freeAttackFound = false;

        foreach (Vector3Int attack in possibleAttacks)
        {
            if (TileManager.instance.allUnits.ContainsKey(attack) && TileManager.instance.allUnits[attack].tag != tag)
            {
                GameObject target = TileManager.instance.allUnits[attack];
                BattleSimulation battle = new BattleSimulation(gameObject, target, move, target.GetComponent<UnitTracker>().GetLocation());
                
                if (!freeAttackFound && battle.GetDefenderAttacks() == 0)
                {
                    freeAttackFound = true;
                    bestRating = CalculateRating(battle);
                    bestAttack = attack;
                }
                else if (!freeAttackFound && battle.GetDefenderAttacks() != 0)
                {
                    float tempRating = CalculateRating(battle);
                    if (tempRating > bestRating)
                    {
                        bestRating = tempRating;
                        bestAttack = attack;
                    }
                }
                else if (battle.GetDefenderAttacks() == 0)
                {
                    float tempRating = CalculateRating(battle);
                    if (tempRating > bestRating)
                    {
                        bestRating = tempRating;
                        bestAttack = attack;
                    }
                }
            }
        }

        return new EnemyDecision(bestAttack);
    }

    Vector3Int WhoToAttack (List<Vector3Int> possibleAttacks)
    {
        foreach (Vector3Int attack in possibleAttacks)
        {
            if (TileManager.instance.allUnits.ContainsKey(attack) && TileManager.instance.allUnits[attack].tag != tag)
            {
                return attack; //Right now, we'll just prioritize the very first unit found with this; change later
            }
        }

        return new Vector3Int(-1, -1, -1);
    }

    Vector3Int WhereToAttack (List<Vector3Int> possibleAttackMoves, GameObject target)
    {
        Vector3Int bestMove = new Vector3Int(-1, -1, -1);
        float bestRating = 0;
        bool freeAttackFound = false;

        foreach (Vector3Int move in possibleAttackMoves)
        {
            BattleSimulation battle = new BattleSimulation(gameObject, target, move, target.GetComponent<UnitTracker>().GetLocation());
            if (!TileManager.instance.allUnits.ContainsKey(move))
            {
                if (!freeAttackFound && battle.GetDefenderAttacks() == 0)
                {
                    freeAttackFound = true;
                    bestRating = CalculateRating(battle);
                    bestMove = move;
                }
                else if (!freeAttackFound && battle.GetDefenderAttacks() != 0)
                {
                    float tempRating = CalculateRating(battle);
                    if (tempRating > bestRating)
                    {
                        bestRating = tempRating;
                        bestMove = move;
                    }
                }
                else if (battle.GetDefenderAttacks() == 0)
                {
                    float tempRating = CalculateRating(battle);
                    if (tempRating > bestRating)
                    {
                        bestRating = tempRating;
                        bestMove = move;
                    }
                }
            }
        }

        return bestMove;
    }

    List<Vector3Int> IntersectLists (List<Vector3Int> a, List<Vector3Int> b)
    {
        List<Vector3Int> returnedList = new List<Vector3Int>();
        foreach (Vector3Int item in a)
        {
            if (b.Contains(item))
            {
                returnedList.Add(item);
            }
        }
        return returnedList;
    }

    bool AnyTargetsInRange (List<Vector3Int> possibleAttacks)
    {
        foreach (Vector3Int attack in possibleAttacks)
        {
            if (TileManager.instance.allUnits.ContainsKey(attack) && TileManager.instance.allUnits[attack].tag != tag)
            {
                return true;
            }
        }
        return false;
    }


    float CalculateRating (BattleSimulation battle)
    {
        float damageDealt = (float) battle.GetAttackerDamage();
        float accuracy = (float) battle.GetAttackerAccuracy();
        float attacks = (float)battle.GetAttackerAttacks();
        return (attacks * damageDealt * accuracy * 0.01f);
    }
}
