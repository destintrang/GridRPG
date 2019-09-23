using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public string actionName = "Interact";


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public virtual void OnInteract (GameObject unit)
    {
        Debug.Log("Interact!");
    }


    protected bool CanGive (GameObject unit)
    {
        return !unit.GetComponent<CharacterInventory>().IsFull();
    }


    public void SetInteractableTiles ()
    {
        Vector3Int thisCoord = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
        List<Vector3Int> directions = new List<Vector3Int> { new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0), new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0) };

        foreach (Vector3Int direction in directions)
        {
            if (TileManager.instance.allTiles.ContainsKey(thisCoord - direction))
            {
                TileManager.instance.allTiles[thisCoord - direction].GetComponent<TileAttributes>().SetInteractable(this);
            }
        }
    }

    protected void UnsetInteractableTiles ()
    {
        Vector3Int thisCoord = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
        List<Vector3Int> directions = new List<Vector3Int> { new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0), new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0) };

        foreach (Vector3Int direction in directions)
        {
            if (TileManager.instance.allTiles.ContainsKey(thisCoord - direction))
            {
                TileManager.instance.allTiles[thisCoord - direction].GetComponent<TileAttributes>().SetInteractable(null);
            }
        }
    }

}
