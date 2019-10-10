using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{


    public Button hire;
    public Button reroll;
    public Button start;

    List<GameObject> currentUnits = new List<GameObject>();

    public List<Image> unitImages;
    public List<Text> unitNames;


    //Singleton
    public static StartManager instance;
    private void Awake()
    {
        instance = this;
    }


    public void HireUnits ()
    {
        ClearCurrentUnits();

        //Create three random units
        currentUnits.Add(CharacterGenerator.instance.CreateRandomUnit());
        currentUnits.Add(CharacterGenerator.instance.CreateRandomUnit());
        currentUnits.Add(CharacterGenerator.instance.CreateRandomUnit());

        UpdateUI();
    }


    public void OnStartButton ()
    {
        foreach(GameObject unit in currentUnits)
        {
            PartyManager.instance.AddToParty(unit);
        }

        StartCoroutine(LevelManager.instance.EndLevel());
    }


    private void ClearCurrentUnits ()
    {

        foreach (GameObject unit in currentUnits)
        {
            Destroy(unit);
        }

        currentUnits.Clear();

    }

    private void UpdateUI ()
    {

        int counter = 0;
        foreach (GameObject unit in currentUnits)
        {
            unitImages[counter].transform.parent.gameObject.SetActive(true);
            unitImages[counter].sprite = unit.GetComponent<SpriteRenderer>().sprite;
            unitNames[counter].text = unit.GetComponent<CharacterStats>().GetName();

            counter++;
        }

    }

    
    //Change the activeness of the buttons
    public void OnHireButton ()
    {
        hire.gameObject.SetActive(false);
        reroll.gameObject.SetActive(true);
        start.gameObject.SetActive(true);
    }

}
