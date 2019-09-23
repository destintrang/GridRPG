using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTracker : MonoBehaviour {


    GridLayout myGrid;
    TileManager manager;

    Vector3Int currentLocation;


    //This is where we will keep track of whether or not a unit will be deployed in a battle
    //By default, units will start off undeployed
    private bool deployed = false;
    public bool isDeployed () { return deployed; }
    public void Deploy() { deployed = true; }
    public void Undeploy() { deployed = false; }


    // Use this for initialization
    void Start ()
    {

        //Invoke("SnapToGrid", 0.0000000001f);  //lol
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public List<Vector3Int> PossibleMovement ()
    {
        int movement = GetComponent<CharacterStats>().GetMovement();

        List<Vector3Int> possibleMoves = CheckAdjacent(currentLocation, movement);

        return possibleMoves;
    }


    public List<Vector3Int> PossibleAttacks (List<Vector3Int> possibleMoves)
    {
        List<Vector3Int> attacks = new List<Vector3Int>();
        //List<int> ranges = GetComponent<CharacterStats>().GetRange();

        
        foreach (Vector3Int move in possibleMoves)
        {
            if (!TileManager.instance.allUnits.ContainsKey(move))
            {
                foreach (Vector3Int attack in PossibleAttacksAtTile(move))
                {
                    attacks.Add(attack);
                }
            }
        }

        return attacks;
    }

    public List<Vector3Int> PossibleAttacksAtTile (Vector3Int tile)
    {
        List<Vector3Int> attacks = new List<Vector3Int>();
        List<int> ranges = GetComponent<CharacterStats>().GetRange();

        foreach (int range in ranges)
        {
            Vector3Int toCheck = tile + new Vector3Int(0, range, 0);

            int counter = 0;
            while (counter < range)
            {
                if (manager.allTiles.ContainsKey(toCheck))
                {
                    attacks.Add(toCheck);
                }
                toCheck += new Vector3Int(1, -1, 0);
                counter++;
            }

            counter = 0;
            while (counter < range)
            {
                if (manager.allTiles.ContainsKey(toCheck))
                {
                    attacks.Add(toCheck);
                }
                toCheck += new Vector3Int(-1, -1, 0);
                counter++;
            }

            counter = 0;
            while (counter < range)
            {
                if (manager.allTiles.ContainsKey(toCheck))
                {
                    attacks.Add(toCheck);
                }
                toCheck += new Vector3Int(-1, 1, 0);
                counter++;
            }

            counter = 0;
            while (counter < range)
            {
                if (manager.allTiles.ContainsKey(toCheck))
                {
                    attacks.Add(toCheck);
                }
                toCheck += new Vector3Int(1, 1, 0);
                counter++;
            }
        }

        return attacks;
    }


    List<Vector3Int> CheckAdjacent (Vector3Int coord, int movement)
    {
        //print("Examining" + coord);
        List<Vector3Int> toCheck = new List<Vector3Int>();
        List<Vector3Int> canMove = new List<Vector3Int>();

        Vector3Int temp;
        temp = coord + new Vector3Int(1, 0, 0);
        toCheck.Add(temp);
        temp = coord + new Vector3Int(0, 1, 0);
        toCheck.Add(temp);
        temp = coord + new Vector3Int(-1, 0, 0);
        toCheck.Add(temp);
        temp = coord + new Vector3Int(0, -1, 0);
        toCheck.Add(temp);

        for (int x = 0; x < toCheck.Count; x++)
        {
            //Checks first to see if adjacent tiles are valid tiles on the map
            //Next checks to see if that tile is walkable
            //Then checks to see if an enemy occupies adjacent tiles
            //Finally checks to see if the character can afford to move there
            if (manager.allTiles.ContainsKey(toCheck[x]) &&
                manager.allTiles[toCheck[x]].GetComponent<TileAttributes>().walkable &&
                !(manager.allUnits.ContainsKey(toCheck[x]) && manager.allUnits[toCheck[x]].tag != tag) &&
                movement >= manager.allTiles[toCheck[x]].GetComponent<TileAttributes>().moveCost)
            {
                //if (manager.allUnits.ContainsKey(toCheck[x]) && manager.allUnits[toCheck[x]].tag != tag)
                canMove.Add(toCheck[x]);
                //If the character has leftover movement, check where else it can go
                if (movement > manager.allTiles[toCheck[x]].GetComponent<TileAttributes>().moveCost)
                {
                    List<Vector3Int> tempList = CheckAdjacent(toCheck[x], (movement - manager.allTiles[toCheck[x]].GetComponent<TileAttributes>().moveCost));
                    for (int y = 0; y < tempList.Count; y++)
                    {
                        if (!canMove.Contains(tempList[y]))
                        {
                            canMove.Add(tempList[y]);
                        }
                    }
                }
            }
        }

        //print("Done examing" + coord);
        return canMove;
    }


    public void MoveTo (Vector3Int tile)
    {
        transform.position = tile + new Vector3(0.5f, 0.5f, 0);
        SetLocation(tile);
    }


    public void SetLocation (Vector3Int tile)
    {
        manager.allUnits.Remove(currentLocation);
        manager.allUnits.Add(tile, this.gameObject);
        currentLocation = tile;
    }


    public void SnapToGrid ()
    {
        myGrid = GameObject.Find("Grid").GetComponent<GridLayout>();
        manager = GameObject.Find("Tile Manager").GetComponent<TileManager>();

        Vector3Int coord = myGrid.LocalToCell(transform.position);
        Vector3 targetLocation = coord;
        targetLocation.x += 0.5f;
        targetLocation.y += 0.5f;

        transform.position = targetLocation;
        currentLocation = coord;
        manager.allUnits.Add(coord, this.gameObject);
    }


    public void FixPosition ()
    {
        Vector3Int coord = myGrid.LocalToCell(transform.position);
        Vector3 targetLocation = coord;
        targetLocation.x += 0.5f;
        targetLocation.y += 0.5f;

        transform.position = targetLocation;
    }


    public Vector3Int GetLocation ()
    {
        return currentLocation;
    }


    public void Die ()
    {
        manager.allUnits.Remove(currentLocation);
        SoundEffectsManager.instance.PlayDeath();
        this.gameObject.SetActive(false);
    }
}
