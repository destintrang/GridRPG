using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{


    //Singleton
    public static PartyManager instance;
    private void Awake()
    {
        instance = this;
        //AddToParty(CharacterGenerator.instance.CreateCharacter(3));
        //AddToParty(CharacterGenerator.instance.CreateCharacter2(3));
        //AddToParty(CharacterGenerator.instance.CreateCharacter3(3));
    }


    public List<GameObject> party;
    public List<GameObject> GetParty () { return party; }

    private const int MAXPARTYSIZE = 10;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddToParty (GameObject character)
    {
        character.transform.SetParent(this.transform);
        party.Add(character);
    }

    //Return the units that are being deployed this battle
    public List<GameObject> GetDeployedUnits ()
    {
        List<GameObject> deployed = new List<GameObject>();

        foreach (GameObject unit in party)
        {
            if (unit.GetComponent<UnitTracker>().isDeployed())
            {
                deployed.Add(unit);
            }
        }

        return deployed;
    }

}
