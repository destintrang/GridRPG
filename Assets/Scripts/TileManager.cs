using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {


    public Dictionary<Vector3Int, GameObject> allTiles;
    public Dictionary<Vector3Int, GameObject> allUnits;

    //Singleton
    public static TileManager instance;


    // Use this for initialization
    void Awake () {
        instance = this;
        allTiles = new Dictionary<Vector3Int, GameObject>();
        allUnits = new Dictionary<Vector3Int, GameObject>();
    }

    private void Start ()
    {
        Camera.main.GetComponent<CameraControl>().SetBounds(GetLevelDimensions());        
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


}
