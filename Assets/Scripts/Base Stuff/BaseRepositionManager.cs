using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRepositionManager : BaseMenu
{

    public int deployableUnits;
    private int deployedCount = 0;

    GridLayout myGrid;

    StartTile currentTile = null;
    private List<StartTile> startingTiles;

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
        startingTiles = new List<StartTile>( FindObjectsOfType<StartTile>() );
        myGrid = GameObject.Find("Grid").GetComponent<GridLayout>();
        InitializeDict();
        DeployUnits();
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



    public bool CanDeploy ()
    {
        //There's enough space to add a unit!
        if (deployedCount < deployableUnits) { return true; }

        //Otherwise there's no space 
        return false;
    }


    void DeployUnits ()
    {
        List<GameObject> party = PartyManager.instance.GetParty();
        List<GameObject> deployed = new List<GameObject>();

        for (int i = 0; i < deployableUnits; i++)
        {
            if (party[i] != null)
            {
                deployed.Add(party[i]);
                party[i].GetComponent<UnitTracker>().Deploy();
                deployedCount++;
            }
            else
            {
                break;
            }
        }

        //Once we've gotten the units to deploy, add them to the startTiles
        LoadStartTiles(deployed);
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
            Vector3 tilePos = tile.transform.position - new Vector3(0.5f, 0.5f, 0);
            startTileDict.Add(tilePos, tile);
        }
    }
}
