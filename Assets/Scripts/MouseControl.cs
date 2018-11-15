using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class MouseControl : MonoBehaviour {

    public enum MouseState {
        NORMAL,
        MOVING,
        ATTACKING,
		BATTLEFORECAST,
        INVENTORY,
        SELECTACTION,
        FINISHED,
        DISABLED
    };
    public MouseState currentState;

    ButtonManager buttons;
    BattleManager bm;
    GridLayout myGrid;
    Tilemap myMap;

    Dictionary<Vector3Int, GameObject> tiles;
    Dictionary<Vector3Int, GameObject> units;

    public GameObject unitOfInterest;
	Vector3Int originalLocation;
	Vector3Int chosenAttack;

    List<Vector3Int> possibleMoves;
    List<Vector3Int> possibleAttacks;
    List<Vector3Int> possibleAttacksSmall;


    List<GameObject> bluePool;
    List<GameObject> redPool;

    const float tileAnimMinSpeed = 2.5f;
    const float tileAnimMaxSpeed = 4.0f;

    public GameObject moveTile;
    public GameObject attackTile;


    //Singleton
    public static MouseControl instance;
    private void Awake()
    {
        instance = this;
    }


    // Use this for initialization
    void Start () {
        currentState = MouseState.NORMAL;
        buttons = ButtonManager.instance;
        bm = BattleManager.instance;
        myGrid = GameObject.Find("Grid").GetComponent<GridLayout>();
        tiles = TileManager.instance.allTiles;
        units = TileManager.instance.allUnits;

        bluePool = new List<GameObject>();
        redPool = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { Escape(); }

        if (TurnManager.instance.currentState == TurnManager.TurnState.BLUE)
        {  
            //The player only has control when it is their turn
            Vector3 clicked = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clicked.z = 0;
            Vector3Int coord = myGrid.LocalToCell(clicked);

            if (Input.GetMouseButtonDown(0))
            {

                switch (currentState)
                {
                    //The mouse's default state; the player can only press on ally units, enemy units, and empty spaces
                    case MouseState.NORMAL:
                        if (units.ContainsKey(coord))
                        {
                            unitOfInterest = units[coord];
                            StartWalking();
                            possibleMoves = unitOfInterest.GetComponent<UnitTracker>().PossibleMovement();
                            possibleMoves = possibleMoves.Distinct().ToList();  //Gotta make this list have no duplicates
                            possibleAttacks = unitOfInterest.GetComponent<UnitTracker>().PossibleAttacks(possibleMoves);
                            possibleAttacks = possibleAttacks.Distinct().ToList();  //Also gotta make this list have no dupes
                            currentState = MouseState.MOVING;
                            ToggleActionTiles(true);
                        }
                        else if (tiles.ContainsKey(coord))
                        {
                            print(coord);
                        }
                        break;
                    //State when the player presses on a ally that has not moved
                    case MouseState.MOVING:
                        if (unitOfInterest.tag == "Blue" && possibleMoves.Contains(coord) && (unitOfInterest.GetComponent<UnitTracker>().GetLocation() == coord || !units.ContainsKey(coord)))
                        {
                            //unitOfInterest.transform.position = coord + new Vector3(0.5f, 0.5f, 0);
                            possibleAttacksSmall = unitOfInterest.GetComponent<UnitTracker>().PossibleAttacksAtTile(coord);
                            //ToggleBlueTiles(false);

                            //Turn on buttons for all available actions to the unit
                            //Check if there are attackable enemies; if so, then the attack option will be displayed
                            ToggleActionTiles(false);
                            ToggleRedTilesSmall(true);
                            buttons.ToggleActionMenu(true);
                            //Move the unit, but save its original location, in case the player cancels
                            originalLocation = unitOfInterest.GetComponent<UnitTracker>().GetLocation();
                            unitOfInterest.GetComponent<UnitTracker>().SetLocation(coord);
                            Movement.instance.BeginMoving(unitOfInterest, ShortestPath.instance.FindPath(originalLocation, coord, "Blue"));
                            currentState = MouseState.SELECTACTION;
                        }
                        else
                        {
                            Escape();
                        }
                        break;
                    case MouseState.SELECTACTION:
                        if (possibleAttacksSmall.Contains(coord) && units.ContainsKey(coord) && units[coord].tag != tag)
                        {
                            ButtonManager.instance.ToggleActionMenu(false);

                            bm.DisplayBattleForecast(unitOfInterest, units[coord], unitOfInterest.GetComponent<UnitTracker>().GetLocation(), coord);
                            chosenAttack = coord;
                            currentState = MouseState.BATTLEFORECAST;
                        }
                        break;
                    //State when the player is deciding who to attack
                    case MouseState.ATTACKING:
                        if (possibleAttacksSmall.Contains(coord) && units.ContainsKey(coord) && units[coord].tag != tag)
                        {
                            bm.DisplayBattleForecast(unitOfInterest, units[coord], unitOfInterest.GetComponent<UnitTracker>().GetLocation(), coord);
                            chosenAttack = coord;
                            currentState = MouseState.BATTLEFORECAST;
                        }
                        else if (!possibleAttacksSmall.Contains(coord))
                        {
                            //Go back to the select action state
                            buttons.ToggleActionMenu(true);
                            currentState = MouseState.SELECTACTION;
                        }
                        break;
                    case MouseState.BATTLEFORECAST:
                        if (coord == chosenAttack)
                        {
                            //Once the player chooses to attack, their move is committed, and is written to the game
                            int range = (int)(Mathf.Abs(unitOfInterest.GetComponent<UnitTracker>().GetLocation().x - coord.x) + Mathf.Abs(unitOfInterest.GetComponent<UnitTracker>().GetLocation().y - coord.y));
                            ToggleRedTilesSmall(false);
                            unitOfInterest.GetComponent<UnitTracker>().MoveTo(unitOfInterest.GetComponent<UnitTracker>().GetLocation());
                            bm.PlayerAttacks(unitOfInterest, units[chosenAttack], range);
                            currentState = MouseState.DISABLED;
                        }
                        else
                        {
                            bm.HideBattleForecast();
                            currentState = MouseState.ATTACKING;
                        }
                        break;
                }
            }

            if (currentState == MouseState.FINISHED)
            {
                TurnManager.instance.FinishUnit();
                Reset();
            }
        }
	}


    void Escape ()
    {
        switch (currentState)
        {
            case MouseState.NORMAL:
                break;
            case MouseState.MOVING:
                SmallReset();
                break;
            case MouseState.SELECTACTION:
                ButtonManager.instance.ToggleActionMenu(false);
                if (Movement.instance != null) { Movement.instance.moving = false; }
                ResetUnit();
                currentState = MouseControl.MouseState.MOVING;
                ToggleRedTilesSmall(false);
                ToggleActionTiles(true);
                break;
            case MouseState.ATTACKING:
                //Go back to the select action state
                buttons.ToggleActionMenu(true);
                currentState = MouseState.SELECTACTION;
                break;
            case MouseState.BATTLEFORECAST:
                //Go back to the attacking state
                bm.HideBattleForecast();
                currentState = MouseState.ATTACKING;
                break;
        }
    }


    public bool CanAttack ()
    {
        foreach (Vector3Int possibleAttack in possibleAttacksSmall)
        {
            if (units.ContainsKey(possibleAttack) && units[possibleAttack].tag != unitOfInterest.tag)
            {
                return true;
            }
        }
        return false;
    }


    public void SmallReset()
    {
        ToggleActionTiles(false);
        StopWalking();
        currentState = MouseControl.MouseState.NORMAL;
    }

    public void Reset ()
    {
        ToggleActionTiles(false);
        ToggleRedTilesSmall(false);
        StopWalking();
        currentState = MouseControl.MouseState.NORMAL;
    }


    public void ToggleActionTiles (bool toggle)
    {
        ToggleBlueTiles(toggle);
        ToggleRedTiles(toggle);
    }

    public void ToggleBlueTiles (bool toggle = false)
    {
        if (toggle)
        {
            if (possibleMoves.Count > bluePool.Count)
            {
                for (int b = bluePool.Count; b < possibleMoves.Count; b++)
                {
                    bluePool.Add(Instantiate(moveTile, this.transform));
                }
            }

            for (int x = 0; x < possibleMoves.Count; x++)
            {
                //tiles[possibleMoves[x]].GetComponent<SpriteRenderer>().color = blue;

                bluePool[x].transform.position = possibleMoves[x] + new Vector3(0.5f, 0.5f, 0);
                bluePool[x].GetComponent<Animator>().speed = Random.Range(tileAnimMinSpeed, tileAnimMaxSpeed);
                bluePool[x].GetComponent<Animator>().Play("BlueTileAnimation");
            }
        } else
        {
            for (int x = 0; x < possibleMoves.Count; x++)
            {
                bluePool[x].transform.position = new Vector3(-1, -1, 0);
            }
        }
    }

    public void ToggleRedTiles (bool toggle)
    {
        List<Vector3Int> difference = possibleAttacks.Except(possibleMoves).ToList();

        if (toggle)
        {
            if (difference.Count > redPool.Count)
            {
                for (int r = redPool.Count; r < possibleMoves.Count; r++)
                {
                    redPool.Add(Instantiate(attackTile, this.transform));
                }
            }

            for (int x = 0; x < difference.Count; x++)
            {
                //tiles[possibleMoves[x]].GetComponent<SpriteRenderer>().color = blue;

                redPool[x].transform.position = difference[x] + new Vector3(0.5f, 0.5f, 0);
                redPool[x].GetComponent<Animator>().speed = Random.Range(tileAnimMinSpeed, tileAnimMaxSpeed);
                redPool[x].GetComponent<Animator>().Play("RedTileAnimation");
            }
        }
        else
        {
            for (int x = 0; x < difference.Count; x++)
            {
                redPool[x].transform.position = new Vector3(-1, -1, 0);
            }
        }
    }

    public void ToggleRedTilesSmall (bool toggle)
    {
        if (toggle)
        {
            if (possibleAttacksSmall.Count > redPool.Count)
            {
                for (int r = redPool.Count; r < possibleMoves.Count; r++)
                {
                    redPool.Add(Instantiate(attackTile, this.transform));
                }
            }

            for (int x = 0; x < possibleAttacksSmall.Count; x++)
            {
                //tiles[possibleMoves[x]].GetComponent<SpriteRenderer>().color = blue;

                redPool[x].transform.position = possibleAttacksSmall[x] + new Vector3(0.5f, 0.5f, 0);
                redPool[x].GetComponent<Animator>().speed = Random.Range(tileAnimMinSpeed, tileAnimMaxSpeed);
                redPool[x].GetComponent<Animator>().Play("RedTileAnimation");
            }
        }
        else
        {
            for (int x = 0; x < possibleAttacksSmall.Count; x++)
            {
                redPool[x].transform.position = new Vector3(-1, -1, 0);
            }
        }
    }

    public void ResetUnit ()
    {
        unitOfInterest.GetComponent<UnitTracker>().MoveTo(originalLocation);
        unitOfInterest.GetComponent<Animator>().SetTrigger("Down");
    }

    void StartWalking ()
    {
        unitOfInterest.GetComponent<Animator>().SetTrigger("Down");
        unitOfInterest.GetComponent<Animator>().SetBool("Selected", true);
    }
    void StopWalking ()
    {
        unitOfInterest.GetComponent<Animator>().SetBool("Selected", false);
        unitOfInterest.GetComponent<Animator>().SetTrigger("Idle");
        unitOfInterest.GetComponent<Animator>().SetBool("Focus", false);
    }
}
