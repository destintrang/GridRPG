using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{

    public GameObject wolf;
    public GameObject rabbit;
    public GameObject bear;


    //Singleton
    public static CharacterGenerator instance;
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


    public GameObject CreateCharacter(int startingRank = 1)
    {
        GameObject character = Instantiate(wolf);
        CharacterStats c = character.GetComponent<CharacterStats>();

        c.SetName(RandomNameGenerator.GetRandomName());

        for (int i = 1; i < startingRank; i++)
        {
            c.RankUp();
        }

        return character;
    }
    public GameObject CreateCharacter2(int startingRank = 1)
    {
        GameObject character = Instantiate(rabbit);
        CharacterStats c = character.GetComponent<CharacterStats>();

        c.SetName(RandomNameGenerator.GetRandomName());

        for (int i = 1; i < startingRank; i++)
        {
            character.GetComponent<CharacterStats>().RankUp();
        }

        return character;
    }
    public GameObject CreateCharacter3(int startingRank = 1)
    {
        GameObject character = Instantiate(bear);
        CharacterStats c = character.GetComponent<CharacterStats>();

        c.SetName(RandomNameGenerator.GetRandomName());

        for (int i = 1; i < startingRank; i++)
        {
            character.GetComponent<CharacterStats>().RankUp();
        }

        return character;
    }
}
