using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestPath : MonoBehaviour {


    List<Vector3Int> directions = new List<Vector3Int>(new Vector3Int[] { new Vector3Int(1, 0 ,0), new Vector3Int(-1, 0, 0), new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0) });

    //Singleton
    public static ShortestPath instance;
    private void Awake()
    {
        instance = this;
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<Vector3Int> FindPath (Vector3Int start, Vector3Int goal, string team)
    {
        if (start == goal) { return new List<Vector3Int> { }; }

        Vector3Int currentTile;
        List<Vector3Int> open = new List<Vector3Int>();
        Dictionary<Vector3Int, float> distances = new Dictionary<Vector3Int, float>();
        Dictionary<Vector3Int, Vector3Int> previousTiles = new Dictionary<Vector3Int, Vector3Int>();
        foreach (KeyValuePair<Vector3Int, GameObject> tile in TileManager.instance.allTiles)
        {
            distances[tile.Key] = Mathf.Infinity;
        }

        open.Add(start);
        distances[start] = 0;

        while (open.Count > 0)
        {
            currentTile = ClosestTile(open);

            foreach (Vector3Int direction in directions)
            {
                Vector3Int adjacent = currentTile + direction;

                if (TileManager.instance.allTiles.ContainsKey(adjacent) && TileManager.instance.allTiles[adjacent].GetComponent<TileAttributes>().walkable &&
                    (!TileManager.instance.allUnits.ContainsKey(adjacent) || TileManager.instance.allUnits[adjacent].tag == team))
                {
                    if (TileManager.instance.allTiles[adjacent].GetComponent<TileAttributes>().moveCost + distances[currentTile] < distances[adjacent])
                    {
                        previousTiles[adjacent] = currentTile;
                        distances[adjacent] = distances[currentTile] + TileManager.instance.allTiles[adjacent].GetComponent<TileAttributes>().moveCost;
                        open.Add(adjacent);
                    }
                }
            }
            open.Remove(currentTile);
        }

        List<Vector3Int> path = new List<Vector3Int>();
        currentTile = goal;
        while(true)
        {
            path.Insert(0, currentTile);
            currentTile = previousTiles[currentTile];
            if (currentTile == start) { break; }
        }

        return path;
    }

    Vector3Int ClosestTile (List<Vector3Int> allOpenTiles)
    {
        int shortest = 666;
        Vector3Int returnedTile = new Vector3Int();

        foreach (Vector3Int tile in allOpenTiles)
        {
            if (TileManager.instance.allTiles[tile].GetComponent<TileAttributes>().moveCost < shortest) 
            {
                shortest = TileManager.instance.allTiles[tile].GetComponent<TileAttributes>().moveCost;
                returnedTile = tile;
            }
        }

        return returnedTile;
    }

}
