using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {


    BattleSimulation battle;

    public int doubleAttackThreshold = 4;
    public bool attacking;

	public GameObject PLAYERINFO;
	public GameObject ENEMYINFO;

    //Singleton
    public static BattleManager instance;
    private void Awake()
    {
        instance = this;
    }

    //Variables for attack animations, not used here, but all characters should have the same timings
    public float strikeDuration;
    public float speed;

    public ParticleSystem slashVFX;
    public GameObject missVFX;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void DisplayBattleForecast (GameObject attacker, GameObject defender, Vector3Int playerPos, Vector3Int enemyPos)
    {
        if (attacker.tag == "Red") { FaceEachOther(attacker, defender); }
        ToggleCombatDetails(attacker, defender, playerPos, enemyPos);

        //PLAYERINFO.SetActive (true);
        //ENEMYINFO.SetActive (true);
    }


	public void HideBattleForecast () 
	{
        PLAYERINFO.GetComponent<CombatPanel>().activated = false;
		ENEMYINFO.GetComponent<CombatPanel>().activated = false;
    }


	public void PlayerAttacks (GameObject player, GameObject enemy, int range)
    {
        FaceEachOther(player, enemy);
        StartCoroutine (Combat(player, enemy, PLAYERINFO, ENEMYINFO, range));
	}

	public void EnemyAttacks (GameObject enemy, GameObject player, int range)
	{
		StartCoroutine (Combat(enemy, player, ENEMYINFO, PLAYERINFO, range));
	}


	IEnumerator Combat (GameObject attacker, GameObject defender, GameObject aPanel, GameObject dPanel, int range)
	{
        attacking = true;
        int aDamage = battle.GetAttackerDamage();
        int aAccuracy = battle.GetAttackerAccuracy();
        int aCrit = battle.GetAttackerCrit();
        int aAttacks = battle.GetAttackerAttacks();
        int dAttacks = battle.GetDefenderAttacks();
        int dDamage = battle.GetDefenderDamage();
        int dAccuracy = battle.GetDefenderAccuracy();
        int dCrit = battle.GetDefenderCrit();

        while (attacking)
        {
            //Initiator attacks
            if (aAttacks > 0)
            {
                aAttacks--;
                attacker.GetComponent<AttackAnimations>().Strike(defender);
                if (Random.Range(0, 100) < aAccuracy)
                {
                    if (Random.Range(0, 100) < aCrit)
                    {
                        //Critical hit!
                        print("CRIT! " + attacker.name + " dealt " + (aDamage * 3) + " damage to " + defender.name + "!");
                        DamageVFX(attacker, defender);
                        SoundEffectsManager.instance.PlayStrike();
                        defender.GetComponent<CharacterStats>().TakeDamage(aDamage * 3);
                        dPanel.GetComponent<CombatPanel>().DeductHealth(aDamage * 3, defender.GetComponent<CharacterStats>().GetMaxHealth());
                    }
                    else
                    {
                        //Regular hit
                        print("Hit! " + attacker.name + " dealt " + aDamage + " damage to " + defender.name + "!");
                        DamageVFX(attacker, defender);
                        SoundEffectsManager.instance.PlayStrike();
                        defender.GetComponent<CharacterStats>().TakeDamage(aDamage);
                        dPanel.GetComponent<CombatPanel>().DeductHealth(aDamage, defender.GetComponent<CharacterStats>().GetMaxHealth());
                    }
                }
                else
                {
                    //Miss...
                    StartCoroutine(MissVFX(defender));
                    SoundEffectsManager.instance.PlayMiss();
                    print(attacker.name + " missed!");
                }

                yield return new WaitForSeconds(1);

                //Check if the defender died to the attacker's strike
                if (defender.GetComponent<CharacterStats>().GetHealth() == 0)
                {
                    defender.GetComponent<UnitTracker>().Die();
                    break;
                }
            }


            //Defender attacks, if they can
            if (dAttacks > 0)
            {
                dAttacks--;
                defender.GetComponent<AttackAnimations>().Strike(attacker);
                if (Random.Range(0, 100) < dAccuracy)
                {
                    if (Random.Range(0, 100) < dCrit)
                    {
                        //Critical hit!
                        print("CRIT! " + defender.name + " dealt " + (dDamage * 3) + " damage to " + attacker.name + "!");
                        DamageVFX(defender, attacker);
                        SoundEffectsManager.instance.PlayStrike();
                        attacker.GetComponent<CharacterStats>().TakeDamage(dDamage * 3);
                        aPanel.GetComponent<CombatPanel>().DeductHealth(dDamage * 3, defender.GetComponent<CharacterStats>().GetMaxHealth());
                    }
                    else
                    {
                        //Regular hit
                        print("Hit! " + defender.name + " dealt " + dDamage + " damage to " + attacker.name + "!");
                        DamageVFX(defender, attacker);
                        SoundEffectsManager.instance.PlayStrike();
                        attacker.GetComponent<CharacterStats>().TakeDamage(dDamage);
                        aPanel.GetComponent<CombatPanel>().DeductHealth(dDamage, defender.GetComponent<CharacterStats>().GetMaxHealth());
                    }
                }
                else
                {
                    //Miss...
                    StartCoroutine(MissVFX(attacker));
                    SoundEffectsManager.instance.PlayMiss();
                    print(defender.name + " missed!");
                }

                yield return new WaitForSeconds(1);

                //Check if the attacker died to the defender's strike
                if (attacker.GetComponent<CharacterStats>().GetHealth() == 0)
                {
                    attacker.GetComponent<UnitTracker>().Die();
                    break;
                }
            }


            if (aAttacks == 0 && dAttacks == 0) { break; }
        }

        //What should happen after combat completes
        //For now, just disable the combat ui and finish the turn
        RevertToIdle(attacker, defender);
        attacking = false;
        HideBattleForecast();
        MouseControl.instance.currentState = MouseControl.MouseState.FINISHED;
    }


    void ToggleCombatDetails (GameObject attacker, GameObject defender, Vector3Int playerPos, Vector3Int enemyPos)
    {
        battle = new BattleSimulation(attacker, defender, playerPos, enemyPos);

        CharacterStats a = attacker.GetComponent<CharacterStats>();
        CharacterStats d = defender.GetComponent<CharacterStats>();
        
        if (attacker.tag == "Blue" || attacker.tag == "Green")
        {
            PLAYERINFO.GetComponent<CombatPanel>().InitializeInfo(attacker.name, a.GetMaxHealth(), a.GetHealth(),
                                                                  a.GetWeapon(), a.GetArmor(), battle.GetAttackerDamage().ToString(),
                                                                  battle.GetAttackerAccuracy().ToString(), battle.GetAttackerCrit().ToString(), battle.GetAttackerAttacks());
            ENEMYINFO.GetComponent<CombatPanel>().InitializeInfo(defender.name, d.GetMaxHealth(), d.GetHealth(),
                                                                     d.GetWeapon(), d.GetArmor(), battle.GetDefenderDamage().ToString(),
                                                                     battle.GetDefenderAccuracy().ToString(), battle.GetDefenderCrit().ToString(), battle.GetDefenderAttacks());
        } else
        {
            ENEMYINFO.GetComponent<CombatPanel>().InitializeInfo(attacker.name, a.GetMaxHealth(), a.GetHealth(),
                                                                  a.GetWeapon(), a.GetArmor(), battle.GetAttackerDamage().ToString(),
                                                                  battle.GetAttackerAccuracy().ToString(), battle.GetAttackerCrit().ToString(), battle.GetAttackerAttacks());
            PLAYERINFO.GetComponent<CombatPanel>().InitializeInfo(defender.name, d.GetMaxHealth(), d.GetHealth(),
                                                                     d.GetWeapon(), d.GetArmor(), battle.GetDefenderDamage().ToString(),
                                                                     battle.GetDefenderAccuracy().ToString(), battle.GetDefenderCrit().ToString(), battle.GetDefenderAttacks());
        }
    }


    void DamageVFX (GameObject a, GameObject d)
    {
        slashVFX.transform.rotation = Quaternion.Euler(0, 90, -90);
        slashVFX.transform.position = new Vector3(d.transform.position.x, d.transform.position.y, -1);
        Vector3 direction = d.transform.position - a.transform.position;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
        slashVFX.transform.Rotate(0, -angle, 0);
        slashVFX.Play();
    }


    IEnumerator MissVFX (GameObject d)
    {
        missVFX.transform.position = d.transform.position;
        missVFX.SetActive(true);
        missVFX.GetComponent<Animator>().Play("MissAnimation");
        yield return new WaitForSeconds(0.5f);
        missVFX.SetActive(false);
    }


    void FaceEachOther (GameObject a, GameObject b)
    {
        Vector3Int difference = a.GetComponent<UnitTracker>().GetLocation() - b.GetComponent<UnitTracker>().GetLocation(); 
        if (Mathf.Abs(difference.x) >= Mathf.Abs(difference.y))
        {
            if (difference.x > 0)
            {
                a.GetComponent<Animator>().SetTrigger("Left");
                b.GetComponent<Animator>().SetTrigger("Right");
            } else if (difference.x < 0)
            {
                a.GetComponent<Animator>().SetTrigger("Right");
                b.GetComponent<Animator>().SetTrigger("Left");
            }
        } else
        {
            if (difference.y > 0)
            {
                a.GetComponent<Animator>().SetTrigger("Down");
                b.GetComponent<Animator>().SetTrigger("Up");
            } else if (difference.y < 0)
            {
                a.GetComponent<Animator>().SetTrigger("Up");
                b.GetComponent<Animator>().SetTrigger("Down");
            }
        }
    }

    void RevertToIdle (GameObject a, GameObject b)
    {
        if (a != null && a.GetComponent<Animator>() != null)
        {
            a.GetComponent<Animator>().SetBool("Selected", false);
            a.GetComponent<Animator>().SetTrigger("Idle");
        }
        if (b != null && b.GetComponent<Animator>() != null)
        {
            b.GetComponent<Animator>().SetBool("Selected", false);
            b.GetComponent<Animator>().SetTrigger("Idle");
        }
    }
}
