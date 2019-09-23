using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour {


    public bool debug;

    public Color blue;
    public Color red;

    public float textDistance;
    public float shadeDistance;
    public float shadeOpacity;
    public float sequenceSpeed;
    public GameObject endTurnText;
    public GameObject topShade;
    public GameObject botShade;

    private List<GameObject> playerUnits;
    public void SetPlayerUnits (List<GameObject> units) { playerUnits = units; }
    private List<GameObject> movedUnits = new List<GameObject>();
    public List<GameObject> GetMovedUnits () { return movedUnits; }

    int finishedUnits;
    int totalBlueArmy;
    int totalRedArmy;

    public enum TurnState { BLUE, RED };
    public TurnState currentState;

    Vector3 textOrigin;
    Vector3 topShadeOrigin;
    Vector3 botShadeOrigin;

    //Singleton
    public static TurnManager instance;
    private void Awake()
    {
        instance = this;
    }


    // Use this for initialization
    void Start ()
    {
        totalBlueArmy = GameObject.FindGameObjectsWithTag("Blue").Length;
        finishedUnits = 0;
        currentState = TurnState.BLUE;

        textOrigin = endTurnText.transform.position;
        topShadeOrigin = topShade.transform.position;
        botShadeOrigin = botShade.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void FinishPlayerUnit (GameObject unit)
    {
        movedUnits.Add(unit);

        if (movedUnits.Count == playerUnits.Count)
        {
            StartCoroutine(EndOfTurnSequence());
        }
    }

    public void FinishUnit ()
    {
        if (!debug)
        {
            finishedUnits++;
            switch (currentState)
            {
                case TurnState.BLUE:
                    if (finishedUnits == totalBlueArmy)
                    {
                        StartCoroutine(EndOfTurnSequence());
                    }
                    break;
                case TurnState.RED:
                    if (finishedUnits == totalRedArmy)
                    {
                        StartCoroutine(EndOfTurnSequence());
                    }
                    break;
            }
        }
    }


    void StartNextTurn ()
    {
        finishedUnits = 0;

        switch (currentState)
        {
            case TurnState.BLUE:
                totalRedArmy = GameObject.FindGameObjectsWithTag("Red").Length;
                currentState = TurnState.RED;
                EnemyManager.instance.StartEnemyTurn();
                MouseControl.instance.currentState = MouseControl.MouseState.DISABLED;
                if (totalRedArmy == 0) { StartCoroutine(EndOfTurnSequence()); }
                break;
            case TurnState.RED:
                StartPlayerTurn();
                break;
        }
    }


    private void StartPlayerTurn ()
    {
        //Resync animations, just in case
        MasterAnimator.instance.ResetIdleAnimations();

        //Make all units movable again
        movedUnits.Clear();

        totalRedArmy = GameObject.FindGameObjectsWithTag("Blue").Length;
        currentState = TurnState.BLUE;
        MouseControl.instance.currentState = MouseControl.MouseState.NORMAL;
    }

    IEnumerator EndOfTurnSequence ()
    {
        MouseControl.instance.currentState = MouseControl.MouseState.DISABLED;
        yield return new WaitForSeconds(0.25f);

        Text txt = endTurnText.GetComponent<Text>();
        Vector3 right = textOrigin + new Vector3(textDistance, 0, 0);
        Vector3 left = textOrigin - new Vector3(textDistance, 0 , 0);

        switch (currentState)
        {
            case TurnState.BLUE:
                txt.text = "Enemy Phase";
                txt.color = red;
                break;
            case TurnState.RED:
                txt.text = "Player Phase";
                txt.color = blue;
                break;
        }

        txt.transform.position = right;
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
        endTurnText.SetActive(true);
        StartCoroutine(ShadeIn());

        SoundEffectsManager.instance.PlayEndTurn();

        while (txt.color.a < 1.0f)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a + (Time.deltaTime * sequenceSpeed));
            txt.transform.position = Vector3.MoveTowards(txt.transform.position, left, (right.x - left.x) * 0.5f * Time.deltaTime * sequenceSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(0.75f);
        StartCoroutine(ShadeOut());

        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1);
        while (txt.color.a > 0)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a - (Time.deltaTime * sequenceSpeed));
            txt.transform.position = Vector3.MoveTowards(txt.transform.position, left, (right.x - left.x) * 0.5f * Time.deltaTime * sequenceSpeed);
            yield return null;
        }

        //yield return new WaitForSeconds(2);

        endTurnText.SetActive(false);
        StartNextTurn();
    }

    IEnumerator ShadeIn ()
    {
        topShade.transform.position = topShadeOrigin;
        botShade.transform.position = botShadeOrigin;
        Vector3 topGoal = topShade.transform.position;
        Vector3 botGoal = botShade.transform.position;

        topShade.transform.position += new Vector3(0, shadeDistance, 0);
        botShade.transform.position -= new Vector3(0, shadeDistance, 0);

        topShade.SetActive(true);
        botShade.SetActive(true);

        RawImage t = topShade.GetComponent<RawImage>();
        t.color = new Color(t.color.r, t.color.g, t.color.b, 0);
        RawImage b = botShade.GetComponent<RawImage>();
        b.color = new Color(b.color.r, b.color.g, b.color.b, 0);

        //while (topShade.transform.position.y > topGoal.y && botShade.transform.position.y < botGoal.y)
        while (t.color.a < (shadeOpacity / 255.0f) && b.color.a < (shadeOpacity / 255.0f))
        {
            topShade.transform.position = Vector3.MoveTowards(topShade.transform.position, topGoal, Time.deltaTime * sequenceSpeed);
            t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a + (Time.deltaTime * sequenceSpeed) * (shadeOpacity / 255.0f));

            botShade.transform.position = Vector3.MoveTowards(botShade.transform.position, botGoal, Time.deltaTime * sequenceSpeed);
            b.color = new Color(b.color.r, b.color.g, b.color.b, b.color.a + (Time.deltaTime * sequenceSpeed) * (shadeOpacity / 255.0f));

            yield return null;
        }
    }

    IEnumerator ShadeOut()
    {
        topShade.transform.position = topShadeOrigin;
        botShade.transform.position = botShadeOrigin;
        Vector3 topGoal = topShade.transform.position += new Vector3(0, shadeDistance, 0);
        Vector3 botGoal = botShade.transform.position -= new Vector3(0, shadeDistance, 0);

        RawImage t = topShade.GetComponent<RawImage>();
        t.color = new Color(t.color.r, t.color.g, t.color.b, (shadeOpacity / 255.0f));
        RawImage b = botShade.GetComponent<RawImage>();
        b.color = new Color(b.color.r, b.color.g, b.color.b, (shadeOpacity / 255.0f));

        //while (topShade.transform.position.y > topGoal.y && botShade.transform.position.y < botGoal.y)
        while (t.color.a > 0 && b.color.a > 0)
        {
            topShade.transform.position = Vector3.MoveTowards(topShade.transform.position, topGoal, Time.deltaTime * sequenceSpeed);
            t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a - (Time.deltaTime * sequenceSpeed) * (shadeOpacity / 255.0f));

            botShade.transform.position = Vector3.MoveTowards(botShade.transform.position, botGoal, Time.deltaTime * sequenceSpeed);
            b.color = new Color(b.color.r, b.color.g, b.color.b, b.color.a - (Time.deltaTime * sequenceSpeed) * (shadeOpacity / 255.0f));

            yield return null;
        }

        topShade.SetActive(false);
        botShade.SetActive(false);
    }
}
