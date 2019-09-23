using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAttributes : MonoBehaviour {


    public int moveCost;
    public int defenseBonus;
    public int evasionBonus;

    public bool walkable;

    private Interactable interactable = null;
    public void SetInteractable (Interactable i) { interactable = i; }
    public Interactable GetInteractable () { return interactable; }


    // Use this for initialization
    void Start () {
        //Vector3Int coordinate = (GameObject.Find("Grid").GetComponent<GridLayout>().LocalToCell(transform.position));
        //GameObject.Find("Tile Manager").GetComponent<TileManager>().allTiles.Add(coordinate, this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
