using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRepositionManager : BaseMenu
{

    public int deployedMax;
    private int deployedCount = 0;
    public int GetDeployedCount() { return deployedCount; }
    public int GetDeployedMax() { return deployedMax; }

    private List<GameObject> deployedUnits = new List<GameObject>();


    GridLayout myGrid;

    StartTile currentTile = null;
    private List<StartTile> startingTiles;
    private List<StartTile> emptyStartingTiles;

    Dictionary<Vector3, StartTile> startTileDict;


    //Singleton
    public static BaseRepositionManager instance;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        //Later lets simply set deployableUnits as the number of starting tiles
        startingTiles = new List<StartTile>(FindObjectsOfType<StartTile>());
        emptyStartingTiles = new List<StartTile>(FindObjectsOfType<StartTile>());

        myGrid = GameObject.Find("Grid").GetComponent<GridLayout>();
        InitializeDict();
        DeployStartingUnits();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clicked = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clicked.z = 0;
            Vector3Int coord = myGrid.LocalToCell(clicked);

            //WHEN THE PLAYER CLICKS ON A STARTING TILE
            if (startTileDict.ContainsKey(coord))
            {
                if (currentTile)
                {
                    //SWITCH
                    currentTile.SwitchUnit(startTileDict[coord]);
                    currentTile = null;
                }
                else
                {
                    currentTile = startTileDict[coord];
                }
            }
        }
    }


    public override void Escape()
    {
        currentTile = null;
        BaseManager.instance.CancelButton();
    }


    public void DeployUnit (GameObject unit)
    {
        unit.GetComponent<UnitTracker>().Deploy();

        emptyStartingTiles[0].SetUnit(unit);
        emptyStartingTiles.RemoveAt(0);

        deployedUnits.Add(unit);

        deployedCount++;
    }
    public void UndeployUnit (GameObject unit)
    {
        Vector3 location = unit.transform.position;

        startTileDict[location].ClearUnit();
        emptyStartingTiles.Add(startTileDict[location]);

        deployedUnits.Remove(unit);
        deployedCount--;

        unit.transform.position = new Vector3(-1, -1, 0);
    }


    public bool CanDeploy ()
    {
        //There's enough space to add a unit!
        if (deployedCount < deployedMax) { return true; }

        //Otherwise there's no space 
        return false;
    }
    public bool CanUndeploy ()
    {
        //Need to have at least one unit deployed
        if (deployedUnits.Count > 1) { return true; }

        return false;
    }


    public bool IsDeployed (GameObject unit)
    {

        if (deployedUnits.Contains(unit))
        {
            return true;
        }

        return false;

    }


    void DeployStartingUnits ()
    {
        List<GameObject> party = PartyManager.instance.GetParty();
        List<GameObject> deployed = new List<GameObject>();

        for (int i = 0; i < deployedMax; i++)
        {
            if (party[i] != null)
            {
                deployed.Add(party[i]);
                DeployUnit(party[i]);
                party[i].GetComponent<UnitTracker>().Deploy();
            }
            else
            {
                break;
            }
        }

        //Once we've gotten the units to deploy, add them to the startTiles
        //LoadStartTiles(deployed);
    }


    void LoadStartTiles (List<GameObject> deployed)
    {
        int counter = 0;

        //Get the list of units from the party for now, later get from deployed!!
        //List<GameObject> party = PartyManager.instance.GetParty();

        foreach (StartTile tile in startingTiles)
        {
            if (counter < deployed.Count)
            {
                GameObject unit = deployed[counter];
                tile.SetUnit(unit);
                deployedUnits.Add(unit);
                counter++;
            }
            else
            {
                break;
            }
        }
    }


    void InitializeDict ()
    {
        startTileDict = new Dictionary<Vector3, StartTile>();

        foreach (StartTile tile in startingTiles)
        {
            Vector3 tilePos = tile.transform.position;
            Debug.Log(tilePos);
            startTileDict.Add(tilePos, tile);
        }
    }
}
