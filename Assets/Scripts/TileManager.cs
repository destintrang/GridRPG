using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {


    public Dictionary<Vector3Int, GameObject> allTiles;
    public Dictionary<Vector3Int, GameObject> allUnits;

    public CameraControl battleCamera;


    //Singleton
    public static TileManager instance;


    // Use this for initialization
    void Awake () {
        instance = this;
        allTiles = new Dictionary<Vector3Int, GameObject>();
        allUnits = new Dictionary<Vector3Int, GameObject>();

        RecordEnemyUnits();
    }

    private void Start ()
    {
        //Put all the tiles on the map into the dictionary
        RecordTiles();

        //Set the bounds of where the camera can go
        battleCamera.SetBounds(GetLevelDimensions());

        foreach (Interactable i in FindObjectsOfType<Interactable>())
        {
            i.SetInteractableTiles();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}


    public Vector2Int GetLevelDimensions ()
    {
        int x = 0;
        int y = 0;
        Vector3Int index;

        while (true)
        {
            index = new Vector3Int(x, 0, 0);
            if (allTiles.ContainsKey(index))
            {
                x++;
            } else
            {
                break;
            }
        }
        while (true)
        {
            index = new Vector3Int(0, y, 0);
            if (allTiles.ContainsKey(index))
            {
                y++;
            }
            else
            {
                break;
            }
        }

        return new Vector2Int(x, y);
    }


    //Record the tiles of all the player units
    public void RecordPlayerUnits()
    {
        foreach (UnitTracker unit in FindObjectsOfType<UnitTracker>())
        {
            if (unit.gameObject.tag == "Blue")
            {
                unit.SnapToGrid();
            }
        }
    }

    //Record the tiles of all non-player units on the map
    private void RecordEnemyUnits ()
    {
        foreach (UnitTracker unit in FindObjectsOfType<UnitTracker>())
        {
            if (unit.gameObject.tag != "Blue")
            {
                unit.SnapToGrid();
            }
        }
    }


    private void RecordTiles ()
    {
        foreach (TileAttributes tile in FindObjectsOfType<TileAttributes>())
        {
            Vector3Int coordinate = (GameObject.Find("Grid").GetComponent<GridLayout>().LocalToCell(tile.transform.position));
            allTiles.Add(coordinate, tile.gameObject);
        }
    }


}
